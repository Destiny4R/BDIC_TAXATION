using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.Models
{
    public class LGAStations
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public int StationCode { get; set; }
        public int LGId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [ForeignKey(nameof(LGId))]
        public IList<LocalGovernment> Localgovernment { get; set; }

    }
}
