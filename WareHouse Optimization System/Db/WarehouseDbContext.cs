using Microsoft.EntityFrameworkCore;
using WareHouse_Optimization_System.Models;

namespace WareHouse_Optimization_System.Db
{
    public class WarehouseDbContext : DbContext
    {
        public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options)
        {
        }
        
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
