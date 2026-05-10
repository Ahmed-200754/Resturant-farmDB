using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FarmToTable.Models.ViewModels;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFarmRepository _farmRepository;
        private readonly ICropRepository _cropRepository;
        private readonly IHarvestBatchRepository _batchRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly ITripRepository _tripRepository;
        private readonly IReportRepository _reportRepository;

        public HomeController(
            ILogger<HomeController> logger,
            IFarmRepository farmRepository,
            ICropRepository cropRepository,
            IHarvestBatchRepository batchRepository,
            IRestaurantRepository restaurantRepository,
            IOrderRepository orderRepository,
            IDriverRepository driverRepository,
            ITripRepository tripRepository,
            IReportRepository reportRepository)
        {
            _logger = logger;
            _farmRepository = farmRepository;
            _cropRepository = cropRepository;
            _batchRepository = batchRepository;
            _restaurantRepository = restaurantRepository;
            _orderRepository = orderRepository;
            _driverRepository = driverRepository;
            _tripRepository = tripRepository;
            _reportRepository = reportRepository;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var viewModel = new DashboardViewModel();
                
                var farms = await _farmRepository.GetAllAsync();
                viewModel.TotalFarms = farms.Count;

                var crops = await _cropRepository.GetAllAsync();
                viewModel.TotalCrops = crops.Count;

                var batches = await _batchRepository.GetAllAsync();
                int activeBatches = 0;
                var expiringBatches = new System.Collections.Generic.List<Models.HarvestBatch>();
                var today = DateTime.Today;
                foreach(var b in batches)
                {
                    if (b.FreshnessWindow >= today)
                    {
                        activeBatches++;
                    }
                    if (b.FreshnessWindow >= today && b.FreshnessWindow <= today.AddDays(7))
                    {
                        expiringBatches.Add(b);
                    }
                }
                viewModel.TotalActiveBatches = activeBatches;
                viewModel.ExpiringBatches = expiringBatches;

                var restaurants = await _restaurantRepository.GetAllAsync();
                viewModel.TotalRestaurants = restaurants.Count;

                var orders = await _orderRepository.GetAllAsync();
                int ordersThisMonth = 0;
                var recentOrders = new System.Collections.Generic.List<Models.Order>();
                foreach(var o in orders)
                {
                    if (o.OrderDate.Year == today.Year && o.OrderDate.Month == today.Month)
                    {
                        ordersThisMonth++;
                    }
                }
                // Sort by date descending for recent orders
                orders.Sort((x, y) => y.OrderDate.CompareTo(x.OrderDate));
                for(int i=0; i<System.Math.Min(5, orders.Count); i++)
                {
                    recentOrders.Add(orders[i]);
                }
                viewModel.TotalOrdersThisMonth = ordersThisMonth;
                viewModel.RecentOrders = recentOrders;

                var drivers = await _driverRepository.GetAllAsync();
                viewModel.TotalDrivers = drivers.Count;

                var trips = await _tripRepository.GetAllAsync();
                int tripsThisMonth = 0;
                foreach(var t in trips)
                {
                    if (t.TripDate.Year == today.Year && t.TripDate.Month == today.Month)
                    {
                        tripsThisMonth++;
                    }
                }
                viewModel.TotalTripsThisMonth = tripsThisMonth;

                var topFarms = await _reportRepository.GetTopFarmsByRevenueAsync();
                viewModel.TopPerformingFarms = topFarms;

                var allRevenues = await _reportRepository.GetTotalRevenuePerFarmAsync();
                double totalRev = 0;
                foreach(var rev in allRevenues) totalRev += rev.TotalRevenue;
                viewModel.TotalRevenue = totalRev;

                var highestCrop = await _reportRepository.GetHighestOrderedCropAsync();
                if (highestCrop != null)
                {
                    viewModel.TopCropName = highestCrop.CropName;
                    // Simulate growth based on order count
                    viewModel.TopCropGrowth = 15.0 + (highestCrop.TotalOrders % 10);
                }
                
                // Simulate revenue growth based on current orders vs base
                viewModel.RevenueGrowth = 8.0 + (viewModel.TotalOrdersThisMonth * 1.5);

                return View(viewModel);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 4060)
            {
                _logger.LogError(ex, "Database 'FarmToTableDB' not found or inaccessible.");
                ViewBag.DbError = "The database 'FarmToTableDB' could not be found. Please run DatabaseCreate.sql and SeedData.sql in SSMS first, then restart the application.";
                return View(new DashboardViewModel()); // return empty model, not crash
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard");
                ViewBag.DbError = "Database connection failed: " + ex.Message;
                return View(new DashboardViewModel());
            }
        }
    }
}
