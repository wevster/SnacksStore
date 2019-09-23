using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SnacksStore.Models
{
    public class LogPurchases
    {
        [Key]
        public int LogPurchaseID { get; set; }
        public int Quantity { get; set; }
        public int ProductID { get; set; }
        public int UserID { get; set; }
        public DateTime Date { get; set; }
    }
}
