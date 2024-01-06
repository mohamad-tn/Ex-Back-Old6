using Abp.Application.Editions;
using Abp.Application.Features;
using Bwr.Exchange.Editions;
using Bwr.Exchange.Settings.Treasuries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bwr.Exchange.EntityFrameworkCore.Seed.Host
{
    public class DefaultTreasuryCreator
    {
        private readonly ExchangeDbContext _context;

        public DefaultTreasuryCreator(ExchangeDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateTreasury();
        }

        private void CreateTreasury()
        {
            var treasury = _context.Treasuries.IgnoreQueryFilters().FirstOrDefault(e => e.Name.Trim() == "الصندوق الرئيسي");
            if (treasury == null)
            {
                treasury = new Treasury { Name = "الصندوق الرئيسي" };
                _context.Treasuries.Add(treasury);
                _context.SaveChanges();

                /* Add desired features to the standard edition, if wanted... */
            }
        }
    }

}
