using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnacksStore.Models
{
    public class ProductsStock
    {
        
        public int ProductID { get; set; }
        public int ProductType { get; set; }
        public string ProductName { get; set; }
        public string Details { get; set; }
        public string Price { get; set; }
        public string Photo { get; set; }
        public int quantity { get; set; }
        public bool available { get; set; }

    }
}
