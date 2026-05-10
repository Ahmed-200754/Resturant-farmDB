using System;
using System.ComponentModel.DataAnnotations;

namespace FarmToTable.Models
{
    public class HarvestBatch
    {
        public int BatchID { get; set; }

        [Required(ErrorMessage = "Harvest Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Harvest Date")]
        public DateTime HarvestDate { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than zero")]
        public double Quantity { get; set; }

        [Required(ErrorMessage = "Freshness Window is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Freshness Window")]
        public DateTime FreshnessWindow { get; set; }

        [Required(ErrorMessage = "Crop is required")]
        [Display(Name = "Crop")]
        public int CropID { get; set; }

        [Required(ErrorMessage = "Farm is required")]
        [Display(Name = "Farm")]
        public int FarmID { get; set; }

        // Joined properties for view
        public string? CropName { get; set; }
        public string? FarmName { get; set; }
    }
}
