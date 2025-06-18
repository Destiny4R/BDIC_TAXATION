using BDIC_TAXATION_MODELS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace BDIC_TAXATION_WEB.Pages.Account.Confirm_Message
{
    public class Account_VerifiedModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;

        public bool GoodResult = false;
        public Account_VerifiedModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public void OnGet(string userId, byte[] code)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
                if (user != null)
                {
                    var code2 =  Encoding.UTF8.GetString(code);

                    var result = userManager.ConfirmEmailAsync(user, code2).GetAwaiter().GetResult();
                    if (result.Succeeded)
                    {
                        GoodResult = true;
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
                
            }
        }
    }
}
