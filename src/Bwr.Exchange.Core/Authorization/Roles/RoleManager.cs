using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Zero.Configuration;
using Bwr.Exchange.Authorization.Users;
using System;
using System.Threading.Tasks;
using System.Linq;
using Abp.UI;
using Abp.Localization;

namespace Bwr.Exchange.Authorization.Roles
{
    public class RoleManager : AbpRoleManager<Role, User>
    {
        private readonly ILocalizationManager _localizationManager;
        public RoleManager(
            RoleStore store,
            IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<AbpRoleManager<Role, User>> logger,
            IPermissionManager permissionManager,
            ICacheManager cacheManager,
            ILocalizationManager localizationManager,
            IUnitOfWorkManager unitOfWorkManager,
            IRoleManagementConfig roleManagementConfig,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository)
            : base(
                  store,
                  roleValidators,
                  keyNormalizer,
                  errors, logger,
                  permissionManager,
                  cacheManager,
                  unitOfWorkManager,
                  roleManagementConfig,
                organizationUnitRepository,
                organizationUnitRoleRepository)
        {
            _localizationManager = localizationManager;
        }

        public override Task SetGrantedPermissionsAsync(Role role, IEnumerable<Permission> permissions)
        {
            CheckPermissionsToUpdate(role, permissions);
            return base.SetGrantedPermissionsAsync(role, permissions);
        }

        public virtual async Task<Role> GetRoleByIdAsync(long userId)
        {
            var role = await FindByIdAsync(userId.ToString());
            if (role == null)
            {
                throw new ApplicationException("There is no user with id: " + userId);
            }

            return role;
        }

        private void CheckPermissionsToUpdate(Role role, IEnumerable<Permission> permissions)
        {
            if (role.Name == StaticRoleNames.Host.Admin &&
                (!permissions.Any(p => p.Name == PermissionNames.Pages_Roles_Edit) ||
                 !permissions.Any(p => p.Name == PermissionNames.Pages_Users_ChangePermissions)))
            {
                throw new UserFriendlyException(L("YouCannotRemoveUserRolePermissionsFromAdminRole"));
            }
        }

        private new string L(string name)
        {
            return _localizationManager.GetString(ExchangeConsts.LocalizationSourceName, name);
        }
    }
}
