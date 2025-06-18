using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.Models
{
    public class LogActivity
    {
        public int Id { get; set; }
        [StringLength(150)]
        public string Browser { get; set; }
        [StringLength(150)]
        public string OperatingSystem { get; set; }
        [StringLength(150)]
        public string? Longitude { get; set; }
        [StringLength(150)]
        public string? Latitude { get; set; }
        [StringLength(50)]
        public string? IpAddress { get; set; }
        [StringLength(750)]
        public string? Others { get; set; }
        [StringLength(750)]
        public string? ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser Applicationuser { get; set; }
    }
}
