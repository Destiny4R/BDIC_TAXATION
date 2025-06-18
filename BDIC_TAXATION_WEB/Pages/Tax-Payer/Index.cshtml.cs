using BDIC_TAXATION_ACCESS.Repository;
using BDIC_TAXATION_ACCESS.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BDIC_TAXATION_WEB.Pages.Tax_Payer
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;
        public bool check { get; set; } = false;
        public IndexModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public void OnGet()
        {//GET CURRENT SIGNIN USER
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                var getindividualInfo = unitOfWork.IndividualTaxpayer.FindSingle(x => x.ApplicationUserId == userId, includes: q => q.Include(q => q.Applicationuser));
                if (getindividualInfo != null)
                {
                    if(getindividualInfo.IsProfileComplete && getindividualInfo.IsProfileComplete2)
                    {
                        check = true;
                    }
                }
            }
        }
    }
}
