using System;
using System.ComponentModel.DataAnnotations;

namespace FarmToTable.Models
{
    public class Restaurant
    {
        public int RestaurantID { get; set; }

        [Required(ErrorMessage = "Restaurant Name is required")]
        [StringLength(50)]
        [Display(Name = "Restaurant Name")]
        public string RestaurantName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [StringLength(70)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Delivery Window is required")]
        [Display(Name = "Delivery Window")]
        public TimeSpan DeliveryWindow { get; set; }
    }
}
