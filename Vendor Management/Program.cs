using Microsoft.EntityFrameworkCore;
using VendorManagement.DTOs;
using VendorManagement.Models;
using VendorManagement.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Database Connection Registration
builder.Services.AddDbContext<VendorDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VendorDb")));

// Register Repository & Service
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<VendorService>();

var app = builder.Build();


app.UseAuthorization();
app.MapControllers();
app.Run();
