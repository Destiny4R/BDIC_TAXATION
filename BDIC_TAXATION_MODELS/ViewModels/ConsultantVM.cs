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
    public class ConsultantVM
    {
        public int Id { get; set; }
        [MaxLength(150)]
        [Required(ErrorMessage = "Company name is required")]
        [Display(Name = "Company Name")]
        public string Name { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [MaxLength(11)]
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage = "RC Number is required")]
        [Display(Name = "RC Number")]
        public string RC_Number { get; set; }
        [MaxLength(150)]
        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [MaxLength(50)]
        [Display(Name = "Tax Identification Number")]
        [Required(ErrorMessage = "Tax Identification Number is required")]
        public string TaxIdentificationNumber { get; set; }
        public ApplicationUser? Applicationuser { get; set; }
        [MaxLength(750)]
        public string? Others { get; set; }
        [MaxLength(450)]
        public string? AddedBy { get; set; }
    }
}
