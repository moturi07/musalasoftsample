using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MusalaDAL.Context;
using MusalaDAL.Models;
using MusalaDAL.Services.IServices;
using MusalaDAL.ViewModels;
using Microsoft.AspNetCore.Http;

namespace MusalaDAL.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UtilityService> _logger;

        public UtilityService(ApplicationDbContext context, IConfiguration configuration, ILogger<UtilityService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Drone>> GetDronesAsync(string userId)
        {
            _logger.LogInformation($"Get Drones");
            try
            {
                var result = await _context.Drones.AsNoTracking().AsQueryable().OrderByDescending(o=>o.ID).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw ex;
            }
        }


        public async Task<IEnumerable<Drone>> GetDroneAsync(DroneSearchViewModel model)
        {
            _logger.LogInformation($"Get drone: {model}");
            try
            {
                var _query = _context.Drones.Where(q => q.Active == true);
                if (model.ID > 0)
                {
                    _query = _query.Where(q => q.ID == model.ID).AsQueryable();
                }
                if (!string.IsNullOrEmpty(model.serialNumber))
                {
                    _query = _query.Where(q => q.serialNumber.Equals(model.serialNumber)).AsQueryable();
                }
                var _count = await _query.CountAsync();
                var _drones = await _query.OrderByDescending(o => o.CreatedOn).AsQueryable().AsNoTracking().ToListAsync();
                return _drones;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw ex;
            }
        }

        public async Task<string> SaveDroneAsync(Drone model, string userId)
        {
            try
            {
                var _query = await _context.Drones.Where(q => q.Active == true && q.serialNumber == model.serialNumber).FirstOrDefaultAsync();
                if (_query == null)
                {
                    var _drone = (Drone)model;
                    _context.Drones.Add(_drone);
                    await _context.SaveChangesAsync();
                    return "Success: "+_drone.ID;
                }
                return "Serial Already Exists";
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw ex;
            }
        }

        public async Task<Drone> UpdateDroneAsync(Drone drone, string userId)
        {
            try
            {
                var _drone = await _context.Drones.Where(q => q.ID == drone.ID).FirstOrDefaultAsync();
                if (_drone != null)
                {
                    _drone.Active = drone.Active;
                    _drone.serialNumber = drone.serialNumber;
                    _drone.model = drone.model;
                    _drone.weightLimit = drone.weightLimit;
                    _drone.batteryCapacity = drone.batteryCapacity;
                    _drone.state = drone.state;
                    await _context.SaveChangesAsync();
                }
                return _drone;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw ex;
            }
        }

        public async Task<bool> DeleteDroneAsync(int id, string userId)
        {
            try
            {
                var _drones = await _context.Drones.Where(q => q.ID==id).FirstOrDefaultAsync();

                if (_drones != null)
                {
                    _context.Drones.Remove(_drones);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw ex;
            }
        }
        public async Task<IEnumerable<Drone>> GetAvailableDronesAsync()
        {
            try
            {
                var _drones = await _context.Drones.Where(q => q.Active == true && q.state == Drone.stateType.IDLE).AsNoTracking().AsQueryable().OrderByDescending(o => o.ID).ToListAsync();
                return _drones;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw ex;
            }
        }

        public async Task<int> CheckBatteryLevelAsync(int Id)
        {
            try
            {
                var _query = await _context.Drones.Where(q => q.Active == true).Select(u=>u.batteryCapacity).FirstOrDefaultAsync();
                return _query;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw ex;
            }
        }



        public async Task<IEnumerable<Medication>> GetMedicationsAsync(string userId)
        {
            _logger.LogInformation($"Get Medications");
            try
            {
                var result = await _context.Medication.AsNoTracking().AsQueryable().OrderByDescending(o => o.ID).ToListAsync();
                foreach( var res in result )
                {
                    var images1 = Convert.ToBase64String(res.image);
                    res.image = (byte[])Convert.FromBase64String(images1);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw ex;
            }
        }


        public async Task<IEnumerable<Medication>> GetMedicationAsync(MedicationSearchModel model)
        {
            _logger.LogInformation($"Get Medication: {model}");
            try
            {
                var _query = _context.Medication.Where(q => q.Active == true);
                if (model.ID > 0)
                {
                    _query = _query.Where(q => q.ID == model.ID).AsQueryable();
                }
                if (!string.IsNullOrEmpty(model.name) )
                {
                    _query = _query.Where(q => q.name.Equals(model.name)).AsQueryable();
                }
                if (!string.IsNullOrEmpty(model.name))
                {
                    _query = _query.Where(q => q.code.Equals(model.code)).AsQueryable();
                }
                var _count = await _query.CountAsync();
                var _medics = await _query.OrderByDescending(o => o.CreatedOn).AsQueryable().AsNoTracking().ToListAsync();
                return _medics;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw ex;
            }
        }

        public async Task<string> SaveMedicationAsync(MedicationViewModel model, string userId)
        {
            try
            {
                var _query = await _context.Medication.Where(q => q.Active == true && q.code==model.code).FirstOrDefaultAsync();
                if (_query == null)
                {
                    Medication medic = new Medication();
                    medic.Active = true;
                    medic.code = model.code;
                    medic.name = model.name;
                    medic.weight = model.weight;
                    using (var ms = new MemoryStream())
                    {
                        model.image.CopyTo(ms);
                        medic.image = ms.ToArray();
                    }
                    await _context.Medication.AddAsync(medic);
                    await _context.SaveChangesAsync();
                    return "Sucess: "+medic.ID.ToString();
                }
                else
                {
                    return "Code exists";
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw ex;
            }
        }

        public async Task<Medication> UpdateMedicationAsync(MedicationViewModel model, string userId)
        {
            try
            {
                var _medic = await _context.Medication.Where(q => q.ID==model.ID).FirstOrDefaultAsync();
                if (_medic != null)
                {
                    _medic.Active = model.Active;
                    _medic.code = model.code;
                    _medic.name = model.name;
                    _medic.weight = model.weight;
                    using (var ms = new MemoryStream())
                    {
                        model.image.CopyTo(ms);
                        _medic.image = ms.ToArray();
                    }
                    await _context.SaveChangesAsync();
                }
                return _medic;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw ex;
            }
        }
        public async Task<int> SaveMedicationImageAsync(MedicationViewModel model, string userId)
        {
            try
            {
                var _drone = await _context.Medication.FindAsync(model.code);
                Medication medic = new Medication();
                if (_drone != null && medic != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        model.image.CopyTo(ms);
                        medic.image = ms.ToArray();
                    }
                    await _context.SaveChangesAsync();
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw ex;
            }
        }



        public async Task<bool> DeleteMedicationAsync(int id, string userId)
        {
            try
            {
                var _drones = await _context.Drones.Where(q => q.ID == id).FirstOrDefaultAsync();

                if (_drones != null)
                {
                    _context.Drones.Remove(_drones);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw ex;
            }
        }



        public async Task<IEnumerable<Delivery>> GetDeliveriesAsync(DeliverySearchModel model, string userId)
        {
            _logger.LogInformation($"Get Deliveries: {model}");
            try
            {
                var _query = _context.Deliveries.Where(q => q.Active == true);
                if (model.ID > 0)
                {
                    _query = _query.Where(q => q.ID == model.ID).AsQueryable();
                }
                if (model.BatchID > 0)
                {
                    _query = _query.Where(q => q.BatchID == model.BatchID).AsQueryable();
                }
                if (model.DroneID > 0)
                {
                    _query = _query.Where(q => q.DroneID == model.DroneID).AsQueryable();
                }
                if (model.MedicationID > 0)
                {
                    _query = _query.Where(q => q.MedicationID == model.MedicationID).AsQueryable();
                }
                if (!string.IsNullOrEmpty(model.Origin))
                {
                    _query = _query.Where(q => q.Origin.Equals(model.Origin)).AsQueryable();
                }
                if (!string.IsNullOrEmpty(model.Destination))
                {
                    _query = _query.Where(q => q.Destination.Equals(model.Destination)).AsQueryable();
                }
                var _count = await _query.CountAsync();
                var _deliveries = await _query.OrderByDescending(o => o.CreatedOn).AsQueryable().AsNoTracking().ToListAsync();
                return _deliveries;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw ex;
            }
        }

        public async Task<int> SaveDeliveriesAsync(Delivery model, string userId)
        {
            try
            {
                var _drones = await _context.Drones.Where(q => q.Active==true && q.ID == model.DroneID).FirstOrDefaultAsync();
                var _medication = await _context.Medication.Where(q => q.Active == true && q.ID == model.MedicationID && q.weight<=500).FirstOrDefaultAsync();
                
                if (_drones == null)
                {                    
                    throw new HttpRequestException("Drone Not Found. Please try again.");
                    return 0;
                }
                if (_drones != null && _drones.batteryCapacity < 25 && _drones.state == Drone.stateType.LOADING)
                {
                    throw new HttpRequestException("Drone Battery Capacity is below 25%. Please charge and try again.");
                    return 0;
                }
                if (_drones != null && _drones.state != Drone.stateType.IDLE)
                {
                    throw new HttpRequestException("Drone is Busy. Please select another one and try again.");
                    return 0;
                }
                if (_medication == null)
                {
                    throw new HttpRequestException("Medication Not Found or above weight limit. Please try again.");
                    return 0;
                }
                var deliver = (Delivery)model;
                _context.Deliveries.Add(deliver);
                await _context.SaveChangesAsync();
                return deliver.ID;
                
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw ex;
            }
        }


    }
}
