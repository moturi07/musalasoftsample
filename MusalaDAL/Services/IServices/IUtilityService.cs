using Microsoft.AspNetCore.Http;
using MusalaDAL.Models;
using MusalaDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusalaDAL.Services.IServices
{
    public interface IUtilityService
    {
        Task<IEnumerable<Drone>> GetDronesAsync(string userId);
        Task<IEnumerable<Drone>> GetDroneAsync(DroneSearchViewModel model);
        Task<string> SaveDroneAsync(Drone model, string userId);
        Task<Drone> UpdateDroneAsync(Drone model, string userId);
        Task<bool> DeleteDroneAsync(int id, string userId);
        Task<IEnumerable<Drone>> GetAvailableDronesAsync();
        Task<int> CheckBatteryLevelAsync(int Id);


        Task<IEnumerable<Medication>> GetMedicationsAsync(string userId);
        Task<IEnumerable<Medication>> GetMedicationAsync(MedicationSearchModel model);
        Task<string> SaveMedicationAsync(MedicationViewModel model,string userId);
        Task<int> SaveMedicationImageAsync(MedicationViewModel model, string userId);
        Task<Medication> UpdateMedicationAsync(MedicationViewModel model, string userId);
        Task<bool> DeleteMedicationAsync(int id, string userId);

        Task<IEnumerable<Delivery>> GetDeliveriesAsync(DeliverySearchModel model, string userId);
        Task<int> SaveDeliveriesAsync(Delivery model, string userId);
    }
}
