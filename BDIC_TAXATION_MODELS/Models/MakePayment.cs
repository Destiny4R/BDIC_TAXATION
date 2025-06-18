using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.Models
{
    public class MakePayment
    {
        public int Id { get; set; }
        [StringLength(150)]
        public string Reference { get; set; }
        [StringLength(150)]
        public string? Party_Reference { get; set; }
        public double Amount { get; set; }
        public DateTime? PaymentDate { get; set; } = null;
        [StringLength(150)]
        public string? PaymentMethod { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }=DateTime.UtcNow;
        public int? FileidividualTaxId { get; set; }
        [ForeignKey(nameof(FileidividualTaxId))]
        public FileReturnIndividual fileReturnIndividual { get; set; }
    }
}
