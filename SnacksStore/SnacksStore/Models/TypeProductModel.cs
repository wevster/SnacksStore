using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SnacksStore.Models
{
    public class TypeProduct
    {
        [Key]
        public int TypeProductID { get; set; }
        public string Details { get; set; }
        
    }
}
