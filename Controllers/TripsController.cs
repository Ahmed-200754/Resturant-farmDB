using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using FarmToTable.Models;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Controllers
{
    public class TripsController : Controller
    {
        private readonly ITripRepository _tripRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly IFarmRepository _farmRepository;
        private readonly ILogger<TripsController> _logger;

        public TripsController(
            ITripRepository tripRepository,
            IDriverRepository driverRepository,
            IFarmRepository farmRepository,
            ILogger<TripsController> logger)
        {
            _tripRepository = tripRepository;
            _driverRepository = driverRepository;
            _farmRepository = farmRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var trips = await _tripRepository.GetAllAsync();
                return View(trips);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading trips");
                TempData["ErrorMessage"] = "Failed to load trips.";
                return View("Error");
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var drivers = await _driverRepository.GetAllAsync();
                var farms = await _farmRepository.GetAllAsync();
                ViewBag.Drivers = new SelectList(drivers, "DriverID", "DriverName");
                ViewBag.Farms = new SelectList(farms, "FarmID", "FarmName");
                return View(new TripToRestaurant { TripDate = DateTime.Today });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading form data");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TripToRestaurant trip)
        {
            if (!ModelState.IsValid)
            {
                var drivers = await _driverRepository.GetAllAsync();
                var farms = await _farmRepository.GetAllAsync();
                ViewBag.Drivers = new SelectList(drivers, "DriverID", "DriverName");
                ViewBag.Farms = new SelectList(farms, "FarmID", "FarmName");
                return View(trip);
            }
            try
            {
                await _tripRepository.CreateAsync(trip);
                TempData["SuccessMessage"] = "Trip created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating trip");
                TempData["ErrorMessage"] = "Failed to create trip.";
                var drivers = await _driverRepository.GetAllAsync();
                var farms = await _farmRepository.GetAllAsync();
                ViewBag.Drivers = new SelectList(drivers, "DriverID", "DriverName");
                ViewBag.Farms = new SelectList(farms, "FarmID", "FarmName");
                return View(trip);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var trip = await _tripRepository.GetByIdAsync(id);
                if (trip == null) return NotFound();
                
                var drivers = await _driverRepository.GetAllAsync();
                var farms = await _farmRepository.GetAllAsync();
                ViewBag.Drivers = new SelectList(drivers, "DriverID", "DriverName");
                ViewBag.Farms = new SelectList(farms, "FarmID", "FarmName");
                
                return View(trip);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading trip for edit");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TripToRestaurant trip)
        {
            if (id != trip.TripID) return BadRequest();
            if (!ModelState.IsValid)
            {
                var drivers = await _driverRepository.GetAllAsync();
                var farms = await _farmRepository.GetAllAsync();
                ViewBag.Drivers = new SelectList(drivers, "DriverID", "DriverName");
                ViewBag.Farms = new SelectList(farms, "FarmID", "FarmName");
                return View(trip);
            }

            try
            {
                await _tripRepository.UpdateAsync(trip);
                TempData["SuccessMessage"] = "Trip updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating trip");
                TempData["ErrorMessage"] = "Failed to update trip.";
                var drivers = await _driverRepository.GetAllAsync();
                var farms = await _farmRepository.GetAllAsync();
                ViewBag.Drivers = new SelectList(drivers, "DriverID", "DriverName");
                ViewBag.Farms = new SelectList(farms, "FarmID", "FarmName");
                return View(trip);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _tripRepository.DeleteAsync(id);
                TempData["SuccessMessage"] = "Trip deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting trip");
                TempData["ErrorMessage"] = "Failed to delete trip.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
