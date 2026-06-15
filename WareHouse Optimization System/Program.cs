using Microsoft.EntityFrameworkCore;
using Serilog; 
using VendorManagement.Services;
using Vendor_Management.Services;
using WareHouse_Optimization_System.Db;
using WareHouse_Optimization_System.Services.Implementations;
using WareHouse_Optimization_System.Services.Interfaces;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting web host"); 

    var builder = WebApplication.CreateBuilder(args);


    builder.Host.UseSerilog();

    builder.Services.AddScoped<ZoneService>();
    builder.Services.AddScoped<IStockService, StockService>();
    builder.Services.AddScoped<TransactionService>();
    builder.Services.AddScoped<IVendorService, VendorService>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();


    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages(); 

                                                // DATABASE SETUP

    var connectionString = builder.Configuration.GetConnectionString("WarehouseDB")
        ?? throw new InvalidOperationException("Connection string 'WarehouseDB' not found in configuration files");

    builder.Services.AddDbContext<WarehouseDbContext>(options =>
        options.UseSqlServer(connectionString));

    var app = builder.Build();

   
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

    app.Run(); // Start the app
}
catch (Exception ex)
{
   
    Log.Fatal(ex, "Application failed to start correctly check configurations");
    throw;
}
finally
{
  
    Log.CloseAndFlush();
}