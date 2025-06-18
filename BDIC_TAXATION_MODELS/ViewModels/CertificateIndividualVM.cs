using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.ViewModels
{
    public class CertificateIndividualVM
    {
        public int Id { get; set; }
        public string fullname { get; set; }
        public string username { get; set; }
        public DateTime createdate { get; set; }
        public string? session { get; set; }
        public string filepath { get; set; }
        public string filename { get; set; }
        public string purpose { get; set; }
        public bool isalltaxfiled { get; set; }
        public bool isallpaymentmade { get; set; }
    }
}
