using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
// ADD for Swagger JWT configuration
using Microsoft.OpenApi.Models;
using Serilog;
using WareHouse_Optimization_System.Db;
using WareHouse_Optimization_System.Middlewares;
using WareHouse_Optimization_System.Services.Implementations;
using WareHouse_Optimization_System.Services.Interfaces;

//register serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


// Use Serilog as the logging provider
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddScoped<IZoneService, ZoneService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IVendorService, VendorService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
//  ADD JwtService registration
builder.Services.AddScoped<JwtService>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//Finds all apis in my project
builder.Services.AddEndpointsApiExplorer();

//  REPLACE SwaggerGen with JWT-enabled config
builder.Services.AddSwaggerGen(options =>
{
    // define Bearer token scheme for Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization", // header name
        Type = SecuritySchemeType.Http, // using HTTP auth
        Scheme = "bearer", // bearer token type
        BearerFormat = "JWT", // token format
        In = ParameterLocation.Header, // token sent in header
        Description = "Enter Bearer token like: Bearer your_token_here"
    });

    // require token globally for secured endpoints
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var connectionString = builder.Configuration.GetConnectionString("WarehouseDB")
    ?? throw new InvalidOperationException("Connection string 'WarehouseDB' not found in configuration files.");
builder.Services.AddDbContext<WarehouseDbContext>(options =>
    options.UseSqlServer(connectionString));

// ✅ ADD JWT Authentication Configuration
var jwtKey = builder.Configuration["Jwt:Key"];
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey))
    };
});

var allowedOrigins = builder.Configuration.GetSection("allowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

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
        //Opens swagger ui in browser
        app.UseSwaggerUI();
    }

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
    }

    app.UseStaticFiles();

    app.UseRouting();

    app.UseCors();

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