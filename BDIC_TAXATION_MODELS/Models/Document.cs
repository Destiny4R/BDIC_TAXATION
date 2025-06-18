using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }
        public string FileNamePath { get; set; }
        public string FilePath { get; set; }
        public DateTime CreatetdDate { get; set; } = DateTime.UtcNow;
        public int? TaxCertificateId { get; set; }
        [ForeignKey(nameof(TaxCertificateId))]
        public TaxIndividualCertificate TaxIndividualCertificate { get; set; }
       //public int? BusinessTaxId { get; set; }
        //[ForeignKey(nameof(BusinessTaxId))]
        //public BusinessTaxpayer BusinessTaxpayer { get; set; }
    }
}
