using Microsoft.EntityFrameworkCore;
using WareHouse_Optimization_System.Db;
using WareHouse_Optimization_System.DTOs.Zone;
using WareHouse_Optimization_System.Models;
using ZoneEntity = WareHouse_Optimization_System.Models.Zone;

namespace WareHouse_Optimization_System.Services.Implementations;

using Microsoft.Data.SqlClient;
using WareHouse_Optimization_System.Models;
using WareHouse_Optimization_System.Services.Interfaces;

public class ZoneService : IZoneService
{
    private readonly WarehouseDbContext _context;

    public ZoneService(WarehouseDbContext context)
    {
        _context = context;
    }

    public async Task<ZoneResponse> CreateAsync(CreateZoneRequest request)
    {
        try
        {
            var zones = await _context.Zones
                .FromSqlRaw("EXEC sp_CreateZone @Name, @MaxCapacity",
                    new SqlParameter("@Name", request.Name),
                    new SqlParameter("@MaxCapacity", request.MaxCapacity))
                .AsNoTracking()
                .ToListAsync();

            var result = zones.Select(z => new ZoneResponse
            {
                ZoneId = z.ZoneId,
                Name = z.Name,
                MaxCapacity = z.MaxCapacity,
                CurrentUsage = z.CurrentUsage
            }).FirstOrDefault();

            return result;
        }
        catch (SqlException ex)
        {
            if (ex.Number == 50001)
                throw new ArgumentException(ex.Message);

            if (ex.Number == 50002)
                throw new InvalidOperationException("Zone already exists");

            throw;
        }
    }

    public async Task<IEnumerable<ZoneResponse>> GetAllAsync()
    {
        return await _context.Zones.Select(z => new ZoneResponse
        {
            ZoneId = z.ZoneId,
            Name = z.Name,
            MaxCapacity = z.MaxCapacity,
            CurrentUsage = z.CurrentUsage
        }).ToListAsync();
    }

    private async Task DeleteAsync(int id)
    {
        var present = await _context.Zones.FindAsync(id);
        if (present == null) throw new KeyNotFoundException();

        _context.Zones.Remove(present);
        await _context.SaveChangesAsync();
    }

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

    public async Task<ServiceResult<object>> UpdateAsync(int id, UpdateZoneRequest request)
    {
        var found = await _context.Zones.FindAsync(id);
        if (found == null) return ServiceResult<object>.Failure("This data Is Not Present");

        if (!string.IsNullOrEmpty(request.Name))
        {
            found.Name = request.Name;
        }

        if (request.MaxCapacity.HasValue)
        {
            if (request.MaxCapacity.Value < found.CurrentUsage)
            {
                return ServiceResult<object>.Failure("New max capacity cannot be less than current usage");
            }

            found.MaxCapacity = request.MaxCapacity.Value;
        }

        await _context.SaveChangesAsync();

        return ServiceResult<object>.Success(null);
    }

    public async Task<bool> CheckAvailableCapacityAsync(int zoneId, int requiredSpace)
    {
        var found = await _context.Zones.FindAsync(zoneId);
        if (found == null)
        {
            throw new KeyNotFoundException("Zone Not Found");
        }

        return (found.MaxCapacity - found.CurrentUsage) >= requiredSpace;
    }

    public async Task<ServiceResult<bool>> UpdateZoneUsageAsync(int zoneId, int spaceUsed)
    {
        var found = await _context.Zones.FindAsync(zoneId);

        if (found == null)
        {
            return ServiceResult<bool>.Failure("Zone Not Found");
        }

        if (found.CurrentUsage + spaceUsed > found.MaxCapacity)
        {
            return ServiceResult<bool>.Failure("Exceeding Zone Capacity");
        }

        found.CurrentUsage += spaceUsed;
        await _context.SaveChangesAsync();

        return ServiceResult<bool>.Success(true);
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
