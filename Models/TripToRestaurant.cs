using System;
using System.ComponentModel.DataAnnotations;

namespace FarmToTable.Models
{
    public class TripToRestaurant
    {
        public int TripID { get; set; }

        [Required(ErrorMessage = "Trip Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Trip Date")]
        public DateTime TripDate { get; set; }

        [Required(ErrorMessage = "Distance is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Distance must be at least 1 km")]
        public int Distance { get; set; }

        [Required(ErrorMessage = "Route Taken is required")]
        [StringLength(100)]
        [Display(Name = "Route Taken")]
        public string RouteTaken { get; set; } = string.Empty;

        [Required(ErrorMessage = "Driver is required")]
        [Display(Name = "Driver")]
        public int DriverID { get; set; }

        [Required(ErrorMessage = "Farm is required")]
        [Display(Name = "Farm")]
        public int FarmID { get; set; }

        // Joined properties
        public string? DriverName { get; set; }
        public string? FarmName { get; set; }
    }
}
