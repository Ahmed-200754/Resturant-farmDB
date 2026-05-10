using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FarmToTable.Models;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Controllers
{
    public class FarmsController : Controller
    {
        private readonly IFarmRepository _farmRepository;
        private readonly ILogger<FarmsController> _logger;

        public FarmsController(IFarmRepository farmRepository, ILogger<FarmsController> logger)
        {
            _farmRepository = farmRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var farms = await _farmRepository.GetAllAsync();
                return View(farms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading farms");
                TempData["ErrorMessage"] = "Failed to load farms. Please try again.";
                return View("Error");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Farm farm)
        {
            if (!ModelState.IsValid) return View(farm);
            try
            {
                await _farmRepository.CreateAsync(farm);
                TempData["SuccessMessage"] = "Farm created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating farm");
                TempData["ErrorMessage"] = "Failed to create farm.";
                return View(farm);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var farm = await _farmRepository.GetByIdAsync(id);
                if (farm == null) return NotFound();
                return View(farm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading farm for edit");
                TempData["ErrorMessage"] = "Failed to load farm.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Farm farm)
        {
            if (id != farm.FarmID) return BadRequest();
            if (!ModelState.IsValid) return View(farm);

            try
            {
                await _farmRepository.UpdateAsync(farm);
                TempData["SuccessMessage"] = "Farm updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating farm");
                TempData["ErrorMessage"] = "Failed to update farm.";
                return View(farm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _farmRepository.DeleteAsync(id);
                TempData["SuccessMessage"] = "Farm deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting farm");
                TempData["ErrorMessage"] = "Failed to delete farm. It may have associated records.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
