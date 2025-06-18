using BDIC_TAXATION_MODELS.Models;
using BDIC_TAXATION_MODELS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace BDIC_TAXATION_WEB.Pages.Account
{
    [Authorize]
    public class Account_SettingsModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public ChangepasswordVM changepassword {  get; set; }
        public Account_SettingsModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostChangePassword(ChangepasswordVM changepassword)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.GetUserAsync(User).Result;
                if (user != null)
                {
                    var result = userManager.ChangePasswordAsync(user, changepassword.OldPassword, changepassword.NewPassword).Result;
                    if (result.Succeeded)
                    {
                        await signInManager.RefreshSignInAsync(user);
                        TempData["success"] = "Password successfully changed!";
                        return Page();
                    }
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return Page();
                }
            }
            return Page();
        }
    }
}
