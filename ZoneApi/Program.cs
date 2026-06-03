using Microsoft.EntityFrameworkCore;
using Zone.Services;

namespace ZoneApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            

            builder.Services.AddScoped<IZoneService, ZoneService>();

            builder.Services.AddDbContext<Model.ZoneContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ZoneDb")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
