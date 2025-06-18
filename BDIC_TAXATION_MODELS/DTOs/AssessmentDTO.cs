using BDIC_TAXATION_MODELS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.DTOs
{
    public class AssessmentDTO
    {
        public int id { get; set; }
        public string companyname { get; set; }
        public string username { get; set; }
        public string phone { get; set; }
        public string session { get; set; }
        public DateTime createdate { get; set; }
        public string income { get; set; }
        public string amountleft { get; set; }
        public string taxamount { get; set; }
        public bool status { get; set; }
        public string filepath { get; set; }
        public string filename { get; set; }
        public IList<Documentx> document { get; set; }
    }
    public class Documentx
    {
        public string name { get; set; }
        public string path { get; set; }
    }
}
