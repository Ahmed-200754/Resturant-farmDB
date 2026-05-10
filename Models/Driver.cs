using System;
using System.ComponentModel.DataAnnotations;

namespace FarmToTable.Models
{
    public class Driver
    {
        public int DriverID { get; set; }

        [Required(ErrorMessage = "Driver Name is required")]
        [StringLength(50)]
        [Display(Name = "Driver Name")]
        public string DriverName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DriverDOB { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [StringLength(20)]
        [Display(Name = "Phone")]
        public string DriverPhone { get; set; } = string.Empty;

        public int Age 
        { 
            get 
            {
                var today = DateTime.Today;
                var age = today.Year - DriverDOB.Year;
                if (DriverDOB.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
    }
}
