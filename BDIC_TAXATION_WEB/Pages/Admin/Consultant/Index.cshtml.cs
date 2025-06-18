using BDIC_TAXATION_ACCESS.Data;
using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_MODELS.Models;
using BDIC_TAXATION_MODELS.ViewModels;
using BDIC_TAXATION_UTILITIES;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BDIC_TAXATION_WEB.Pages.Admin.Consultant
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        [BindProperty]
        public ConsultantVM consultant { get; set; }
        public IEnumerable<SelectListItem> Consultant { get; set; }
        public IndexModel(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        public void OnGet()
        {
            GetConsult();
        }
        public async Task<IActionResult> OnPost()
        {
            GetConsult();
            if (ModelState.IsValid)
            {
                try
                {
                    if (consultant.Id > 0)
                    {
                        var consult = unitOfWork.ConsultantUsers.GetByIdSingle(consultant.Id);
                        if (consult != null)
                        {
                            consult.Address = consultant.Address;
                            consult.Email = consultant.Email;
                            consult.Name = consultant.Name;
                            consult.Phone = consultant.Phone;
                            consult.RC_Number = consultant.RC_Number;
                            consult.TaxIdentificationNumber = consultant.TaxIdentificationNumber;
                            consult.Others = consultant.Others;
                            consult.UpdatedDate = DateTime.UtcNow;
                            unitOfWork.ConsultantUsers.Update(consult);
                            int result = unitOfWork.SaveChanges();
                            if (result > 0)
                            {
                                TempData["success"] = "Consultant updated successfully";
                            }
                            else
                            {
                                TempData["error"] = "Error updating consultant";
                            }
                        }
                            return Page();
                    }
                    else
                    {
                        //Create a function for generating unique user name Starting: BIDIC-CONST-0001
                        var cons = await unitOfWork.ConsultantUsers.GetAllAsync();
                        string userName = "";
                        if (!cons.Any())
                        {
                            userName = "BDIC-CONST-100";
                        }
                        else
                        {
                            int addnumber = cons.Max(x => x.Id);
                            userName = $"BDIC-CONST-{(100 + addnumber)}";
                        }

                        //Create Login Account For the Consultant
                        var user = new ApplicationUser
                        {
                            UserName = userName,
                            Email = userName,
                            PhoneNumber = consultant.Phone,
                            EmailConfirmed = true,
                        };
                        var result = await userManager.CreateAsync(user, consultant.RC_Number.Trim());
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, SD.Role_Consultant);
                            var consultantUser = new ConsultantUser
                            {
                                Address = consultant.Address,
                                Email = consultant.Email,
                                Name = consultant.Name,
                                Phone = consultant.Phone,
                                RC_Number = consultant.RC_Number,
                                TaxIdentificationNumber = consultant.TaxIdentificationNumber,
                                Others = consultant.Others,
                                ApplicationUserId = user.Id,
                            };
                            unitOfWork.ConsultantUsers.Add(consultantUser);
                            int save = unitOfWork.SaveChanges();
                            if (save > 0)
                            {
                                TempData["success"] = "Consultant created successfully";
                            }
                            else
                            {
                                TempData["error"] = "Error creating consultant";
                            }
                            return Page();
                        }
                        else
                        {
                            TempData["error"] = "Error creating consultant";
                            return Page();
                        }

                    }
                }catch(Exception ex)
                {
                    TempData["error"] = $"Error: {ex.Message}";
                    return Page();
                }
            }
            return Page();
        }

        private  void GetConsult()
        {
            Consultant = unitOfWork.Specializations.GetAllWithoutCondition().Select(q=> new SelectListItem
            {
                Text = q.Name,
                Value = q.Id.ToString()
            });
        }
    }
}
