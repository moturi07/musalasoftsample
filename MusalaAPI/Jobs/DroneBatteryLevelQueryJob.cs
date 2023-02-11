using Microsoft.Extensions.Logging;
using MusalaDAL.Services.IServices;
using Quartz;
using System;
using System.Threading.Tasks;

namespace MusalaDAL.Service.Jobs
{
    [DisallowConcurrentExecution]
    public class DroneBatteryLevelQueryJob : IJob
    {
        private readonly ILogger<DroneBatteryLevelQueryJob> _logger;
        private readonly ICheckBatteryLevelService _checkbatterylevel;

        public DroneBatteryLevelQueryJob(ILogger<DroneBatteryLevelQueryJob> logger, ICheckBatteryLevelService checkbatterylevel)
        {
            _logger = logger;
            _checkbatterylevel = checkbatterylevel;
        }

        public Task Execute(IJobExecutionContext context)
        {
            //_logger.LogInformation("Running Query for delayed Drone Battery Level");
            try
            {
                Task.Run(() => _checkbatterylevel.QueryDroneBatteryLevelResult()).Wait();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ooops an error occured =>  {ex.Message}");
            }
            return Task.CompletedTask;
        }
    }
}
