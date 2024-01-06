using System;

namespace Bwr.Exchange.Models.Statments.Clients
{
    public class TotalClientBalancePdf
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }

        public string Currency0 { get; set; }
        public double Balance0 { get; set; }

        public string Currency1 { get; set; }
        public double Balance1 { get; set; }

        public string Currency2 { get; set; }
        public double Balance2 { get; set; }

        public string Currency3 { get; set; }
        public double Balance3 { get; set; }

        public string Currency4 { get; set; }
        public double Balance4 { get; set; }

        public double ForHim
        {
            get
            {
                double forHim = 0;
                forHim += Balance0 < 0 ? Balance0 : 0;
                forHim += Balance1 < 0 ? Balance1 : 0;
                forHim += Balance2 < 0 ? Balance2 : 0;
                forHim += Balance3 < 0 ? Balance3 : 0;
                forHim += Balance4 < 0 ? Balance4 : 0;
                return forHim;
            }
        }
        public double OnHim
        {
            get
            {
                double onHim = 0;
                onHim += Balance0 > 0 ? Balance0 : 0;
                onHim += Balance1 > 0 ? Balance1 : 0;
                onHim += Balance2 > 0 ? Balance2 : 0;
                onHim += Balance3 > 0 ? Balance3 : 0;
                onHim += Balance4 > 0 ? Balance4 : 0;
                return onHim;
            }
        }
        public string Total
        {
            get
            {
                var total = ForHim + OnHim;
                return total < 0 ? $"{Math.Abs(total)}/له" : $"{total}/عليه";
            }
        }
    }
}
