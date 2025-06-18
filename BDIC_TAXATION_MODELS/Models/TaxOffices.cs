using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.Models
{
    public class TaxOffices
    {
        public int Id { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(150)]
        public string Code { get; set; }
        [StringLength(150)]
        public string Address { get; set; }
        [StringLength(11)]
        public string Phone { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(100)]
        public string OfficerInCharge { get; set; }
        public bool Status { get; set; } = true;
        [StringLength(450)]
        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; }
        public int LgaId { get; set; }
        [ForeignKey(nameof(LgaId))]
        public LocalGovernment LocalGovernment { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
