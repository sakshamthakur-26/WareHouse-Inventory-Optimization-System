using Microsoft.EntityFrameworkCore;
using WareHouse_Optimization_System.Db;
using WareHouse_Optimization_System.DTOs.Zone;
using WareHouse_Optimization_System.Models;
using ZoneEntity = WareHouse_Optimization_System.Models.Zone;
namespace WareHouse_Optimization_System.Services.Implementations;

using WareHouse_Optimization_System.Models;
using WareHouse_Optimization_System.Services.Interfaces;

public class ZoneService : IZoneService
{
    private readonly WarehouseDbContext _context;
    public ZoneService(WarehouseDbContext context)
    {
        _context = context;
    }

    //                              CREATE
    public async Task<ZoneResponse> CreateAsync(CreateZoneRequest request)
    {
        if (string.IsNullOrEmpty(request.Name))
        {
            throw new ArgumentException("Zone name cannot be null or empty.", nameof(request.Name));
        }

        var present = await _context.Zones.AnyAsync(i => i.Name != null && i.Name.ToLower() == request.Name.ToLower());
        if (present)
        {
            throw new InvalidOperationException("ZONE_ALREADY_EXISTS");
        }
        else
        {
            var zone = new Zone
            {
                Name = request.Name,
                MaxCapacity = request.MaxCapacity
            };

            _context.Zones.Add(zone);
            await _context.SaveChangesAsync();

            return new ZoneResponse
            {
                ZoneId = zone.ZoneId,
                Name = request.Name,
                MaxCapacity = request.MaxCapacity

            };
        }
    }
    //                       GET ALL
    public async Task<IEnumerable<ZoneResponse>> GetAllAsync()
    {
        return await _context.Zones.Select(z => new ZoneResponse
        {
            ZoneId = z.ZoneId,
            Name = z.Name,
            MaxCapacity = z.MaxCapacity

        }).ToListAsync();

        //throw new NotImplementedException();
    }

    //                  DELETE
    public async Task DeleteAsync(int id)
    {
        var present = await _context.Zones.FindAsync(id);
        if (present == null) throw new KeyNotFoundException();

        _context.Zones.Remove(present);
        await _context.SaveChangesAsync();
    }


    //                      Get Zone by Its ID
    public async Task<ZoneResponse> GetByIdAsync(int id)
    {
        var found = await _context.Zones.FindAsync(id);
        if (found == null)
        {
            throw new InvalidOperationException("Zone Data Not Found");
        }
        else
        {

            return MapToResponse(found);
        }
    }


    //                           UPDSTE Zone Id find

    public async Task UpdateAsync(int id, CreateZoneRequest request)
    {
        var found = await _context.Zones.FindAsync(id);
        if (found == null)
        {
            throw new InvalidOperationException("This data Is Not Present");
        }
        else
        {
            found.Name = request.Name;
            found.MaxCapacity = request.MaxCapacity;
        }

        await _context.SaveChangesAsync();
    }

    //-------------------------------CAPACITY BASED--------------------------------------//


    public async Task<bool> CheckAvailableCapacityAsync(int zoneId, int requiredSpace)
    {
        var found = await _context.Zones.FindAsync(zoneId);
        if (found == null)
        {
            throw new KeyNotFoundException("Zone Not Found");
        }

        return (found.MaxCapacity - found.CurrentUsage) >= requiredSpace;
        //throw new NotImplementedException();
    }

    public async Task UpdateZoneUsageAsync(int zoneId, int spaceUsed)
    {
        var found = await _context.Zones.FindAsync(zoneId);
        if (found == null)
        {
            throw new KeyNotFoundException("Zone Not Found");
        }
        if (found.CurrentUsage + spaceUsed > found.MaxCapacity)
        {
            throw new InvalidOperationException("Exceeding Zone Capacity");
        }
        else
        {
            found.CurrentUsage += spaceUsed;
            await _context.SaveChangesAsync();
        }
        //throw new NotImplementedException();
    }

    private static ZoneResponse MapToResponse(ZoneEntity z)
    {
        return new ZoneResponse
        {
            ZoneId = z.ZoneId,
            Name = z.Name,
            MaxCapacity = z.MaxCapacity,
            CurrentUsage = z.CurrentUsage
        };
    }

}

