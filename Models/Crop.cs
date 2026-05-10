using System.ComponentModel.DataAnnotations;

namespace FarmToTable.Models
{
    public class Crop
    {
        public int CropID { get; set; }

        [Required(ErrorMessage = "Crop Name is required")]
        [StringLength(50)]
        [Display(Name = "Crop Name")]
        public string CropName { get; set; } = string.Empty;

        // Navigation properties
        public int BatchCount { get; set; }
    }
}
