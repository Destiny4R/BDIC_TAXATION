using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.Models
{
    public class TaxPaymentPercentSettings
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double MinimunAmount { get; set; }
        public double MaximunAmount { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        [StringLength(250)]
        public string ApplicationuserId { get; set; }
        [ForeignKey(nameof(ApplicationuserId))]
        public ApplicationUser Applicationuser { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
