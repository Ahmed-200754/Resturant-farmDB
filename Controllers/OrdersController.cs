using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using FarmToTable.Models;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(
            IOrderRepository orderRepository,
            IRestaurantRepository restaurantRepository,
            ILogger<OrdersController> logger)
        {
            _orderRepository = orderRepository;
            _restaurantRepository = restaurantRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var orders = await _orderRepository.GetAllAsync();
                return View(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading orders");
                TempData["ErrorMessage"] = "Failed to load orders.";
                return View("Error");
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var restaurants = await _restaurantRepository.GetAllAsync();
                ViewBag.Restaurants = new SelectList(restaurants, "RestaurantID", "RestaurantName");
                return View(new Order { OrderDate = DateTime.Today });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading form data");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (!ModelState.IsValid)
            {
                var restaurants = await _restaurantRepository.GetAllAsync();
                ViewBag.Restaurants = new SelectList(restaurants, "RestaurantID", "RestaurantName");
                return View(order);
            }
            try
            {
                await _orderRepository.CreateAsync(order);
                TempData["SuccessMessage"] = "Order created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                TempData["ErrorMessage"] = "Failed to create order.";
                var restaurants = await _restaurantRepository.GetAllAsync();
                ViewBag.Restaurants = new SelectList(restaurants, "RestaurantID", "RestaurantName");
                return View(order);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _orderRepository.DeleteAsync(id);
                TempData["SuccessMessage"] = "Order deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order");
                TempData["ErrorMessage"] = "Failed to delete order.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
