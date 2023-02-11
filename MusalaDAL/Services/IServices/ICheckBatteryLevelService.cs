using System.Threading.Tasks;

namespace MusalaDAL.Services.IServices
{
    public interface ICheckBatteryLevelService
    {
        Task<bool> QueryDroneBatteryLevelResult();
    }
}
