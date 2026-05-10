using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FarmToTable.Models;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Controllers
{
    public class TaskRequirementsController : Controller
    {
        private readonly ITaskRequirementsRepository _repo;

        public TaskRequirementsController(ITaskRequirementsRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            // Retrieve all explicit query requirements
            ViewBag.Farms = await _repo.GetAllFarmsAsync();
            ViewBag.Restaurants = await _repo.GetAllRestaurantsAsync();
            ViewBag.HarvestBatches = await _repo.GetAllHarvestBatchesAsync();

            ViewBag.Join1 = await _repo.GetRestaurantOrdersWithBatchDetailsAsync();
            ViewBag.Join2 = await _repo.GetCropNamesWithHarvestBatchesAsync();
            ViewBag.Join3 = await _repo.GetFarmBatchesWithCropInfoAsync();
            ViewBag.Join4 = await _repo.GetOrdersWithRestaurantNamesAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RunInserts()
        {
            try
            {
                await _repo.InsertNewFarmAsync(new Farm { FarmName = "AgriIntel Testing Farm", FarmerName = "QA Tester", FarmerPhone = "0123456789", Longitude = 30.0m, Latitude = 30.0m });
                await _repo.InsertNewRestaurantAsync(new Restaurant { RestaurantName = "AgriIntel QA Bistro", Address = "Tech Park", DeliveryWindow = new TimeSpan(12, 0, 0) });
                await _repo.InsertNewHarvestBatchAsync(new HarvestBatch { HarvestDate = DateTime.Today, Quantity = 999, FreshnessWindow = DateTime.Today.AddDays(5), CropID = 1, FarmID = 1 });
                TempData["SuccessMessage"] = "Successfully executed 3 INSERT statements manually using ADO.NET!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error executing INSERTS: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RunDeletes()
        {
            try
            {
                // Attempt to delete very high IDs that don't exist to show valid syntax without breaking real data
                await _repo.DeleteHarvestBatchAsync(999999);
                await _repo.DeleteRestaurantAsync(999999);
                await _repo.DeleteDriverAsync(999999);
                TempData["SuccessMessage"] = "Successfully executed 3 DELETE statements (WITH proper WHERE conditions)!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error executing DELETES: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RunUpdates()
        {
            try
            {
                // Updates actual ID 1 just for demonstration
                await _repo.UpdateFarmPhoneNumberAsync(1, "01099999999");
                await _repo.UpdateRestaurantDeliveryWindowAsync(1, new TimeSpan(14, 30, 0));
                await _repo.UpdateHarvestBatchQuantityAsync(1, 205.5);
                TempData["SuccessMessage"] = "Successfully executed 3 UPDATE statements (WITH proper WHERE conditions)!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error executing UPDATES: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
