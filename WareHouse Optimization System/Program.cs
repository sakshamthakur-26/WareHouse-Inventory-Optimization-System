using Microsoft.EntityFrameworkCore;
using WareHouse_Optimization_System.Db;
using WareHouse_Optimization_System.Services.Implementations;
using WareHouse_Optimization_System.Services.Interfaces;
using WareHouse_Optimization_System.Middlewares;
using Serilog;

//register serilog configuration...

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); builder.Services.AddCors(options =>
{

    options.AddPolicy("policy1", policy =>
    {

        policy.AllowAnyHeader();

        policy.AllowAnyMethod();

        policy.AllowAnyOrigin();

    });

});

// Use Serilog as the logging provider...

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddScoped<IZoneService, ZoneService>();        
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<ITransactionService, TransactionService>(); 
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IVendorService, VendorService>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//Finds all apis in my project
builder.Services.AddEndpointsApiExplorer();
// creates Swagger docmentation
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("WarehouseDB")
    ?? throw new InvalidOperationException("Connection string 'WarehouseDB' not found in configuration files.");

builder.Services.AddDbContext<WarehouseDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Custom request logger middleware - logs timestamp, method and path to Logs/requests.log
app.UseRequestLogger();

// Configure the HTTP request pipeline.
try
{
    Log.Information("Starting web host");

    if (app.Environment.IsDevelopment())
    {
        // Enables Swagger backend(JSON)
        app.UseSwagger();
        //Opens swaggr ui in browser
        app.UseSwaggerUI();
    }

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
    }

    app.UseStaticFiles();

    app.UseRouting();

    app.UseCors("policy1");

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

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
