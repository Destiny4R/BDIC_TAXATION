using BDIC_TAXATION_MODELS.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_ACCESS.SystemRepos
{
    public interface IMailSender
    {
        int SendMail(string SendTo, string Subject, string Body, List<string> attachments=null);
        int SendMail1(string SendTo, string Subject, string Body, IFormFileCollection attachments = null);
        int SendMail2(string SendTo, string Subject, string Body, IFormFileCollection attachments = null);
    }
}
