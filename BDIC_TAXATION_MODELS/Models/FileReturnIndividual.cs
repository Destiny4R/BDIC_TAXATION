using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.Models
{
    public class FileReturnIndividual
    {
        public int Id { get; set; }
        public double IncomeAmount { get; set; }
        public double TaxAmount { get; set; }
        public double Percent { get; set; }
        [StringLength(250)]
        public string AssetName { get; set; }
        [StringLength(250)]
        public string ReferenceNo { get; set; }
        [StringLength(250)]
        public string AssetTaxable { get; set; }
        public bool CanPaidStatus { get; set; } = false;
        public bool IsReject { get; set; } = false;
        [StringLength(500)]
        public string? Message { get; set; }
        [StringLength(20)]
        public string? FlaggedReason { get; set; }
        public bool CertificateStatus { get; set; } = false;
        [StringLength(20)]
        public string SessionYear { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public int? IndividualId { get; set; }
        [ForeignKey(nameof(IndividualId))]
        public IndividualTaxpayer IndividualTaxpayer { get; set; }
        public int? BusinessTaxId { get; set; }
        [ForeignKey(nameof(BusinessTaxId))]
        public BusinessTaxpayer BusinessTaxpayer { get; set; }
        public TaxIndividualCertificate TaxIndividualCertificate { get; set; }
    }
}
