using BDIC_TAXATION_MODELS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.DTOs
{
    public class consultantcy
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string rcnumber { get; set; }
        public string address { get; set; }
        public string tin { get; set; }
        public string username { get; set; }
    }
}
