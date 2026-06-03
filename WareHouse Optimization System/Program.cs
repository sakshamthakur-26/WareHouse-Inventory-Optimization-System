using Microsoft.EntityFrameworkCore;
using WareHouse_Optimization_System.Db;
using WareHouse_Optimization_System.Services.Implementations;
using WareHouse_Optimization_System.Services.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddScoped<ZoneService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
var connectionString = builder.Configuration.GetConnectionString("WarehouseDB")
    ?? throw new InvalidOperationException("Connection string 'WarehouseDB' not found in configuration files.");
builder.Services.AddDbContext<WarehouseDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
try
{
    Log.Information("Starting web host");

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

    app.MapRazorPages();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
