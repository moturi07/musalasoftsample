using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusalaDAL.Helpers;
using MusalaDAL.Models;
using MusalaDAL.Services.IServices;
using MusalaDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusalaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class DeliveryController : Controller
    {
        private readonly IUtilityService _utilityService;
        public DeliveryController(IUtilityService utilityService)
        {
            _utilityService = utilityService;
        }

        [HttpPost, Route("RegisterDrone")]
        public async Task<ActionResult<int>> SaveDroneAsync([FromBody] Drone model)
        {
            try
            {
                string userId = HttpContext.GetUserId();
                return Ok(await _utilityService.SaveDroneAsync(model, userId));
            }
            catch (Exception ex)
            {
                return BadRequest("Error registering drone."+ ex.Message);
            }
        }
        [HttpPost, Route("SaveDeliveries")]
        public async Task<ActionResult<string>> SaveDeliveriesAsync([FromBody] Delivery model)
        {
            try
            {
                string userId = HttpContext.GetUserId();
                return Ok(await _utilityService.SaveDeliveriesAsync(model, userId));
            }
            catch (Exception ex)
            {
                return BadRequest("Error saving delivery." + ex.Message);
            }
        }



        [HttpGet, Route("GetAvailableDrones")]
        public async Task<ActionResult<IEnumerable<Drone>>> GetAvailableDronesAsync()
        {
            try
            {
                string userId = HttpContext.GetUserId();
                return Ok(await _utilityService.GetAvailableDronesAsync());
            }
            catch (Exception ex)
            {
                return BadRequest("Error getting available drones." + ex.Message);
            }
        }
        [HttpGet, Route("CheckBatteryLevel")]
        public async Task<ActionResult<IEnumerable<Medication>>> CheckBatteryLevelAsync([FromRoute] int Id)
        {
            try
            {
                string userId = HttpContext.GetUserId();
                return Ok(await _utilityService.CheckBatteryLevelAsync(Id));
            }
            catch (Exception ex)
            {
                return BadRequest("Error checking battery level." + ex.Message);
            }
        }






        [HttpGet, Route("GetDrones")]
        public async Task<ActionResult<IEnumerable<Drone>>> GetDronesAsync()
        {
            try
            {
                string userId = HttpContext.GetUserId();
                return Ok(await _utilityService.GetDronesAsync(userId));
            }
            catch (Exception ex)
            {
                return BadRequest("Error retreiving drones." + ex.Message);
            }
        }

        [HttpGet, Route("GetDrone")]
        public async Task<ActionResult<IEnumerable<Drone>>> GetDroneAsync([FromBody] DroneSearchViewModel model)
        {
            try
            {
                string userId = HttpContext.GetUserId();
                return Ok(await _utilityService.GetDroneAsync(model));
            }
            catch (Exception ex)
            {
                return BadRequest("Error retreiving drone." + ex.Message);
            }
        }
        [HttpPost, Route("UpdateDrone")]
        public async Task<ActionResult<Drone>> UpdateDroneAsync([FromBody] Drone model)
        {
            try
            {
                string userId = HttpContext.GetUserId();
                return Ok(await _utilityService.UpdateDroneAsync(model, userId));
            }
            catch (Exception ex)
            {
                return BadRequest("Error Updating drone." + ex.Message);
            }
        }




        [HttpGet, Route("GetMedications")]
        public async Task<ActionResult<IEnumerable<Medication>>> GetMedicationsAsync()
        {
            try
            {
                string userId = HttpContext.GetUserId();
                return Ok(await _utilityService.GetMedicationsAsync(userId));
            }
            catch (Exception ex)
            {
                return BadRequest("Error retreiving medications." + ex.Message);
            }
        }

        [HttpPost, Route("GetMedication")]
        public async Task<ActionResult<IEnumerable<Medication>>> GetMedicationAsync([FromBody] MedicationSearchModel model)
        {
            try
            {
                string userId = HttpContext.GetUserId();
                return Ok(await _utilityService.GetMedicationAsync(model));
            }
            catch (Exception ex)
            {
                return BadRequest("Error retreiving medication." + ex.Message);
            }
        }

        [HttpPost, Route("SaveMedication")]
        public async Task<ActionResult<string>> SaveMedicationAsync([FromForm] MedicationViewModel model)
        {
            try
            {
                string userId = HttpContext.GetUserId();
                return Ok(await _utilityService.SaveMedicationAsync(model, userId));             
            }
            catch (Exception ex)
            {
                return BadRequest("Error saving medication." + ex.Message);
            }
        }

        //[HttpPost]
        //[Route("SaveMedicationImage")]
        //public async Task<object> SaveMedicationImageAsync([FromForm] MedicationViewModel model)
        //{
        //    var userId = this.HttpContext.GetUserId();
        //    return Ok(await _utilityService.SaveMedicationImageAsync(model, userId));
        //}

        [HttpPost, Route("UpdateMedication")]
        public async Task<ActionResult<Medication>> UpdateMedicationAsync([FromForm] MedicationViewModel model)
        {
            try
            {
                string userId = HttpContext.GetUserId();
                return Ok(await _utilityService.UpdateMedicationAsync(model, userId));
            }
            catch (Exception ex)
            {
                return BadRequest("Error updating medication." + ex.Message);
            }
        }










        [HttpPost, Route("GetDeliveries")]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetDeliveriesAsync([FromBody] DeliverySearchModel model)
        {
            try
            {
                string userId = HttpContext.GetUserId();
                return Ok(await _utilityService.GetDeliveriesAsync(model, userId));
            }
            catch (Exception ex)
            {
                return BadRequest("Error getting deliveries." + ex.Message);
            }
        }


    }
}
