using Microsoft.EntityFrameworkCore;
namespace ZoneApi.Model
{
    public class ZoneContext :DbContext
    {

        public ZoneContext (DbContextOptions<ZoneContext> options) : base(options)
        {

        }
        public DbSet<Zone> Zones { get; set; }
    }
}
