namespace VendorManagement.Models;

using Microsoft.EntityFrameworkCore;
using VendorManagement.Models;


public class VendorDbContext : DbContext
{
    public VendorDbContext(DbContextOptions<VendorDbContext> options) : base(options) { }

    public DbSet<Vendor> Vendors { get; set; }
}

