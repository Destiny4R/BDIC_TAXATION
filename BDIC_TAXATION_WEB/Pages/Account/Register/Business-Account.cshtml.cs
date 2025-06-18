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
    public class Business_AccountModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMailSender mailSender;
        private readonly HtmlTemplateService htmlTemplate;
        private readonly ILogger<Business_AccountModel> logger;
        private readonly RoleManager<IdentityRole> roleManager;

        [BindProperty]
        public BusinessTaxPayerVM BusinessTax {  get; set; }
        public Business_AccountModel(UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork,
            IMailSender mailSender,
            HtmlTemplateService htmlTemplate,
            ILogger<Business_AccountModel> logger,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.mailSender = mailSender;
            this.htmlTemplate = htmlTemplate;
            this.logger = logger;
            this.roleManager = roleManager;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //FIRST THINGS FIRST, CREATE A LOGIN ACCOUNT FOR THE INCOMING USER BEFORE SAVING THEIR INFORMATION
                    var user = new ApplicationUser
                    {
                        UserName = BusinessTax.Email,
                        Email = BusinessTax.Email,
                        PhoneNumber = BusinessTax.PhoneNumber
                    };
                    var createuser = await userManager.CreateAsync(user, BusinessTax.ConfirmPassword);
                    if (createuser.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, SD.Role_Business);

                        string[] strings = BusinessTax.LogData.Split('|');
                        var business = new BusinessTaxpayer
                        {
                            BusinessName = BusinessTax.BusinessName,
                            RegistrationNumber = BusinessTax.RegistrationNumber,
                            BusinessType = BusinessTax.BusinessType,
                            IndustrySector = BusinessTax.IndustrySector,
                            LGA = BusinessTax.LGA,
                            Address = BusinessTax.Address,
                            ApplicationUserId = user.Id,
                            Latitude = strings[0],
                            Longitude = strings[1],
                            IpAddress = strings[4],
                            Others = strings[3] + "|" + strings[2] + "|" + strings[5],
                            Email = BusinessTax.Email,
                            PhoneNumber = BusinessTax.PhoneNumber
                        };
                        unitOfWork.BusinessTaxPayer.Add(business);
                        int result = unitOfWork.SaveChanges();
                        if (result > 0)
                        {
                            //SEND EMAIL TO THE USER
                            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                            var callbackUrl = Url.Page("/Account/Confirm-Message/Account-Verified", pageHandler: null, values: new { area = "Identity", userId = user.Id, code = code, returnUrl = "" },
                                protocol: Request.Scheme);


                            var button = $"<a {callbackUrl} style=\"color: #ffffff; text-decoration: none; font-weight: bold; font-size: 16px; display: inline-block; background-color: #007bff; padding: 12px 25px; border-radius: 4px;\">Click here to verify</a>";

                            string[] strings1 = { business.BusinessName, button, callbackUrl };
                            string processedHtml = htmlTemplate.ProcessHtmlTemplate(
                                            "htmlfiles/createaccount.html", strings1);
                            string subject = "Account Verification";
                            int outcome = mailSender.SendMail1(business.Email, subject.ToUpper(), processedHtml);
                            switch (outcome)
                            {
                                case 1:
                                    TempData["success"] = "Account created successfully. Please check your email to verify your account";
                                    return RedirectToPage("/Account/Confirm-Message/Account-Created", new { email = user.Email });
                                case 0:
                                    TempData["error"] = "Account created successfully. Verification email could not be sent.";
                                    return RedirectToPage("/Account/Login");
                                default:
                                    TempData["error"] = "Account created successfully. Verification email could not be sent.";
                                    return RedirectToPage("/Account/Login");
                            }
                        }
                    }
                }catch(Exception ex)
                {
                    logger.Log(LogLevel.Error, ex.Message);
                }
            }
            return Page();
        }
    }
}
