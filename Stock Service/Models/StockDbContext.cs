using Microsoft.EntityFrameworkCore;
namespace Stock_Service.Models
{
    public class StockDbContext:DbContext
    {
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
        {
        }
        public DbSet<StockItem> StockItems { get; set; }
    }
}
