using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.DTOs
{
    public class firereturnDTO
    {
        public int id { get; set; }
        public string income { get; set; }
        public string referenceno { get; set; }
        public string taxamount { get; set; }
        public double percent { get; set; }
        public string assetname { get; set; }
        public string assettaxable { get; set; }
        public bool status { get; set; }
        public string session { get; set; }
        public DateTime createddate { get; set; }
        public string? message { get; set; }
        public bool transactionstate { get; set; }
    }
}
