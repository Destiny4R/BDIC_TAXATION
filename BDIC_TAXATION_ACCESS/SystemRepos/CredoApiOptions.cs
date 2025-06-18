using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_ACCESS.SystemRepos
{
    public class CredoApiOptions
    {
        public const string SectionName = "CredoApi";

        public string SecretKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
    }
}
