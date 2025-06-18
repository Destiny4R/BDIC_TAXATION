using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_MODELS.Models;
using BDIC_TAXATION_MODELS.ViewManager;
using BDIC_TAXATION_MODELS.ViewModels;
using BDIC_TAXATION_UTILITIES;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BDIC_TAXATION_WEB.Pages.Admin
{
    [Authorize]
    public class Tax_Offices_ManagementModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;

        public ViewSelectors ViewSelectors { get; set; }
        public TaxOfficesVM taxOffice { get; set; }
        public TaxOfficesVM taxOffice2 { get; set; }
        public Tax_Offices_ManagementModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public void OnGet()
        {
            _GetViewSelectors();
        }
        public IActionResult OnPostAddOffice(TaxOfficesVM taxOffice)
        {
            _GetViewSelectors();
            if (ModelState.IsValid)
            {
                //get the current signin userid
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //Get the total number of tax offices in the database
                int taxoff = 0;
                var taxOff2 = unitOfWork.TaxOffices.GetAllWithoutCondition();
                if (taxOff2.Any())
                {
                    taxoff = taxOff2.Max(q => q.Id);
                }
                else
                {
                    taxoff = 0;
                }
                    var thetaxOffice = new TaxOffices()
                    {
                        Name = taxOffice.Name,
                        Phone = taxOffice.Phone,
                        Address = taxOffice.Address,
                        Email = taxOffice.Email,
                        LgaId = taxOffice.LgaId,
                        OfficerInCharge = taxOffice.OfficerInCharge,
                        Code = "ATO" + SD.GenerateCodeFromId(taxoff, "ATO", 5),
                        ApplicationUserId = userId
                    };
                unitOfWork.TaxOffices.Add(thetaxOffice);
                int result = unitOfWork.SaveChanges();
                if (result > 0)
                {
                    TempData["success"] = "Tax office added successfully.";
                }
                else
                {
                    TempData["error"] = "Error adding tax office.";
                }
                return Page();
            }
            TempData["error"] = "Some Missing Tax office Information.";
            return Page();
        }
        public IActionResult OnPostUpdate(TaxOfficesVM taxOffice2)
        {
            _GetViewSelectors();
            if (ModelState.IsValid)
            {
                var taxOff = unitOfWork.TaxOffices.FindSingle(q => q.Id == taxOffice2.Id);
                if (taxOff == null)
                {
                    TempData["error"] = "Unknown Tax office Information.";
                    return Page();
                }

                taxOff.Name = taxOffice2.Name;
                taxOff.Phone = taxOffice2.Phone;
                taxOff.Address = taxOffice2.Address;
                taxOff.Email = taxOffice2.Email;
                taxOff.LgaId = taxOffice2.LgaId;
                taxOff.OfficerInCharge = taxOffice2.OfficerInCharge;
                
                unitOfWork.TaxOffices.Update(taxOff);
                int result = unitOfWork.SaveChanges();
                if (result > 0)
                {
                    TempData["success"] = "Tax office successfully Updates.";
                }
                else
                {
                    TempData["error"] = "Error updating tax office.";
                }
                return Page();
            }
            TempData["error"] = "Some Missing Tax office Information.";
            return Page();
        }

        void _GetViewSelectors()
        {
            ViewSelectors = new ViewSelectors
            {
                Localgovernments = unitOfWork.LocalGovernments.GetAllWithoutCondition().Select(q => new SelectListItem
                {
                    Text = q.Name,
                    Value = q.Id.ToString()
                })
            };
        }
    }
}
