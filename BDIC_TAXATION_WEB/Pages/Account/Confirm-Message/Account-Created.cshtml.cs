using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_ACCESS.SystemRepos;
using BDIC_TAXATION_MODELS.Models;
using BDIC_TAXATION_WEB.HelperClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BDIC_TAXATION_WEB.Pages.Account.Confirm_Message
{
    public class Account_CreatedModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMailSender mailSender;
        private readonly HtmlTemplateService htmlTemplate;
        private readonly UserManager<ApplicationUser> userManager;

        public string Email { get; set; }
        public Account_CreatedModel(IUnitOfWork unitOfWork, IMailSender mailSender, HtmlTemplateService htmlTemplate, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.mailSender = mailSender;
            this.htmlTemplate = htmlTemplate;
            this.userManager = userManager;
        }
        public void OnGet(string email)
        {
            Email = email;
        }
        public IActionResult OnPost(string Email)
        {
            if (ModelState.IsValid)
            {
                var user = unitOfWork.IndividualTaxpayer.GetFirstOrDefaultAsync(u => u.Email == Email, q=>q.Include(q=>q.Applicationuser)).Result;
                if (user != null)
                {
                    var code = userManager.GenerateEmailConfirmationTokenAsync(user.Applicationuser).Result;
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page("/Account/Confirm-Message/Account-Verified", pageHandler: null, values: new { area = "Identity", userId = user.Applicationuser.Id, code = code, returnUrl = "" },
                        protocol: Request.Scheme);


                    var button = $"<a {callbackUrl} style=\"color: #ffffff; text-decoration: none; font-weight: bold; font-size: 16px; display: inline-block; background-color: #007bff; padding: 12px 25px; border-radius: 4px;\">Click here to verify</a>";

                    string[] strings1 = { user.FirstName, button, callbackUrl };
                    string processedHtml = htmlTemplate.ProcessHtmlTemplate(
                                    "htmlfiles/createaccount.html",strings1);
                    string subject = "Account Verification";
                    int outcome = mailSender.SendMail1(user.Email, subject.ToUpper(), processedHtml);
                    switch (outcome)
                    {
                        case 1:
                            TempData["success"] = "Account created successfully. Please check your email to verify your account";
                            TempData["success"] = "The verification message has been successfully resent. Please check your email to activate your account.";
                            return RedirectToPage( new {email=user.Email});
                        case 0:
                            TempData["error"] = "Account created successfully. Verification email could not be sent.";
                            return RedirectToPage("/Account/Login");
                        default:
                            TempData["error"] = "Account created successfully. Verification email could not be sent.";
                            return RedirectToPage("/Account/Login");
                    }
                }
                else
                {
                    return RedirectToPage("/Account/Register/Individual-Account");
                }
            }
            else
            {
                return Page();
            }
        }
    }
}
