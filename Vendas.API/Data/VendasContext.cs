using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vendas.API.Entities;

namespace Vendas.API.Data
{
    public class VendasContext : DbContext, IVendasContext
    {
        public VendasContext(DbContextOptions<VendasContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().Property(p => p.TotalPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<OrderItem>().Property(p => p.UnitPrice).HasColumnType("decimal(18,2)");
        }


    }
}