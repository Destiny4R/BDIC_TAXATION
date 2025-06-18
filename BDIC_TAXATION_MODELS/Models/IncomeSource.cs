using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.Models
{
    public class IncomeSource
    {
        [Key]
        public int Id { get; set; }
        public decimal AnnualIncome { get; set; }
        public string SourceName { get; set; }
        public int? BusinessTaxId { get; set; }
        public int? IndividualTaxId { get; set; }
        [ForeignKey(nameof(IndividualTaxId))]
        public IndividualTaxpayer IndividualTaxpayer { get; set; }
        [ForeignKey(nameof(BusinessTaxId))]
        public BusinessTaxpayer BusinessTaxpayer { get; set; }
    }
}
