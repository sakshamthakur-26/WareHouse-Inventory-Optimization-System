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

    //                              CREATE USING PROCEDURE
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

    //                       GET ALL
    public async Task<IEnumerable<ZoneResponse>> GetAllAsync()
    {
        return await _context.Zones.Select(z => new ZoneResponse
        {
            ZoneId = z.ZoneId,
            Name = z.Name,
            MaxCapacity = z.MaxCapacity,
            CurrentUsage = z.CurrentUsage
        }).ToListAsync();

        //throw new NotImplementedException();
    }

    //                  DELETE
    private async Task DeleteAsync(int id)
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
    public async Task<ServiceResult<object>> UpdateAsync(int id , UpdateZoneRequest request)
    {
        var found = await _context.Zones.FindAsync(id);
        if (found == null) return ServiceResult<object>.Failure("This data Is Not Present");

        // Only update fields that are provided in the request
        if (!string.IsNullOrEmpty(request.Name))
        {
            found.Name = request.Name;
        }

        if (request.MaxCapacity.HasValue)
        {
            // Ensure we don't set MaxCapacity below current usage
            if (request.MaxCapacity.Value < found.CurrentUsage)
            {
                return ServiceResult<object>.Failure("New max capacity cannot be less than current usage");
            }

            found.MaxCapacity = request.MaxCapacity.Value;
        }

        await _context.SaveChangesAsync();

        return ServiceResult<object>.Success(null);
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
        //throw new NotImplmentedexception()  ;
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
        //throw new NotImpleementedException();
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

