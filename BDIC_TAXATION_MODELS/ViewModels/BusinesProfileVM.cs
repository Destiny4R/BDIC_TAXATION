using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.ViewModels
{
    public class BusinesProfileVM
    {
        public int? Id { get; set; }
        [MaxLength(100)]
        [Required]
        [Display(Name = "Business Type")]
        public string BusinessType { get; set; }
        [Display(Name = "Business Registration Number")]
        [MaxLength(100)]
        [Required]
        public string RegNumber { get; set; }
        [Display(Name = "Business Sector")]
        [MaxLength(100)]
        [Required]
        public string IndustrySector { get; set; }
        [Display(Name = "Business Name")]
        [MaxLength(100)]
        [Required]
        public string BusinessName { get; set; }
        [MaxLength(200)]
        [Required]
        public string Address { get; set; }
        [MaxLength(200)]
        [Required]
        [Display(Name = "Local Government")]
        public string LGA { get; set; }
        [MaxLength(11)]
        [Required]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please provide a valid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [MaxLength(20)]
        [Display(Name = "Tax Identification Number")]
        public string? TaxIdentificationNumber { get; set; }
    }
}
