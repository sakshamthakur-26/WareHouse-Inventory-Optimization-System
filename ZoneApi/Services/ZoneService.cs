using Microsoft.EntityFrameworkCore;
using Zone.DTOs;
using ZoneApi.Model;
using ZoneEntity = ZoneApi.Model.Zone;
using Zone.Services;

namespace Zone.Services
{

    public class ZoneService : IZoneService
    {
        private readonly ZoneContext _context;
        public ZoneService(ZoneContext context)
        {
            _context = context;
        }

        //                              CREATE
        public async Task<ServiceResult<ZoneResponse>> CreateAsync(CreateZoneRequest request)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                return ServiceResult<ZoneResponse>.Failure("Zone name cannot be null or empty.");
            }

            var present = await _context.Zones.AnyAsync(i => i.Name != null && i.Name.ToLower() == request.Name.ToLower());
            if (present)
            {
                return ServiceResult<ZoneResponse>.Failure("ZONE_ALREADY_EXISTS");
            }
            else
            {
                var zone = new ZoneApi.Model.Zone
                {
                    Name = request.Name,
                    MaxCapacity = request.MaxCapacity
                };

                _context.Zones.Add(zone);
                await _context.SaveChangesAsync();

                return ServiceResult<ZoneResponse>.Success(new ZoneResponse
                {
                    ZoneId = zone.ZoneId,
                    Name = request.Name,
                    MaxCapacity = request.MaxCapacity

                });
            }
        }
        //                       GET ALL
        public async Task<ServiceResult<IEnumerable<ZoneResponse>>> GetAllAsync()
        {
            var list = await _context.Zones.Select(z => new ZoneResponse
            {
                ZoneId = z.ZoneId,
                Name = z.Name,
                MaxCapacity = z.MaxCapacity

            }).ToListAsync();

            return ServiceResult<IEnumerable<ZoneResponse>>.Success(list);

            //throw new NotImplementedException();
        }

        //                  DELETE  : Now private not used ...

        private async Task<ServiceResult<object>> DeleteAsync(int id)
        {
            var present = await _context.Zones.FindAsync(id);
            if (present == null) return ServiceResult<object>.Failure("Zone not found");

            _context.Zones.Remove(present);
            await _context.SaveChangesAsync();

            return ServiceResult<object>.Success(null);
        }


        //                      Get Zone by Its ID : 
        public async Task<ServiceResult<ZoneResponse>> GetByIdAsync(int id)
        {
            var found = await _context.Zones.FindAsync(id);
            if (found == null) return ServiceResult<ZoneResponse>.Failure("Zone Data Not Found");

            return ServiceResult<ZoneResponse>.Success(MapToResponse(found));
        }


        //                           UPDSTE Zone Id find

        public async Task<ServiceResult<object>> UpdateAsync(int id, CreateZoneRequest request)
        {
            var found = await _context.Zones.FindAsync(id);
            if (found == null) return ServiceResult<object>.Failure("This data Is Not Present");

            found.Name = request.Name;
            found.MaxCapacity = request.MaxCapacity;

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

}
