using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test180222_1419_BL.Models
{
    public class TradeLineModel
    {
        public long tradelineId { get; set; }
        public string accountNumber { get; set; }
        public int balance { get; set; }
        public int monthlyPayment { get; set; }
        public string type { get; set; }
        public bool isMortgage { get; set; }
    }
}
