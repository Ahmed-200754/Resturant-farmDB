using System.ComponentModel.DataAnnotations;

namespace FarmToTable.Models
{
    public class Farm
    {
        public int FarmID { get; set; }

        [Required(ErrorMessage = "Farm Name is required")]
        [StringLength(50)]
        [Display(Name = "Farm Name")]
        public string FarmName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Farmer Name is required")]
        [StringLength(50)]
        [Display(Name = "Farmer Name")]
        public string FarmerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Farmer Phone is required")]
        [StringLength(20)]
        [Display(Name = "Farmer Phone")]
        public string FarmerPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Longitude is required")]
        [Range(-180.0, 180.0)]
        public decimal Longitude { get; set; }

        [Required(ErrorMessage = "Latitude is required")]
        [Range(-90.0, 90.0)]
        public decimal Latitude { get; set; }
    }
}
