using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.Models
{
    public class SystemManagers
    {
        public int Id { get; set; }
        [StringLength(150)]
        public string Surname { get; set; }

        [StringLength(150)]
        public string FirstName { get; set; }

        [StringLength(150)]
        public string OtherNames { get; set; }
        [StringLength(20)]
        public string LocationId { get; set; }
        [StringLength(250)]
        public string Position { get; set; }
        [StringLength(250)]
        public string ZonalCoverage { get; set; }
        [StringLength(100)]
        public string LGA { get; set; }
        [StringLength(150)]
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        [StringLength(450)]
        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser Applicationuser { get; set; }
    }
}
