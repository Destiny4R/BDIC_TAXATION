using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.Models
{
    public class ConsultantUser
    {
        public int Id { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(11)]
        public string Phone { get; set; }
        [StringLength(50)]
        public string RC_Number { get; set; }
        [StringLength(150)]
        public string Address { get; set; }
        [StringLength(50)]
        public string? TaxIdentificationNumber { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        [StringLength(450)]
        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser Applicationuser { get; set; }
        public int SpecialId { get; set; }
        [ForeignKey(nameof(SpecialId))]
        public Specialization Specialization { get; set; }
        public string? Longitude { get; set; }
        [StringLength(150)]
        public string? Latitude { get; set; }
        [StringLength(750)]
        public string? IpAddress { get; set; }
        [StringLength(750)]
        public string? Others { get; set; }
        [StringLength(450)]
        public string? AddedBy { get; set; }
    }
}
