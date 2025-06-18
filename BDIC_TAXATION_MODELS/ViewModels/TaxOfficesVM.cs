using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.ViewModels
{
    public class TaxOfficesVM
    {
        public int? Id { get; set; }
        [MaxLength(150)]
        [Required]
        [Display(Name="Office Name")]
        public string Name { get; set; }
        [MaxLength(150)]
        [Required]
        public string Address { get; set; }
        [MaxLength(11)]
        [Required]
        [Phone]
        public string Phone { get; set; }
        [MaxLength(50)]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Local Government Area")]
        public int LgaId { get; set; }
        [MaxLength(150)]
        [Required]
        [Display(Name = "Officer In Charge")]
        public string OfficerInCharge { get; set; }
    }
}
