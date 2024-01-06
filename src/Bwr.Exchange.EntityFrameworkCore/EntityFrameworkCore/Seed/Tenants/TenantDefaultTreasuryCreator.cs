using Bwr.Exchange.Settings.Treasuries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Bwr.Exchange.EntityFrameworkCore.Seed.Tenants
{
    public class TenantDefaultTreasuryCreator
    {
        private readonly ExchangeDbContext _context;
        private readonly int _tenantId;

        public TenantDefaultTreasuryCreator(ExchangeDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateTreasury();
        }

        private void CreateTreasury()
        {
            var treasury = _context.Treasuries.IgnoreQueryFilters().FirstOrDefault(x => x.Id == _tenantId && x.Name.Trim() == "الصندوق الرئيسي");
            if (treasury == null)
            {
                treasury = new Treasury { Name = "الصندوق الرئيسي" };
                treasury.TenantId = _tenantId;
                _context.Treasuries.Add(treasury);
                _context.SaveChanges();
            }
        }
    }
}
