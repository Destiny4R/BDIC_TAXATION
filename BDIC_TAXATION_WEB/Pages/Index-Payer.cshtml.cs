using BDIC_TAXATION_ACCESS.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BDIC_TAXATION_WEB.Pages
{
    [Authorize]
    public class Index_PayerModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;
        public bool completeprofile;
        public string Name;
        public Index_PayerModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public void OnGet()
        {
            //get the current login userid
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                var individual = unitOfWork.IndividualTaxpayer.FindSingle(q => q.ApplicationUserId == userId);
                if(individual != null)
                {
                    Name = individual.FirstName + " " + individual.OtherName + " " + individual.Surname;
                    if (individual.IsProfileComplete && individual.IsProfileComplete2)
                    {
                        completeprofile = true;
                    }
                }
            }
        }
    }
}
