using BDIC_TAXATION_MODELS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace BDIC_TAXATION_ACCESS.SystemRepos
{
    public class MailSender : IMailSender
    {
        private readonly MailConfig _options;

        public MailSender(IOptions<MailConfig> options)
        {
            _options = options.Value;
        }
        public int SendMail(string sendTo, string subject, string body, List<string> attachments = null)
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(_options.Username);
                    message.To.Add(sendTo);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    // Add attachments if provided
                    if (attachments != null && attachments.Count > 0)
                    {
                        foreach (var filePath in attachments)
                        {
                            if (File.Exists(filePath))
                            {
                                var attachment = new Attachment(filePath);
                                message.Attachments.Add(attachment);
                            }
                        }
                    }

                    using (var client = new SmtpClient(_options.SmtpHost, _options.SmtpPort))
                    {
                        client.Credentials = new NetworkCredential(_options.Username, _options.Password);
                        client.EnableSsl = true;
                        client.Send(message);
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                // Log exception
                return 0;
            }
        }
        public int SendMail2(string sendTo, string subject, string body, IFormFileCollection attachments = null)
        {
            const int maxFileSize = 5 * 1024 * 1024; // 5MB

            try
            {
                // Validate attachments first
                if (attachments != null)
                {
                    foreach (var file in attachments)
                    {
                        if (file.Length > maxFileSize)
                        {
                            return -2; // File size exceeded error code
                        }
                    }
                }
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(_options.Username);
                    message.To.Add(sendTo);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;
                    message.BodyEncoding = Encoding.UTF8;
                    message.SubjectEncoding = Encoding.UTF8;

                    // Process attachments
                    if (attachments != null && attachments.Count > 0)
                    {
                        foreach (var file in attachments)
                        {
                            var memoryStream = new MemoryStream();
                            file.CopyTo(memoryStream);
                            memoryStream.Position = 0; // Reset stream position

                            var attachment = new Attachment(memoryStream, file.FileName, file.ContentType);

                            attachment.ContentDisposition.FileName = Encoding.UTF8.GetString(Encoding.ASCII.GetBytes(file.FileName));

                            message.Attachments.Add(attachment);
                        }
                    }

                    using (var client = new SmtpClient(_options.SmtpHost, _options.SmtpPort))
                    {
                        client.Credentials = new NetworkCredential(_options.Username, _options.Password);
                        client.EnableSsl = true;
                        //var result = client.SendMailAsync(message).IsCompleted;
                        //if (result)
                        //{
                        //    return 1;
                        //}
                        client.Send(message);
                        return 1;
                    }
                }
                return 0; // Success
            }
            catch (Exception ex)
            {
                // Log exception
                string msg = ex.Message;
                return -1; // General error
            }
        }

        public int SendMail1(string sendTo, string subject, string body, IFormFileCollection attachments = null)
        {
            int result = 0;

            // Retrieve these values using a secure method. Here they're hard-coded for demonstration.
            // In production, retrieve these settings from a secure configuration store.
            string smtpHost = _options.SmtpHost;
            int smtpPort = 587;    // Use 587 for secure (STARTTLS) or 25 for unsecure
            bool enableSsl = false; // Set to "false" for an unsecure connection (although SSL is strongly recommended)
            string smtpUser = _options.Username;
            string smtpPass = _options.Password;
            string fromAddress = _options.Username;

            try
            {
                using (var message = new MailMessage())
                {
                    // Set sender and recipient addresses.
                    message.From = new MailAddress(fromAddress);
                    message.To.Add(new MailAddress(sendTo));

                    // Set subject and content.
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true; // Adjust if you need plain text

                    // Add attachments if provided.
                    if (attachments != null)
                    {
                        foreach (var file in attachments)
                        {
                            if (file != null && file.Length > 0)
                            {
                                // For best practices, you might want to validate file types and lengths.
                                var stream = file.OpenReadStream();
                                var attachment = new Attachment(stream, file.FileName);
                                message.Attachments.Add(attachment);
                            }
                        }
                    }

                    // Configure the SMTP client.
                    using (var smtpClient = new SmtpClient("mail.deograstiaschools.com.ng", 25))//587
                    {
                        smtpClient.EnableSsl = false;
                        smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);

                        // Optional: set delivery method and timeout.
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.Timeout = 180000; // Timeout in milliseconds (e.g., 3 minutes)

                        // Send the email synchronously.
                        smtpClient.Send(message);
                    }
                }

                result = 1;
            }
            catch (SmtpException smtpEx)
            {
                // Log the SMTP-specific error information (e.g., to your logging system).
                Console.WriteLine("SMTP Exception occurred: " + smtpEx.Message);
                result = 0;
            }
            catch (Exception ex)
            {
                // Log any other exceptions that might occur.
                Console.WriteLine("An unexpected error occurred while sending email: " + ex.Message);
                result = 0;
            }

            return result;
        }

    }
}
