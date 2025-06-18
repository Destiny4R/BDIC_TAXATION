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
    public class IndividualAccountVM
    {
        public int? Id { get; set; }
        [MaxLength(100)]
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(100)]
        [Display(Name = "Other Name")]
        public string? OtherName { get; set; }
        [MaxLength(100)]
        [Required]
        public string Surname { get; set; }
        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage ="Date of birth is required")]
        public DateTime DateofBirth { get; set; }
        [MaxLength(10)]
        [Required]
        public string Gender { get; set; }
        [MaxLength(11)]
        [Required]
        [DataType(DataType.PhoneNumber, ErrorMessage ="Please provide a valid phone number")]
        public string Phone { get; set; }
        [MaxLength(100)]
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please provide a valid email address")]
        public string Email { get; set; }
        [MaxLength(50)]
        [Display(Name = "Tax Identification Number")]
        public string? TaxIdentificationNumber { get; set; }
        [MaxLength(100)]
        [Required]
        [Display(Name = "Local Government Area")]
        public string LGA { get; set; }
        [MaxLength(11)]
        [Required]
        [Display(Name = "National Identification Number")]
        public string NationalIdentificationNumber { get; set; }
        [MaxLength(11)]
        [Required]
        [Display(Name = "Bank Verification Number")]
        public string BankVerificationNumber { get; set; }
        [MaxLength(100)]
        [Required]
        public string Occupation { get; set; }
        [MaxLength(100)]
        [Required]
        [Display(Name = "Residential Address")]
        public string ResidentialAddress { get; set; }
        [MaxLength(450)]
        public string? ApplicationUserId { get; set; }
        [MaxLength(1550)]
        public string? Logdata { get; set; }
        [MaxLength(18)]
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [MaxLength(18)]
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password does not match")]
        public string ConfirmPassword { get; set; }
        
    }
}
