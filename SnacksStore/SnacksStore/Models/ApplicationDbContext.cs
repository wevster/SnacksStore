using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SnacksStore.Models;

namespace SnacksStore.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
            { }

        public DbSet<Products> Products { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Likes> Likes { get; set; }
        public DbSet<TypeProduct> TypeProduct { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
    }
}
