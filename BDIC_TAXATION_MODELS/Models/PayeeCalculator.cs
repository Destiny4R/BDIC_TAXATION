using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.Models
{
    public class PayeeCalculator
    {
        public int Id { get; set; }
        public double Percent { get; set; }
        public double HighestAmount { get; set; }
        public double LowestAmount { get; set; }
        [StringLength(20)]
        public string Code { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
