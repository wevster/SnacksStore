using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SnacksStore.Models
{
    public class logUserPurchases
    {
        
        public int LogPricesID { get; set; }
        public int ProductID { get; set; }
        public string PrevPrice { get; set; }
        public string NewPrice { get; set; }
        public string User { get; set; }
        public string ProductName { get; set; }
        public DateTime Date { get; set; }
    }
}
