using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_MODELS.DTOs;
using BDIC_TAXATION_MODELS.Models;
using BDIC_TAXATION_UTILITIES;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BDIC_TAXATION_WEB.Pages.Admin.PayeeSetting
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
        public IActionResult OnPostAdd(PayeeSettingvm payeeget)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Generate unique code for specialization
                    string code = "";
                    var specials = unitOfWork.PayeeCalculators.GetAllWithoutCondition().MaxBy(k => k.Id);
                    if (specials == null)
                    {
                        code = SD.GenerateCodeFromId(0, "PE-", 7);
                    }
                    else
                    {
                        code = SD.GenerateCodeFromId(specials.Id, "PE-", 7);
                    }

                    var Payeeseter = new PayeeCalculator
                    {
                        HighestAmount = payeeget.high,
                        LowestAmount = payeeget.low,
                        Percent = payeeget.percent,
                        Code = "PE-"+code
                    };
                    unitOfWork.PayeeCalculators.Add(Payeeseter);
                    int result = unitOfWork.SaveChanges();
                    if (result > 0)
                    {
                        TempData["success"] = "Payee Setting added successfully!";
                        return RedirectToPage("Index");
                    }
                }
                catch (Exception ex)
                {
                    TempData["error"] = "An error occurred while adding the Payee Setting" + ex.Message;
                    return RedirectToPage("Index");
                }
            }
            TempData["error"] = "Please provide Payee required information!";
            return RedirectToPage("Index");
        }
        public IActionResult OnPostUpdate(PayeeSettingvm payee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (payee.id > 0)
                    {
                        var Payeeset = unitOfWork.PayeeCalculators.FindSingle(x => x.Id == payee.id);
                        Payeeset.HighestAmount = payee.high;
                        Payeeset.LowestAmount = payee.low;
                        Payeeset.Percent = payee.percent;

                        unitOfWork.PayeeCalculators.Update(Payeeset);
                        int result = unitOfWork.SaveChanges();
                        if (result > 0)
                        {
                            TempData["success"] = "Specialization updated successfully!";
                            return RedirectToPage("Index");
                        }
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
        public class PayeeSettingvm
        {
            public int? id { get; set; }
            public double high { get; set; }
            public double low { get; set; }
            public string? code { get; set; }
            public double percent { get; set; }
        } 
    }
}
