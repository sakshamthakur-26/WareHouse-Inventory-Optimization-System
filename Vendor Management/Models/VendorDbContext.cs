namespace VendorManagement.Models;

using Microsoft.EntityFrameworkCore;
using VendorManagement.Models;


public class VendorDbContext : DbContext
{
    public VendorDbContext(DbContextOptions<VendorDbContext> options) : base(options) { }

    public DbSet<Vendor> Vendors { get; set; }
}

public interface IVendorRepository
{
    Task<IEnumerable<Vendor>> GetAllAsync();
    Task<Vendor> GetByIdAsync(int id);
    Task AddAsync(Vendor vendor);
    Task UpdateAsync(Vendor vendor);
    Task DeleteAsync(int id);
}

public class VendorRepository : IVendorRepository
{
    private readonly VendorDbContext _context;
    public VendorRepository(VendorDbContext context) { _context = context; }

    public async Task<IEnumerable<Vendor>> GetAllAsync() => await _context.Vendors.ToListAsync();

    public async Task<Vendor> GetByIdAsync(int id) => await _context.Vendors.FindAsync(id);

    public async Task AddAsync(Vendor vendor)
    {
        await _context.Vendors.AddAsync(vendor);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Vendor vendor)
    {
        _context.Entry(vendor).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var v = await _context.Vendors.FindAsync(id);
        if (v != null)
        {
            _context.Vendors.Remove(v);
            await _context.SaveChangesAsync();
        }
    }
}