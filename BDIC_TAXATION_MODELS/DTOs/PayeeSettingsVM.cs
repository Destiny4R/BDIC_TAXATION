using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.DTOs
{
    public class PayeeSettingsVM
    {
        public int id { get; set; }
        public float percent { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string code { get; set; }
        public DateTime createddate { get; set; }
    }
}
