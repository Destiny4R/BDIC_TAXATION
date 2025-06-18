using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.ViewModels
{
    public class IndividualVM
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
        [MaxLength(11)]
        [Required]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please provide a valid phone number")]
        public string Phone { get; set; }
        [MaxLength(100)]
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please provide a valid email address")]
        public string Email { get; set; }
        [MaxLength(11)]
        [Required]
        [Display(Name = "National Identification Number")]
        public string NationalIdentificationNumber { get; set; }
        [MaxLength(1550)]
        public string? Logdata { get; set; }
        [MaxLength(18)]
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "You must agree to the Terms & Conditions.")]
        public bool Agree { get; set; }
        //[MaxLength(18)]
        //[Required]
        //[MinLength(8)]
        //[DataType(DataType.Password)]
        //[Compare("Password", ErrorMessage = "Password does not match")]
        //public string ConfirmPassword { get; set; }
    }
}
