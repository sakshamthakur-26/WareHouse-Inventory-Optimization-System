using Microsoft.EntityFrameworkCore;
using WareHouse_Optimization_System.Db;
using WareHouse_Optimization_System.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ZoneService>();
builder.Services.AddScoped<StockService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("WarehouseDB")
    ?? throw new InvalidOperationException("Connection string 'WarehouseDB' not found in configuration files.");
builder.Services.AddDbContext<WarehouseDbContext>(options =>
    options.UseSqlServer(connectionString));


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
    pattern: "api/{controller=StockItem}/{action=GetStockItems}");

app.Run();
