using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SnacksStore.Models
{
    public class Products
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int TypeProduct { get; set; }
        public string Price { get; set; }
        public string Photo { get; set; }
    }
}
