using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_MODELS.Models;
using BDIC_TAXATION_MODELS.ViewManager;
using BDIC_TAXATION_MODELS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BDIC_TAXATION_WEB.Pages.Admin.Stationary.Local_Station
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;

        [BindProperty]
        public Stations stationa { get; set; }
        public ViewSelectors ViewSelectors { get; set; }
        public UpsertModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task OnGet(int id)
        {
            _GetViewSelectors();
            if (id > 0)
            {
                var govern = await unitOfWork.LGAStations.GetAllAsync(x => x.Id == id);
                if (govern != null)
                {
                    stationa = govern.Select(q => new Stations { Id = q.Id, Name = q.Name, LGId = q.LGId }).FirstOrDefault();
                }
            }
        }
        public IActionResult OnPost()
        {
            _GetViewSelectors();
            if (ModelState.IsValid)
            {
                try
                {
                    if (stationa.Id > 0)
                    {
                        var Sta = unitOfWork.LGAStations.GetByIdSingle((int)stationa.Id);
                        if (Sta != null)
                        {
                            Sta.Name = stationa.Name;
                            Sta.LGId = stationa.LGId;
                            unitOfWork.LGAStations.Update(Sta);
                            int result = unitOfWork.SaveChanges();
                            if (result > 0)
                            {
                                TempData["success"] = "Local Government Station updated successfully";
                                return RedirectToPage("Index");
                            }
                        }
                    }
                    else
                    {
                        var existingGov = unitOfWork.LGAStations.GetAllWithoutCondition();
                        var _findamatch = existingGov.FirstOrDefault(x => x.Name == stationa.Name && x.LGId==stationa.LGId);
                        if (_findamatch != null)
                        {
                            TempData["error"] = "Local Station Government already exists";
                            return Page();
                        }
                        int code = 0;
                        if (existingGov.Count() > 0)
                        {
                            var _getMax = existingGov.Max(x => x.StationCode);
                            code = _getMax + 1;
                        }
                        else
                        {
                            code = 100;
                        }
                        var newGov = new LGAStations
                        {
                            Name = stationa.Name,
                            StationCode = code,
                            LGId = stationa.LGId
                        };
                        unitOfWork.LGAStations.Add(newGov);
                        int result = unitOfWork.SaveChanges();
                        if (result > 0)
                        {
                            TempData["success"] = "Local Government Station created successfully";
                            return RedirectToPage("Index");
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"Error: {ex.Message}";
                    return Page();
                }
            }
            TempData["error"] = $"Provide the local government name";
            return Page();
        }
        void _GetViewSelectors()
        {
            ViewSelectors = new ViewSelectors
            {
                Localgovernments = unitOfWork.LocalGovernments.GetAllWithoutCondition().Select(q => new SelectListItem
                {
                    Text = q.Name, Value = q.Id.ToString()
                })
            };
        }
    }
}
