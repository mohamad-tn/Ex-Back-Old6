using System;
using System.Collections.Generic;
using System.Data.Common;
using Abp.Data;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Bwr.Exchange.EntityFrameworkCore;
using Bwr.Exchange.EntityFrameworkCore.Seed;
using Bwr.Exchange.MultiTenancy;

namespace Bwr.Exchange.Migrate
{
    public class ExchangeMigrateExecuter : ITransientDependency
    {
        private readonly AbpZeroDbMigrator _migrator;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IDbPerTenantConnectionStringResolver _connectionStringResolver;

        public ExchangeMigrateExecuter(
            AbpZeroDbMigrator migrator,
            IRepository<Tenant> tenantRepository,
            
            IDbPerTenantConnectionStringResolver connectionStringResolver)
        {
            _migrator = migrator;
            _tenantRepository = tenantRepository;
            _connectionStringResolver = connectionStringResolver;
        }

        public bool Run(string dbName)
        {
            var currenctConnectionString = _connectionStringResolver.GetNameOrConnectionString(new ConnectionStringResolveArgs(MultiTenancySides.Host));
            var nextCunnectionString = currenctConnectionString.Replace("EX2023", dbName);
            var hostConnStr = CensorConnectionString(nextCunnectionString);
            if (hostConnStr.IsNullOrWhiteSpace())
            {
                return false;
            }

            ConnectionStringHelper.GetConnectionString(hostConnStr);
            try
            {
                _migrator.CreateOrMigrateForHost(SeedHelper.SeedHostDb);
            }
            catch (Exception ex)
            {
                return false;
            }

            var migratedDatabases = new HashSet<string>();
            var tenants = _tenantRepository.GetAllList(t => t.ConnectionString != null && t.ConnectionString != "");
            for (var i = 0; i < tenants.Count; i++)
            {
                var tenant = tenants[i];
                SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString);
                if (!migratedDatabases.Contains(tenant.ConnectionString))
                {
                    try
                    {
                        _migrator.CreateOrMigrateForTenant(tenant);
                    }
                    catch (Exception ex)
                    {
                        //"An error occured during migration of tenant database:";
                    }

                    migratedDatabases.Add(tenant.ConnectionString);
                }
                else
                {
                    //This database has already migrated before (you have more than one tenant in same database). Skipping it...."
                }

            }

            //All databases have been migrated.;

            return true;
        }

        private static string CensorConnectionString(string connectionString)
        {
            var builder = new DbConnectionStringBuilder { ConnectionString = connectionString };
            var keysToMask = new[] { "password", "pwd", "user id", "uid" };

            foreach (var key in keysToMask)
            {
                if (builder.ContainsKey(key))
                {
                    builder[key] = "*****";
                }
            }

            return builder.ToString();
        }
    }
}
