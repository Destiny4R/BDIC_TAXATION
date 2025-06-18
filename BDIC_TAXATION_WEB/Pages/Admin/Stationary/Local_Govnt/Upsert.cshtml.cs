using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_MODELS.Models;
using BDIC_TAXATION_MODELS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BDIC_TAXATION_WEB.Pages.Admin.Stationary.Local_Govnt
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;

        [BindProperty]
        public Government  government { get; set; }
        public UpsertModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task OnGet(int id)
        {
            if (id > 0)
            {
                var govern = await unitOfWork.LocalGovernments.GetAllAsync(x => x.Id == id);                
                if (govern != null)
                {
                    government = govern.Select(q => new Government { Id = q.Id, Name = q.Name }).FirstOrDefault();
                }
            }
        }
        public IActionResult OnPost() 
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (government.Id > 0)
                    {
                        var gov = unitOfWork.LocalGovernments.GetByIdSingle((int)government.Id);
                        if (gov != null)
                        {
                            gov.Name = government.Name;
                            unitOfWork.LocalGovernments.Update(gov);
                            int result = unitOfWork.SaveChanges();
                            if (result > 0)
                            {
                                TempData["success"] = "Local Government updated successfully";
                                return RedirectToPage("Index");
                            }
                        }
                    }
                    else
                    {
                        var existingGov = unitOfWork.LocalGovernments.GetAllWithoutCondition();
                        var _findamatch = existingGov.FirstOrDefault(x => x.Name == government.Name);
                        if (_findamatch != null)
                        {
                            TempData["error"] = "Local Government already exists";
                            return Page();
                        }
                        int code = 0;
                        if (existingGov.Count()>0)
                        {
                            var _getMax = existingGov.Max(x => x.LGACode);
                            code = _getMax + 1;
                        }
                        else
                        {
                            code = 100;
                        }
                        var newGov = new LocalGovernment
                        {
                            Name = government.Name,
                            LGACode = code
                        };
                        unitOfWork.LocalGovernments.Add(newGov);
                        int result = unitOfWork.SaveChanges();
                        if (result > 0)
                        {
                            TempData["success"] = "Local Government created successfully";
                            return RedirectToPage("Index");
                        }
                    }
                }
                catch(Exception ex)
                {
                    TempData["error"] = $"Error: {ex.Message}";
                    return Page();
                }
            }
            TempData["error"] = $"Provide the local government name";
            return Page();
        }
    }
}
