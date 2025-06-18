using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDIC_TAXATION_MODELS.Models
{
    public class BusinessTaxpayer
    {
        public int Id { get; set; }
        [StringLength(150)]
        public string BusinessName { get; set; }
        [StringLength(20)]
        public string RegistrationNumber { get; set; }
        [StringLength(250)]
        public string BusinessType { get; set; }
        [StringLength(250)]
        public string IndustrySector { get; set; }
        [StringLength(100)]
        public string LGA { get; set; }
        [StringLength(150)]
        public string Address { get; set; }
        [StringLength(11)]
        public string PhoneNumber { get; set; }
        [StringLength(150)]
        public string Email { get; set; }
        [StringLength(50)]
        public string? TaxIdentificationNumber { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        [StringLength(450)]
        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser Applicationuser { get; set; }
        public string? Longitude { get; set; }
        [StringLength(150)]
        public string? Latitude { get; set; }
        [StringLength(750)]
        public string? IpAddress { get; set; }
        [StringLength(750)]
        public string? Others { get; set; }
    }
}
