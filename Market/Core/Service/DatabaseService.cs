using Market.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.Service
{
    public class DatabaseService : DbContext
    {
        public DatabaseService(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Nomenclature> Nomenclatures { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
