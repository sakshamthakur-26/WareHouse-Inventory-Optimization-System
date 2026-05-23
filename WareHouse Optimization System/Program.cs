using Microsoft.EntityFrameworkCore;
using WareHouse_Optimization_System.Db;
using WareHouse_Optimization_System.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddScoped<ZoneService>();
//builder.Services.AddScoped<StockService>();
//builder.Services.AddScoped<TransactionService>();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<WarehouseDbContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("WarehouseDB")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
