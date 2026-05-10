using System.ComponentModel.DataAnnotations;

namespace FarmToTable.Models
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }

        [Required(ErrorMessage = "Unit Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit Price must be greater than zero")]
        [Display(Name = "Unit Price")]
        public double UnitPrice { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than zero")]
        public double Quantity { get; set; }

        [Required(ErrorMessage = "Order is required")]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Batch is required")]
        [Display(Name = "Batch")]
        public int BatchID { get; set; }

        // Joined properties
        public string? CropName { get; set; }
        public string? FarmName { get; set; }
        public double LineTotal => UnitPrice * Quantity;
    }
}
