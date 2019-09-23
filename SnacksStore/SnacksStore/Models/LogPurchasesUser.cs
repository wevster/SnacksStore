using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SnacksStore.Models
{
    public class LogPurchasesUser
    {
        
        public int LogPurchaseID { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public string User { get; set; }
        public DateTime Date { get; set; }
    }
}
