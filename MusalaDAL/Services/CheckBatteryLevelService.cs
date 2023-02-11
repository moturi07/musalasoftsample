using System.Linq;
using Microsoft.Extensions.Logging;
using MusalaDAL.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using MusalaDAL.Services.IServices;

namespace MusalaDAL.Services
{
    public class CheckBatteryLevelService : ICheckBatteryLevelService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CheckBatteryLevelService> _logger;
        public CheckBatteryLevelService(ApplicationDbContext context, ILogger<CheckBatteryLevelService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<bool> QueryDroneBatteryLevelResult()
        {
            try
            {
                var _query = await _context.Drones.Where(x => x.Active==true)
                    //.Select(q => q.batteryCapacity)
                    .ToListAsync();
                if (_query != null && _query.Any())
                {
                    foreach (var drone in _query)
                    {
                        //_logger.LogInformation("Drone Details {@model}", drone);
                        if (drone != null)
                        {
                            if (drone.batteryCapacity > 25)
                            {
                                _logger.LogInformation("Drone " + drone.serialNumber + " Is Okay with Capacity " + drone.batteryCapacity);
                            }
                            else
                            {
                                _logger.LogInformation("Drone " + drone.serialNumber + " Is with Low battery Capacity " + drone.batteryCapacity);

                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return false;
            }
        }
    }
}
