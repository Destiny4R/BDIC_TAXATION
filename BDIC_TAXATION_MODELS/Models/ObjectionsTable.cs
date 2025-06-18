using BDIC_TAXATION_UTILITIES;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.Models
{
    public class ObjectionsTable
    {
        public int Id { get; set; }
        [StringLength(1000)]
        public string Message { get; set; }
        [StringLength(1000)]
        public string? Remarks { get; set; }
        [StringLength(50)]
        public string ProcessStatus { get; set; }
        [StringLength(50)]
        public string ObjectionReason { get; set; }
        public double SuggectedAmount { get; set; }
        [StringLength(50)]
        public string AuditCaseId { get; set; }
        [StringLength(50)]
        public string ReferenceId { get; set; } //SD.GenerateUniqueNumberWithInnitials("OBJ");
        [StringLength(450)]
        public string FilePath { get; set; }
        public bool Status { get; set; } = false;
        [StringLength(100)]
        public string FileName { get; set; }
        public int FileTaxId { get; set; }
        [ForeignKey(nameof(FileTaxId))]
        public FileReturnIndividual FileReturnIndividual { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
