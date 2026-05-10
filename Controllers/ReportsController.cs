using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FarmToTable.Models.ViewModels;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(IReportRepository reportRepository, ILogger<ReportsController> logger)
        {
            _reportRepository = reportRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int? month, int? year)
        {
            try
            {
                var currentMonth = month ?? DateTime.Today.Month;
                var currentYear = year ?? DateTime.Today.Year;

                var viewModel = new ReportViewModel
                {
                    SelectedMonth = currentMonth,
                    SelectedYear = currentYear,
                    Report1 = await _reportRepository.GetHighestOrderedCropAsync(),
                    Report2 = await _reportRepository.GetInactiveFarmsAsync(currentYear, currentMonth),
                    Report3 = await _reportRepository.GetTopDriverAsync(currentYear, currentMonth),
                    Report4 = await _reportRepository.GetRestaurantsWithNoOrdersAsync(currentYear, currentMonth),
                    Report5 = await _reportRepository.GetBatchesDeliveredToRestaurantsAsync(currentYear, currentMonth),
                    Report6 = await _reportRepository.GetTotalRevenuePerFarmAsync()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating reports");
                TempData["ErrorMessage"] = "Failed to generate reports.";
                return View("Error");
            }
        }
    }
}
