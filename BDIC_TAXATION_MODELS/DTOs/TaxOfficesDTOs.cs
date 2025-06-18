using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.DTOs
{
    public class TaxOfficesDTOs
    {
        public int? id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string lga { get; set; }
        public int lgaId { get; set; }
        public string officerincharge { get; set; }
        public bool status { get; set; }
        public DateTime createdate { get; set; }
    }
}
