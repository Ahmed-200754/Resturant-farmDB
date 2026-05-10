using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FarmToTable.Models;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Controllers
{
    public class CropsController : Controller
    {
        private readonly ICropRepository _cropRepository;
        private readonly ILogger<CropsController> _logger;

        public CropsController(ICropRepository cropRepository, ILogger<CropsController> logger)
        {
            _cropRepository = cropRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var crops = await _cropRepository.GetAllAsync();
                return View(crops);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading crops");
                TempData["ErrorMessage"] = "Failed to load crops.";
                return View("Error");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Crop crop)
        {
            if (!ModelState.IsValid) return View(crop);
            try
            {
                await _cropRepository.CreateAsync(crop);
                TempData["SuccessMessage"] = "Crop created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating crop");
                TempData["ErrorMessage"] = "Failed to create crop.";
                return View(crop);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var crop = await _cropRepository.GetByIdAsync(id);
                if (crop == null) return NotFound();
                return View(crop);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading crop for edit");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Crop crop)
        {
            if (id != crop.CropID) return BadRequest();
            if (!ModelState.IsValid) return View(crop);

            try
            {
                await _cropRepository.UpdateAsync(crop);
                TempData["SuccessMessage"] = "Crop updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating crop");
                TempData["ErrorMessage"] = "Failed to update crop.";
                return View(crop);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _cropRepository.DeleteAsync(id);
                TempData["SuccessMessage"] = "Crop deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting crop");
                TempData["ErrorMessage"] = "Failed to delete crop. It may have associated records.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
