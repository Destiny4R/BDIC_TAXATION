using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_MODELS.Models;
using BDIC_TAXATION_UTILITIES;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BDIC_TAXATION_WEB.Pages.Admin.Specialisation
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;
        public IndexModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPostAdd(string specialname) 
        {
            if(ModelState.IsValid)
            {
                // Inject IUnitOfWork and use it to add the specialization
                //refactor this code and add it in try catch block
                // and also add the code to check if the specialization already exists
                //Get current user id
                //Write a function for generating 7 digit unique code for specialization Code property


                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                try
                {
                    var special = unitOfWork.Specializations.FindSingle(x => x.Name == specialname);
                    if (special != null)
                    {
                        TempData["error"] = "Specialization already exists!";
                        return RedirectToPage("Index");
                    }
                    //Generate unique code for specialization
                    string code = "";
                    var specials = unitOfWork.Specializations.GetAllWithoutCondition().MaxBy(k=>k.Id);
                    if (specials == null)
                    {
                        code = SD.GenerateCodeFromId(0, "SC-", 7);
                    }
                    else
                    {
                        code = SD.GenerateCodeFromId(specials.Id, "SC-", 7);
                    }

                    var specialization = new Specialization
                    {
                        Name = specialname,
                        ApplicationUserId = userId,
                        Code = code
                    };
                    unitOfWork.Specializations.Add(specialization);
                    int result = unitOfWork.SaveChanges();
                    if (result > 0)
                    {
                        TempData["success"] = "Specialization added successfully!";
                        return RedirectToPage("Index");
                    }
                }catch(Exception ex)
                {
                    TempData["error"] = "An error occurred while adding the specialization"+ex.Message;
                    return RedirectToPage("Index");
                }
            }
            TempData["error"] = "Please Provide Specialization!";
            return RedirectToPage("Index");
        }
        public IActionResult OnPostUpdate(string specialudate, int specialId)
        {
            if (ModelState.IsValid)
            {
                try 
                {
                    var special = unitOfWork.Specializations.FindSingle(x => x.Id == specialId);
                    special.Name = specialudate;
                    unitOfWork.Specializations.Update(special);
                    int result = unitOfWork.SaveChanges();
                    if (result > 0)
                    {
                        TempData["success"] = "Specialization updated successfully!";
                        return RedirectToPage("Index");
                    }
                }
                catch (Exception ex)
                {
                    TempData["error"] = "An error occurred while updating the specialization";
                    return RedirectToPage("Index");
                }
            }
            TempData["error"] = "Please Provide Specialization!";
            return RedirectToPage("Index");
        }
    }
}
