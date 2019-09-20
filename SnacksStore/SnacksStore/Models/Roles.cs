using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SnacksStore.Models
{
    public class Roles
    {
        [Key]
        public int RoleID { get; set; }
        public string Description { get; set; }
    }
}
