using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.ViewModels
{
    public class MDAsMinistryVM
    {
        public int? id { get; set; }
        [MaxLength(150)]
        [Required]
        public string name { get; set; }
        [MaxLength(50)]
        public string? code { get; set; }
        [MaxLength(450)]
        [Required]
        public string description { get; set; }
        [MaxLength(100)]
        [Required]
        [EmailAddress]
        public string email { get; set; }
        public DateTime establistdate { get; set; }
        public bool? status { get; set; }
        public DateTime? createdate { get; set; }
    }
}
