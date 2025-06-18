using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_WEB.HelperClasses
{
    public class HtmlTemplateService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HtmlTemplateService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string ProcessHtmlTemplate(string templatePath, params object[] values)
        {
            // Get the full path to the file in wwwroot
            string fullPath = Path.Combine(_hostingEnvironment.WebRootPath, templatePath);

            // Read the template file
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"Template file not found at {fullPath}");
            }

            string htmlContent = File.ReadAllText(fullPath);

            // Replace placeholders with provided values
            return string.Format(htmlContent, values);
        }

        public string ProcessHtmlTemplate(string templatePath, string[] strings)
        {
            // Get the full path to the file in wwwroot
            string fullPath = Path.Combine(_hostingEnvironment.WebRootPath, templatePath);

            // Read the template file
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"Template file not found at {fullPath}");
            }

            string htmlContent = File.ReadAllText(fullPath);

            string curyOp = "{";
            string curyCl = "}";
            int replace = 0;
            // Replace each placeholder with its corresponding value
            foreach (var value in strings)
            {
                htmlContent = htmlContent.Replace(curyOp+$"{replace}"+curyCl, value);
                replace++;
            }

            return htmlContent;
        }
    }
}
