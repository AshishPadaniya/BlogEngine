using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment1.Models
{
    public class Assignment1DataContext : DbContext
    {
        public Assignment1DataContext(DbContextOptions<Assignment1DataContext> options)
            : base(options)
        {

        }

        
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Blogpost> Blogposts { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
