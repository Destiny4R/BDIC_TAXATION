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
    public class IndividualProfileVM
    {
        public int? Id { get; set; }
        [MaxLength(20)]
        [Display(Name = "Tax Identification Number")]
        [Required]
        public string TaxNumber { get; set; }
        [MaxLength(11)]
        [Display(Name = "NIN")]
        public string NIN { get; set; }
        [MaxLength(100)]
        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }
        [MaxLength(100)]
        [Display(Name = "Other Name")]
        public string OtherName { get; set; }
        [MaxLength(50)]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [MaxLength(100)]
        [Required]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please provide a valid Phone Number")]
        public string Phone { get; set; }
        public string? Email { get; set; }
        [MaxLength(100)]
        [Required]
        [Display(Name = "Residential Address")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Date of Birth")]
        public string DateofBirth { get; set; }
        [MaxLength(10)]
        [Required]
        [Display(Name = "Gender")]
        public string? Gender { get; set; }
        [MaxLength(100)]
        [Required]
        [Display(Name = "Occupation")]
        public string? Occupation { get; set; }
        [MaxLength(100)]
        [Required]
        [Display(Name = "Local Government")]
        public string? LGA { get; set; }

    }
}
