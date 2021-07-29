using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RedisDemoMVC.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("Default")
        {
        }

        public DbSet<Post> Posts { get; set; }
    }
}