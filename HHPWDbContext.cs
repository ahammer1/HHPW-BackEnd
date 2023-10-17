using HHPW_BackEnd.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HHPW_BackEnd
{
    public class HHPWDbContext : DbContext
    {
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderStatus> Status { get; set; }
        public DbSet<PaymentType> PaymentType { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Employee> Employee { get; set; }

        public DbSet<OrderProducts> OrderProducts { get; set; }

        public HHPWDbContext(DbContextOptions<HHPWDbContext> context) : base(context)
        {

        }
    }
}