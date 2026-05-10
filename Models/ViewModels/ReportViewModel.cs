using System.Collections.Generic;
using FarmToTable.Models;

namespace FarmToTable.Models.ViewModels
{
    public class ReportViewModel
    {
        public int SelectedMonth { get; set; }
        public int SelectedYear { get; set; }
        
        public CropReport? Report1 { get; set; }
        public List<Farm> Report2 { get; set; } = new List<Farm>();
        public DriverReport? Report3 { get; set; }
        public List<Restaurant> Report4 { get; set; } = new List<Restaurant>();
        public List<RestaurantBatchReport> Report5 { get; set; } = new List<RestaurantBatchReport>();
        public List<FarmRevenue> Report6 { get; set; } = new List<FarmRevenue>();
    }

    public class CropReport
    {
        public string CropName { get; set; } = string.Empty;
        public int TotalOrders { get; set; }
    }

    public class DriverReport
    {
        public int DriverID { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public int TripCount { get; set; }
    }

    public class RestaurantBatchReport
    {
        public string RestaurantName { get; set; } = string.Empty;
        public int BatchID { get; set; }
        public System.DateTime HarvestDate { get; set; }
        public double BatchQuantity { get; set; }
        public string CropName { get; set; } = string.Empty;
        public string FarmName { get; set; } = string.Empty;
        public double OrderedQuantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
