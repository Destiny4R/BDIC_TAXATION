using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.ViewModels
{
    public class ChangepasswordVM
    {
        [Display(Name = "Current Password")]
        [MaxLength(18)]
        [Required(ErrorMessage = "Old password cannot be empty")]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string OldPassword { get; set; }
        [MaxLength(18)]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [Required(ErrorMessage = "Password cannot be empty")]
        public string NewPassword { get; set; }
        [Display(Name = "Confirm Password")]
        [MaxLength(18)]
        [MinLength(8)]
        [Required(ErrorMessage = "Confirm password cannot be empty")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword),ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
