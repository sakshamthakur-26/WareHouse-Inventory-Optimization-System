using Microsoft.EntityFrameworkCore;
using VendorManagement.Models;
using WareHouse_Optimization_System.Models;

namespace WareHouse_Optimization_System.Db
{
    public class WarehouseDbContext : DbContext
    {
        // This constructor automatically receives the configuration from Program.cs
        public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options)
        {
        }



        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}