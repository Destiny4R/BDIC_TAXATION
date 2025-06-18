using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.Models
{
    public class Payment
    {
        public int Id { get; set; }
        [StringLength(150)]
        public string PaymentReference { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        [StringLength(150)]
        public string PaymentMethod { get; set; }
        public bool PaymentStatus { get; set; }
        public int? FileidividualTaxId { get; set; }
        public decimal IndividualTaxAmount { get; set; }
        public int? BusinessTaxId { get; set; }
        public decimal BusinessTaxAmount { get; set; }
        public int? MDAsMinistryId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [ForeignKey(nameof(FileidividualTaxId))]
        public FileReturnIndividual fileReturnIndividual { get; set; }
        [ForeignKey(nameof(BusinessTaxId))]
        public BusinessTaxpayer BusinessTaxpayer { get; set; }
        [ForeignKey(nameof(MDAsMinistryId))]
        public MDAsMinistry MDAsMinistry { get; set; }
        [StringLength(150)]
        public string? Longitude { get; set; }
        [StringLength(150)]
        public string? Latitude { get; set; }
        [StringLength(750)]
        public string? IpAddress { get; set; }
        [StringLength(750)]
        public string? Others { get; set; }
    }
}
