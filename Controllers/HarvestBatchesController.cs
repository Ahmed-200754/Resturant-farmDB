using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using FarmToTable.Models;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Controllers
{
    public class HarvestBatchesController : Controller
    {
        private readonly IHarvestBatchRepository _batchRepository;
        private readonly ICropRepository _cropRepository;
        private readonly IFarmRepository _farmRepository;
        private readonly ILogger<HarvestBatchesController> _logger;

        public HarvestBatchesController(
            IHarvestBatchRepository batchRepository,
            ICropRepository cropRepository,
            IFarmRepository farmRepository,
            ILogger<HarvestBatchesController> logger)
        {
            _batchRepository = batchRepository;
            _cropRepository = cropRepository;
            _farmRepository = farmRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var batches = await _batchRepository.GetAllAsync();
                return View(batches);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading harvest batches");
                TempData["ErrorMessage"] = "Failed to load harvest batches.";
                return View("Error");
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var crops = await _cropRepository.GetAllAsync();
                var farms = await _farmRepository.GetAllAsync();
                ViewBag.Crops = new SelectList(crops, "CropID", "CropName");
                ViewBag.Farms = new SelectList(farms, "FarmID", "FarmName");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading form data");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HarvestBatch batch)
        {
            if (!ModelState.IsValid)
            {
                var crops = await _cropRepository.GetAllAsync();
                var farms = await _farmRepository.GetAllAsync();
                ViewBag.Crops = new SelectList(crops, "CropID", "CropName");
                ViewBag.Farms = new SelectList(farms, "FarmID", "FarmName");
                return View(batch);
            }
            try
            {
                await _batchRepository.CreateAsync(batch);
                TempData["SuccessMessage"] = "Harvest batch created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating batch");
                TempData["ErrorMessage"] = "Failed to create harvest batch.";
                var crops = await _cropRepository.GetAllAsync();
                var farms = await _farmRepository.GetAllAsync();
                ViewBag.Crops = new SelectList(crops, "CropID", "CropName");
                ViewBag.Farms = new SelectList(farms, "FarmID", "FarmName");
                return View(batch);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var batch = await _batchRepository.GetByIdAsync(id);
                if (batch == null) return NotFound();
                
                var crops = await _cropRepository.GetAllAsync();
                var farms = await _farmRepository.GetAllAsync();
                ViewBag.Crops = new SelectList(crops, "CropID", "CropName");
                ViewBag.Farms = new SelectList(farms, "FarmID", "FarmName");
                
                return View(batch);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading batch for edit");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HarvestBatch batch)
        {
            if (id != batch.BatchID) return BadRequest();
            if (!ModelState.IsValid)
            {
                var crops = await _cropRepository.GetAllAsync();
                var farms = await _farmRepository.GetAllAsync();
                ViewBag.Crops = new SelectList(crops, "CropID", "CropName");
                ViewBag.Farms = new SelectList(farms, "FarmID", "FarmName");
                return View(batch);
            }

            try
            {
                await _batchRepository.UpdateAsync(batch);
                TempData["SuccessMessage"] = "Harvest batch updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating batch");
                TempData["ErrorMessage"] = "Failed to update harvest batch.";
                var crops = await _cropRepository.GetAllAsync();
                var farms = await _farmRepository.GetAllAsync();
                ViewBag.Crops = new SelectList(crops, "CropID", "CropName");
                ViewBag.Farms = new SelectList(farms, "FarmID", "FarmName");
                return View(batch);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _batchRepository.DeleteAsync(id);
                TempData["SuccessMessage"] = "Harvest batch deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting batch");
                TempData["ErrorMessage"] = "Failed to delete harvest batch.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
