using Microsoft.EntityFrameworkCore;
using WareHouse_Optimization_System.Db;
using WareHouse_Optimization_System.Services.Implementations;
using WareHouse_Optimization_System.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);   //creates app config and service container , prepares di

// Add services to the container.
builder.Services.AddScoped<IZoneService,ZoneService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<ITransactionService,TransactionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("WareHouse") //get connstring
    ?? throw new InvalidOperationException("Connection string 'WareHouse' not found in configuration files.");
builder.Services.AddDbContext<WarehouseDbContext>(options =>
    options.UseSqlServer(connectionString));  //use sqlserver


var app = builder.Build(); //convert configurat

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}


app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller=StockItem}/{action=GetStockItems}");

app.Run();