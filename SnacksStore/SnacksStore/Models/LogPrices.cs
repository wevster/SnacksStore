using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SnacksStore.Models
{
    public class LogPrices
    {
        [Key]
        public int LogPricesID { get; set; }
        public int ProductID { get; set; }
        public string PrevPrice { get; set; }
        public string NewPrice { get; set; }
        public int UserID { get; set; }
        public DateTime Date { get; set; }
    }
}
