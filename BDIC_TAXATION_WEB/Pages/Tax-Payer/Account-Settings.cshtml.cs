using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_MODELS.Models;
using BDIC_TAXATION_MODELS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BDIC_TAXATION_WEB.Pages.Tax_Payer
{
    [Authorize]
    public class Account_SettingsModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public ChangepasswordVM Change { get; set; }    
        public IndividualProfileVM individual { get; set; }
        public IndividualBusinessVM individualbusiness { get; set; }
        public Account_SettingsModel(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public void OnGet()
        {
            //GET CURRENT SIGNIN USER
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(!string.IsNullOrEmpty(userId))
            {
                var getindividualInfo = unitOfWork.IndividualTaxpayer.FindSingle(x => x.ApplicationUserId == userId, includes:q=>q.Include(q=>q.Applicationuser));
                if (getindividualInfo != null)
                {
                    individual = new IndividualProfileVM
                    {
                        Id = getindividualInfo.Id,
                        FirstName = getindividualInfo.FirstName,
                        OtherName = getindividualInfo.OtherName,
                        Surname = getindividualInfo.Surname,
                        Phone = getindividualInfo.Phone,
                        TaxNumber = getindividualInfo.TIN,
                        LGA = getindividualInfo.LGA,
                        Gender = getindividualInfo.Gender,
                        NIN = getindividualInfo.NationalIdentificationNumber,
                        DateofBirth = getindividualInfo.DateofBirth.ToString("yyyy-MM-dd"),
                        Address = getindividualInfo.ResidentialAddress,
                        Occupation = getindividualInfo.Occupation,
                        Email = getindividualInfo.Applicationuser.Email,
                        
                    };
                    individualbusiness = new IndividualBusinessVM
                    {
                        Id = getindividualInfo.Id,
                        BusinessName = getindividualInfo.BusinessName,
                        BusinessType = getindividualInfo.BusinessType,
                        IndustrySector = getindividualInfo.IndustrySector,
                        Address = getindividualInfo.ResidentialAddress
                    };
                }
            }
        }
        public IActionResult OnPostBusinessProfile(IndividualProfileVM individual)
        {
            if (ModelState.IsValid)
            {
                var getindividualInfo = unitOfWork.IndividualTaxpayer.FindSingle(x => x.Id == individual.Id);
                if (getindividualInfo != null)
                {
                    getindividualInfo.FirstName = individual.FirstName;
                    getindividualInfo.OtherName = individual.OtherName;
                    getindividualInfo.Surname = individual.Surname;
                    getindividualInfo.Phone = individual.Phone;
                    getindividualInfo.TIN = individual.TaxNumber;
                    getindividualInfo.LGA = individual.LGA;
                    getindividualInfo.Gender = individual.Gender;
                    getindividualInfo.NationalIdentificationNumber = individual.NIN;
                    getindividualInfo.DateofBirth = Convert.ToDateTime(individual.DateofBirth);
                    getindividualInfo.ResidentialAddress = individual.Address;
                    getindividualInfo.Occupation = individual.Occupation;
                    getindividualInfo.UpdatedDate = DateTime.UtcNow;
                    getindividualInfo.IsProfileComplete = true;
                    unitOfWork.IndividualTaxpayer.Update(getindividualInfo);
                    var result = unitOfWork.SaveChanges();
                    if (result > 0)
                    {
                        TempData["success"] = "Individual Profile Updated Successfully";
                        return RedirectToPage();
                    }

                }

            }
            TempData["error"] = "Error Updating Individual Profile, provide all fields";
            return RedirectToPage();
        }
        public IActionResult OnPostBusinessInfo(IndividualBusinessVM individualbusiness)
        {
            if (ModelState.IsValid)
            {
                var getindividualInfo = unitOfWork.IndividualTaxpayer.FindSingle(x => x.Id == individualbusiness.Id);
                if (getindividualInfo != null)
                {
                    getindividualInfo.BusinessName = individualbusiness.BusinessName;
                    getindividualInfo.BusinessType = individualbusiness.BusinessType;
                    getindividualInfo.IndustrySector = individualbusiness.IndustrySector;
                    getindividualInfo.ResidentialAddress = individualbusiness.Address;
                    getindividualInfo.UpdatedDate = DateTime.UtcNow;
                    getindividualInfo.IsProfileComplete2 = true;
                    unitOfWork.IndividualTaxpayer.Update(getindividualInfo);
                    var result = unitOfWork.SaveChanges();
                    if (result > 0)
                    {
                        TempData["success"] = "Business Profile Updated Successfully";
                        return RedirectToPage();
                    }
                }
            }
            TempData["error"] = "Error Updating Business Profile, provide all fields";
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostChangePassword(ChangepasswordVM Change)
        {
            if (ModelState.IsValid)
            {
                //GET CURRENT SIGNIN USER
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    var user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var changeUserPassword = await userManager.ChangePasswordAsync(user, Change.OldPassword, Change.NewPassword);
                        if (changeUserPassword.Succeeded)
                        {
                            await signInManager.RefreshSignInAsync(user);
                            TempData["success"] = "Password Successfully Changed";
                            return RedirectToPage();
                        }
                        //return Page();
                    }
                }
            }
            return RedirectToPage();
        }
    }
}
