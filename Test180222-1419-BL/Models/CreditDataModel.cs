using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test180222_1419_BL.Models
{
    public class CreditDataModel
    {
        public int applicationId { get; set; }
        public string customerName { get; set; }
        public string source { get; set; }
        public string bureau { get; set; }
        public int minPaymentPercentage { get; set; }
        public TradeLineModel[] tradelines { get; set; }
    }
}
