using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.ViewModels
{
    public class LoginVM
    {
        [MaxLength(100)]
        [Required]
        [Display(Name ="Email/Phone")]
        public string Username { get; set; }
        [MaxLength(18)]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember me")]
        public bool Rememberme { get; set; } = false;
    }
}
