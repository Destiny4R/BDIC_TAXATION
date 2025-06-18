using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.Models
{
    public class MDAsTaxAssets
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int MDAsAssetsId { get; set; }
        [ForeignKey(nameof(MDAsAssetsId))]
        public MDAsMinistry MDAsministry { get; set; }
       
    }
}
