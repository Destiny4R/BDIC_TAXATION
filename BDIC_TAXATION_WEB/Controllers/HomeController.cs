using BDIC_TAXATION_MODELS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BDIC_TAXATION_WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;

        public HomeController(SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
        }
        public IActionResult Signout()
        {
            if (signInManager.IsSignedIn(User))
            {
                signInManager.SignOutAsync().GetAwaiter().GetResult();
                return RedirectToPage("/Account/Login");
            }
            return RedirectToPage("/Index");
        }
    }
}
