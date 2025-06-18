using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.ViewModels
{
    public class ObjectionsVM
    {
        public int id { get; set; }
        public string name { get; set; }
        public string payerid { get; set; }
        public string reference { get; set; }
        public string objectiontype { get; set; }
        public string income { get; set; }
        public string taxamount { get; set; }
        public string session { get; set; }
        public DateTime createdate { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string filepath { get; set; }
        public string filename { get; set; }
        public string reason { get; set; }
    }
}
