using BDIC_TAXATION_MODELS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.ViewModels
{
    public class BusinessTaxPayerVM
    {
        public int? Id { get; set; }
        [MaxLength(150)]
        [Required(ErrorMessage ="Provide your business name")]
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }
        [MaxLength(20)]
        [Display(Name = "Business Registration Number")]
        [Required(ErrorMessage = "Provide your valid business registration number")]
        public string RegistrationNumber { get; set; }
        [MaxLength(250)]
        [Display(Name = "Business Type")]
        [Required(ErrorMessage = "Provide your business type")]
        public string BusinessType { get; set; }
        [MaxLength(250)]
        [Required(ErrorMessage = "Provide your business industry sector")]
        [Display(Name = "Business Sector")]
        public string IndustrySector { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage = "Provide business Local Government Area")]
        [Display(Name = "Local Government Area")]
        public string LGA { get; set; }
        [MaxLength(150)]
        [Required(ErrorMessage = "Provide your business address")]
        public string Address { get; set; }
        [MaxLength(150)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Provide your business valid email address")]
        [Required(ErrorMessage = "Provide your business valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [MaxLength(11)]
        [Required(ErrorMessage = "Provide your business phone number")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name ="Phone Number")]
        public string PhoneNumber { get; set; }
        [MaxLength(450)]
        public string? ApplicationUserId { get; set; }
        [MaxLength(16)]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Compare(nameof(Password), ErrorMessage = "Password does not match")]
        [Required]
        [MaxLength(18)]
        [MinLength(8)]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [StringLength(2550)]
        public string? LogData { get; set; }
    }
}
