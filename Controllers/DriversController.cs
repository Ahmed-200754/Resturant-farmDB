using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FarmToTable.Models;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Controllers
{
    public class DriversController : Controller
    {
        private readonly IDriverRepository _driverRepository;
        private readonly ILogger<DriversController> _logger;

        public DriversController(IDriverRepository driverRepository, ILogger<DriversController> logger)
        {
            _driverRepository = driverRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var drivers = await _driverRepository.GetAllAsync();
                return View(drivers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading drivers");
                TempData["ErrorMessage"] = "Failed to load drivers.";
                return View("Error");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Driver driver)
        {
            if (!ModelState.IsValid) return View(driver);
            try
            {
                await _driverRepository.CreateAsync(driver);
                TempData["SuccessMessage"] = "Driver created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating driver");
                TempData["ErrorMessage"] = "Failed to create driver.";
                return View(driver);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var driver = await _driverRepository.GetByIdAsync(id);
                if (driver == null) return NotFound();
                return View(driver);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading driver for edit");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Driver driver)
        {
            if (id != driver.DriverID) return BadRequest();
            if (!ModelState.IsValid) return View(driver);

            try
            {
                await _driverRepository.UpdateAsync(driver);
                TempData["SuccessMessage"] = "Driver updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating driver");
                TempData["ErrorMessage"] = "Failed to update driver.";
                return View(driver);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _driverRepository.DeleteAsync(id);
                TempData["SuccessMessage"] = "Driver deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting driver");
                TempData["ErrorMessage"] = "Failed to delete driver. It may have associated records.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
