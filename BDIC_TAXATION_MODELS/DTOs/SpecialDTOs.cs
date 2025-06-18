using BDIC_TAXATION_MODELS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.DTOs
{
    public class SpecialDTOs
    {
        public int? id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public DateTime createdate { get; set; }
    }
}
