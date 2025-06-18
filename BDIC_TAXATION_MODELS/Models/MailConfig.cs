using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.Models
{
    public class MailConfig
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpHost { get; set; }
    }
}
