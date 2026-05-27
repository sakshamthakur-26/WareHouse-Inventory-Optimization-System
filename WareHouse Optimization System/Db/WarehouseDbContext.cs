using Microsoft.EntityFrameworkCore;
using WareHouse_Optimization_System.Models;

namespace WareHouse_Optimization_System.Db
{
    public class WarehouseDbContext : DbContext
    {
        public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        // TEMPORARY: Hardcode just to generate the migration
        //        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Initial catalog=WarehouseDB;User Id=sa;Password=12345678;TrustServerCertificate=True;");
        //    }
        //}
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
