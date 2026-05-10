using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using FarmToTable.Models;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly IOrderDetailRepository _detailRepository;
        private readonly IHarvestBatchRepository _batchRepository;
        private readonly ILogger<OrderDetailsController> _logger;

        public OrderDetailsController(
            IOrderDetailRepository detailRepository,
            IHarvestBatchRepository batchRepository,
            ILogger<OrderDetailsController> logger)
        {
            _detailRepository = detailRepository;
            _batchRepository = batchRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int orderId)
        {
            try
            {
                ViewBag.OrderID = orderId;
                var details = await _detailRepository.GetAllByOrderIdAsync(orderId);
                return View(details);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading order details");
                TempData["ErrorMessage"] = "Failed to load order details.";
                return RedirectToAction("Index", "Orders");
            }
        }

        public async Task<IActionResult> Create(int orderId)
        {
            try
            {
                var batches = await _batchRepository.GetAllAsync();
                var batchOptions = batches.Select(b => new { 
                    BatchID = b.BatchID, 
                    DisplayInfo = $"Batch {b.BatchID} - {b.CropName} from {b.FarmName} (Avail: {b.Quantity})" 
                });
                ViewBag.Batches = new SelectList(batchOptions, "BatchID", "DisplayInfo");
                return View(new OrderDetail { OrderID = orderId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading form data");
                return RedirectToAction(nameof(Index), new { orderId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderDetail detail)
        {
            if (!ModelState.IsValid)
            {
                var batches = await _batchRepository.GetAllAsync();
                var batchOptions = batches.Select(b => new { 
                    BatchID = b.BatchID, 
                    DisplayInfo = $"Batch {b.BatchID} - {b.CropName} from {b.FarmName} (Avail: {b.Quantity})" 
                });
                ViewBag.Batches = new SelectList(batchOptions, "BatchID", "DisplayInfo");
                return View(detail);
            }
            try
            {
                await _detailRepository.CreateAsync(detail);
                TempData["SuccessMessage"] = "Order detail added successfully.";
                return RedirectToAction(nameof(Index), new { orderId = detail.OrderID });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order detail");
                TempData["ErrorMessage"] = "Failed to add order detail.";
                return View(detail);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int orderId)
        {
            try
            {
                await _detailRepository.DeleteAsync(id);
                TempData["SuccessMessage"] = "Order detail removed successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order detail");
                TempData["ErrorMessage"] = "Failed to remove order detail.";
            }
            return RedirectToAction(nameof(Index), new { orderId });
        }
    }
}
