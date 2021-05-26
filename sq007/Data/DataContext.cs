using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using sq007.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sq007.Data
{
    public class DataContext : IdentityDbContext<Contact>
    {
        private readonly DbContextOptions _options;
        public DataContext(DbContextOptions options) : base(options)
        {
            _options = options;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Address> Addresses { get; set; }
    }
}
