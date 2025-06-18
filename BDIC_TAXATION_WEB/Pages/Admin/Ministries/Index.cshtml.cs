using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_MODELS.Models;
using BDIC_TAXATION_MODELS.ViewModels;
using BDIC_TAXATION_UTILITIES;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BDIC_TAXATION_WEB.Pages.Admin.Ministries
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        [BindProperty]
        public MDAsMinistryVM mdasministry { get; set; }
        public IndexModel(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        public async Task OnGet(int id)
        {
            //Get MDAsMinistry Data and past it to MDAsMinistryVM data
            if (id > 0)
            {
                var mdasministry1 = await unitOfWork.MDAsMinistry.GetAllAsync(predicate: q => q.Id == id, includes: q => q.Include(q => q.Applicationuser));
                if (mdasministry == null)
                {
                    // Handle not found case, e.g., redirect or show error message
                    TempData["Error"] = "MDAs Ministry not found.";
                    RedirectToPage("Index");
                }
                mdasministry = mdasministry1.Select(h => new MDAsMinistryVM
                {
                    id = h.Id,
                    name = h.Name,
                    code = h.Code,
                    description = h.Description,
                    email = h.Applicationuser.Email,
                    status = h.Applicationuser.LockoutEnabled,
                    createdate = h.CreatedDate
                }).FirstOrDefault();
            }
            else
            {
                mdasministry = new MDAsMinistryVM();
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            //validate the model state and create ApplicationUser account for the MDAsMinistry and add the MDAsMinistry to the database
            if (ModelState.IsValid)
            {
                var mdas = await unitOfWork.MDAsMinistry.GetAllAsync();
                string codevalue = "";
                if (mdas.Any())
                {
                    var id = mdas.MaxBy(q=>q.Id).Id;
                    codevalue = SD.GenerateCodeFromId(id, "MDA-TRN-", 5);
                }
                else
                    codevalue = SD.GenerateCodeFromId(0, "MDA-TRN-", 5);
                //Create ApplicationUser account for the MDAsMinistry using usermanager service api
                var applicationUser = new ApplicationUser
                {
                    UserName = codevalue,
                    Email = mdasministry.email,
                    //EmailConfirmed = true,
                    LockoutEnabled = false
                };
                var result = await userManager.CreateAsync(applicationUser, "Password123!"); // Use a secure password in production
                if (result.Succeeded)
                {
                    //Add MDAsMinistry to role using rolemanager service api
                    if (!await userManager.IsInRoleAsync(applicationUser, SD.Role_MDAs))
                    {
                        await userManager.AddToRoleAsync(applicationUser, SD.Role_MDAs);
                    }
                    //Create MDAsMinistry object and add it to the database
                    var mdasministry1 = new MDAsMinistry
                    {
                        Name = mdasministry.name,
                        Code = codevalue,
                        Description = mdasministry.description,
                        ApplicationUserId = applicationUser.Id
                    };
                    unitOfWork.MDAsMinistry.Add(mdasministry1);
                    await unitOfWork.SaveAsync();
                    TempData["Success"] = "MDAs Ministry created successfully.";
                    return RedirectToPage("Index");
                }
                else
                {
                    // Handle errors from user creation
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        //Create MDAsMinistry object and add it to the database
                        var mdasministry1 = new MDAsMinistry
                        {
                            Name = mdasministry.name,
                            Code = mdasministry.code,
                            Description = mdasministry.description,
                            ApplicationUserId = applicationUser.Id
                        };
                        if (mdasministry.id > 0)
                        {
                            //Add new MDAsMinistry
                            unitOfWork.MDAsMinistry.Add(mdasministry1);
                        }
                        await unitOfWork.SaveAsync();
                        TempData["Success"] = "MDAs Ministry saved successfully.";
                        return RedirectToPage("Index");
                    }
                    return Page();
                }
            }
            return Page();
        }
    }
}
