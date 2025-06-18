using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.ViewModels
{
    public class Stations
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? StationCode { get; set; }
        public int LGId { get; set; }
        public string? Lgas {  get; set; }
    }
}
