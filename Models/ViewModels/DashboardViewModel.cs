using System.Collections.Generic;
using FarmToTable.Models;

namespace FarmToTable.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalFarms { get; set; } = 0;
        public int TotalCrops { get; set; } = 0;
        public int TotalActiveBatches { get; set; } = 0;
        public int TotalRestaurants { get; set; } = 0;
        public int TotalOrdersThisMonth { get; set; } = 0;
        public int TotalDrivers { get; set; } = 0;
        public int TotalTripsThisMonth { get; set; } = 0;
        public double TotalRevenue { get; set; } = 0;

        public List<Order> RecentOrders { get; set; } = new List<Order>();
        public List<HarvestBatch> ExpiringBatches { get; set; } = new List<HarvestBatch>();
        public List<FarmRevenue> TopPerformingFarms { get; set; } = new List<FarmRevenue>();
    }

    public class FarmRevenue
    {
        public int FarmID { get; set; }
        public string FarmName { get; set; } = string.Empty;
        public double TotalRevenue { get; set; }
    }
}
