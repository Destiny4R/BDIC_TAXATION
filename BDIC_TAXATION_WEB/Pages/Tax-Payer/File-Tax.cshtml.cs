using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_MODELS.Models;
using BDIC_TAXATION_MODELS.ViewModels;
using BDIC_TAXATION_UTILITIES;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BDIC_TAXATION_WEB.Pages.Tax_Payer
{
    public class File_TaxModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        [BindProperty]
        public FileReturnVM fileReturn { get; set; }
        public File_TaxModel(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost(FileReturnVM fileReturn)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Get the current user
                    var user = userManager.GetUserAsync(User).Result;
                    //Check if the user is an individual taxpayer
                    if (user == null)
                    {
                        TempData["error"] = "User not found.";
                        return Page();
                    }
                    //Check if the user is an individual taxpayer
                    var individualTaxPayer = unitOfWork.IndividualTaxpayer.FindSingle(x => x.ApplicationUserId == user.Id);
                    if (individualTaxPayer == null)
                    {
                        TempData["error"] = "Individual taxpayer not found.";
                        return Page();
                    }
                    //Check if the user has already filed a tax return for the current year
                    var existingFileReturn = unitOfWork.FileReturnIndividuals.FindSingle(x => x.IndividualId == individualTaxPayer.Id && x.SessionYear == fileReturn.SessionYear && x.AssetName == fileReturn.AssetName);
                    if(existingFileReturn != null)
                    {
                        TempData["error"] = "You have already filed a tax return for this asset in the selected session year.";
                        return Page();
                    }

                    var filereturn = new FileReturnIndividual()
                    {
                        IncomeAmount = fileReturn.IncomeAmount,
                        AssetName = fileReturn.AssetName,
                        SessionYear = fileReturn.SessionYear,
                        AssetTaxable = fileReturn.AssetTaxable,
                        IndividualId = individualTaxPayer.Id,
                        TaxAmount = (double)SD.CalculatePercent(7, (decimal)fileReturn.IncomeAmount),
                        Percent = 7,
                        ReferenceNo = SD.TrackNumber()
                    };
                    //Add the file return to the database
                    unitOfWork.FileReturnIndividuals.Add(filereturn);
                    int result = unitOfWork.SaveChanges();
                    //Check if the file return was added successfully
                    if (result > 0)
                    {
                        TempData["success"] = "Tax return filed successfully.";
                        return Page();
                    }
                    else
                    {
                        TempData["error"] = "Error filing tax return.";
                        return Page();
                    }
                }catch(Exception ex)
                {
                    TempData["error"] = "An error occurred while processing your request: " + ex.Message;
                    return Page();
                }
            }
            return Page();
        }
    }
}
