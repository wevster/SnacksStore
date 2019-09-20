﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SnacksStore.Models
{
    public class Stock
    {
        [Key]
        public int StockID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public bool Available { get; set; }


    }
}
