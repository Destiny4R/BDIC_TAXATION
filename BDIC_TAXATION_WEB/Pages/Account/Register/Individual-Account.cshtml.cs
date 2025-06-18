using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_ACCESS.SystemRepos;
using BDIC_TAXATION_MODELS.Models;
using BDIC_TAXATION_MODELS.ViewModels;
using BDIC_TAXATION_UTILITIES;
using BDIC_TAXATION_WEB.HelperClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace BDIC_TAXATION_WEB.Pages.Account.Register
{
    public class Individual_AccountModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly HtmlTemplateService htmlTemplate;
        private readonly IMailSender mailSender;
        public readonly UserManager<ApplicationUser> userManager;

        [BindProperty]
        public IndividualVM individual { get; set; }
        public Individual_AccountModel(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, HtmlTemplateService htmlTemplate, IMailSender mailSender)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.htmlTemplate = htmlTemplate;
            this.mailSender = mailSender;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var getIndividual = unitOfWork.IndividualTaxpayer.GetAllWithoutCondition().Count();
                string TaxId = "";
                if (getIndividual > 0) {
                    TaxId = SD.GenerateCodeFromId(getIndividual, "BN-", 8);
                }
                else
                {
                    TaxId = SD.GenerateCodeFromId(0, "BN-", 8);
                }
                    var user = new ApplicationUser
                    {
                        UserName = TaxId,
                        Email = individual.Email,
                        PhoneNumber = individual.Phone,
                    };
                var createUser = await userManager.CreateAsync(user, individual.Password);
                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, SD.Role_Individual);
                    //string[] strings = individual.Logdata.Split('|');
                    var individualacc = new IndividualTaxpayer
                    {
                        ApplicationUserId = user.Id,
                        FirstName = individual.FirstName,
                        OtherName = individual.OtherName,
                        Surname = individual.Surname,
                        Email = individual.Email,
                        Phone = individual.Phone,
                        NationalIdentificationNumber = individual.NationalIdentificationNumber,
                        Latitude = "null",//strings[0],
                        Longitude = "null",//strings[1],
                        IpAddress = "null",//strings[4],
                        Others = "null",//strings[3]+"|"+ strings[2] + "|" + strings[5]
                    };
                    //lat + '|' + lng + '|' + browserInfo + '|' + osInfo + '|' + ip + '|' + userAgent;
                    unitOfWork.IndividualTaxpayer.Add(individualacc);
                    int result = unitOfWork.SaveChanges();
                    if (result > 0)
                    {
                        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page("/Account/Confirm-Message/Account-Verified", pageHandler: null, values: new { area = "Identity", userId = user.Id, code = code, returnUrl = "" },
                            protocol: Request.Scheme);
                        Writetofile(callbackUrl);

                        //var button = $"<a {callbackUrl} style=\"color: #ffffff; text-decoration: none; font-weight: bold; font-size: 16px; display: inline-block; background-color: #007bff; padding: 12px 25px; border-radius: 4px;\">Click here to verify</a>";

                        //string[] strings1 = { individualacc.FirstName, button, callbackUrl };
                        //string processedHtml = htmlTemplate.ProcessHtmlTemplate("htmlfiles/createaccount.html", strings1);
                        string subject = "Account Verification";
                       //int outcome = mailSender.SendMail1(individualacc.Email, subject.ToUpper(), processedHtml);
                        //switch(outcome)
                        //{
                            //case 1:
                            //    TempData["success"] = "Account created successfully. Please check your email to verify your account";
                            //    return RedirectToPage("/Account/Confirm-Message/Account-Created", new { email = user.Email });
                            //case 0:
                            //    TempData["error"] = "Account created successfully. Verification email could not be sent.";
                            //    return RedirectToPage("/Account/Login");
                            //default:
                            //    TempData["error"] = "Account created successfully. Verification email could not be sent.";
                            //    return RedirectToPage("/Account/Login");
                        //}
                        return RedirectToPage("/Account/Confirm-Message/Account-Created");
                    }
                }
            }
            return Page();
        }
        public void Writetofile(string content)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","log", "message.txt");
            // Write the text to the file (overwrite if it exists)
            System.IO.File.WriteAllText(filePath, content);
        }
    }
}
