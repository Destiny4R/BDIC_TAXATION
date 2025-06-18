using BDIC_TAXATION_ACCESS.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BDIC_TAXATION_WEB.Pages.Tax_Payer
{
    public class Request_ConsultantModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;

        public IEnumerable<SelectListItem> Consultant { get; set; }
        [Required]
        [Display(Name ="Consultant")]
        public int GetConsultant { get; set; }
        public Request_ConsultantModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public void OnGet()
        {
            Loader();
        }
        public IActionResult OnPost ()
        {
            Loader();
            return Page();
        }
        private void Loader()
        {
            Consultant = unitOfWork.ConsultantUsers.GetAllWithoutCondition().Select(q => new SelectListItem
            {
                Text = q.Name+$"({q.RC_Number})",
                Value = q.Id.ToString()
            });
        }
    }
}
