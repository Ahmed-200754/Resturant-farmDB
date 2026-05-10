using System;
using System.ComponentModel.DataAnnotations;

namespace FarmToTable.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Order Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Restaurant is required")]
        [Display(Name = "Restaurant")]
        public int RestaurantID { get; set; }

        // Joined properties for view
        public string? RestaurantName { get; set; }
        public int TotalItems { get; set; }
        public double TotalValue { get; set; }
    }
}
