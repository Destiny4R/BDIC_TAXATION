using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.Models
{
    public class IndividualTaxpayer
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string FirstName { get; set; }
        [StringLength(100)]
        public string? OtherName { get; set; }
        [StringLength(100)]
        public string Surname { get; set; }
        public DateTime DateofBirth { get; set; }
        [StringLength(10)]
        public string? Gender { get; set; }
        [StringLength(11)]
        public string Phone { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(50)]
        public string? TaxIdentificationNumber { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        [StringLength(100)]
        public string? LGA { get; set; }
        [StringLength(11)]
        public string NationalIdentificationNumber { get; set; }
        [StringLength(11)]
        public string? TIN { get; set; }
        [StringLength(100)]
        public string? Occupation { get; set; }
        [StringLength(100)]
        public string? ResidentialAddress { get; set; }
        [StringLength(450)]
        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser Applicationuser { get; set; }
        public string? Longitude { get; set; }
        [StringLength(150)]
        public string? Latitude { get; set; }
        [StringLength(50)]
        public string? IpAddress { get; set; }
        [StringLength(100)]
        public string? BusinessType { get; set; }
        [StringLength(100)]
        public string? IndustrySector { get; set; }
        [StringLength(100)]
        public string? BusinessName { get; set; }
        public bool IsProfileComplete { get; set; } = false;
        public bool IsProfileComplete2 { get; set; }= false;
        [StringLength(750)]
        public string? Others { get; set; }
    }
}
