using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_MODELS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BDIC_TAXATION_WEB.Controllers
{
    public class special_actionController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public special_actionController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        public async Task<IActionResult> UnlockUser(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user != null)
            {
                // Check if the user is locked out
                if (!await userManager.IsLockedOutAsync(user))
                {
                    await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                    return Json(new { success = true, message = "Consultant account successfully lock" });

                }
                else
                {
                    await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                    return Json(new { success = true, message = "Consultant account successfully unlock" });
                }
            }
            else
            {

                return Json(new { success=false, message = "We couldn't identify this user account" });
            }
        }
        [HttpPost]
        public IActionResult Index()
        {
            return View();
        }
    }
}
