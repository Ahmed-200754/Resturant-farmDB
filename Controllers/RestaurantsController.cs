using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FarmToTable.Models;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly ILogger<RestaurantsController> _logger;

        public RestaurantsController(IRestaurantRepository restaurantRepository, ILogger<RestaurantsController> logger)
        {
            _restaurantRepository = restaurantRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var restaurants = await _restaurantRepository.GetAllAsync();
                return View(restaurants);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading restaurants");
                TempData["ErrorMessage"] = "Failed to load restaurants.";
                return View("Error");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Restaurant restaurant)
        {
            if (!ModelState.IsValid) return View(restaurant);
            try
            {
                await _restaurantRepository.CreateAsync(restaurant);
                TempData["SuccessMessage"] = "Restaurant created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating restaurant");
                TempData["ErrorMessage"] = "Failed to create restaurant.";
                return View(restaurant);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var restaurant = await _restaurantRepository.GetByIdAsync(id);
                if (restaurant == null) return NotFound();
                return View(restaurant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading restaurant for edit");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Restaurant restaurant)
        {
            if (id != restaurant.RestaurantID) return BadRequest();
            if (!ModelState.IsValid) return View(restaurant);

            try
            {
                await _restaurantRepository.UpdateAsync(restaurant);
                TempData["SuccessMessage"] = "Restaurant updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating restaurant");
                TempData["ErrorMessage"] = "Failed to update restaurant.";
                return View(restaurant);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _restaurantRepository.DeleteAsync(id);
                TempData["SuccessMessage"] = "Restaurant deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting restaurant");
                TempData["ErrorMessage"] = "Failed to delete restaurant. It may have associated records.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
