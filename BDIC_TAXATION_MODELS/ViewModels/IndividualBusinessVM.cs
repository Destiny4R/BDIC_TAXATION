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
    public class IndividualBusinessVM
    {
        public int? Id { get; set; }
        [MaxLength(150)]
        [Required(ErrorMessage ="Provide your business name")]
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }
        [MaxLength(250)]
        [Display(Name = "Business Type")]
        [Required(ErrorMessage = "Provide your business type")]
        public string BusinessType { get; set; }
        [MaxLength(250)]
        [Required(ErrorMessage = "Provide your business industry sector")]
        [Display(Name = "Business Sector")]
        public string IndustrySector { get; set; }
        [MaxLength(150)]
        [Required(ErrorMessage = "Provide your business address")]
        public string Address { get; set; }
        [StringLength(2550)]
        public string? LogData { get; set; }
    }
}
