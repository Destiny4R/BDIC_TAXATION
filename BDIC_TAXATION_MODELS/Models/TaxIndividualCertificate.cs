using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.Models
{
    public class TaxIndividualCertificate
    {
        public int Id { get; set; }
        [StringLength(1500)]
        public string Description { get; set; }
        [StringLength(1500)]
        public string FilePath { get; set; }
        [StringLength(20)]
        public string SessionYear { get; set; }
        [StringLength(150)]
        public string? FileName { get; set; }
        public int FileReturnId { get; set; }
        [ForeignKey(nameof(FileReturnId))]
        public FileReturnIndividual FileReturnIndividual { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}
