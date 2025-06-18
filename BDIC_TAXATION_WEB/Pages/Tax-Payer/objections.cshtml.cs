using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_ACCESS.SystemRepos;
using BDIC_TAXATION_MODELS.Models;
using BDIC_TAXATION_UTILITIES;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BDIC_TAXATION_WEB.Pages.Tax_Payer
{
    //Add Authorize attribute to restrict access to authenticated users
    [Authorize(Roles = SD.Role_Individual)]
    public class objectionsModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileLoader fileLoader;

        public int theid { get; set; }
        public ObjectionStates objectionStates { get; set; } = new ObjectionStates
        {
            Review = 0,
            Progress = 0,
            Resolved = 0,
            Closed = 0,
            Total = 0
        };
        public objectionsModel(IUnitOfWork unitOfWork, IFileLoader fileLoader)
        {
            this.unitOfWork = unitOfWork;
            this.fileLoader = fileLoader;
        }
        public async Task OnGet(int id)
        {
            if (id > 0)
            {
                theid = id;
            }
            //Get the current logged-in userid
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //check if the userId is null or empty
            if (!string.IsNullOrEmpty(userId))
            {
                var objpending = await unitOfWork.ObjectionsTables.GetAllAsync(o => o.FileReturnIndividual.IndividualTaxpayer.ApplicationUserId == userId, includes: k=>k.Include(q=>q.FileReturnIndividual.IndividualTaxpayer.Applicationuser));
                objectionStates.Review = objpending.Where(o => o.ProcessStatus == ((int)SD.ObjectionStatus.Review).ToString()).Count();
                objectionStates.Progress = objpending.Where(o => o.ProcessStatus == ((int)SD.ObjectionStatus.Progress).ToString()).Count();
                objectionStates.Resolved = objpending.Where(o => o.ProcessStatus == ((int)SD.ObjectionStatus.Resolved).ToString()).Count();
                objectionStates.Closed = objpending.Where(o => o.ProcessStatus == ((int)SD.ObjectionStatus.Closed).ToString()).Count();
                objectionStates.Total = objpending.Count();
            }
        }
        public async Task<IActionResult> OnPost(IFormFile? uploadedFile, int incomingId,  double amount, string message, string document_type, string reason)
        {
            if (ModelState.IsValid)
            {
                if (uploadedFile != null || uploadedFile.Length != 0)
                {
                    var extension = Path.GetExtension(uploadedFile.FileName);
                    if (!extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                    {
                        TempData["error"] = "Uploaded file is not support, only pdf files";
                        return RedirectToPage(new { id = incomingId });
                    }
                    if (!uploadedFile.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                    {
                        TempData["error"] = "Uploaded file is not support, only pdf files";
                        return RedirectToPage(new { id = incomingId });
                    }
                }

                // Check file extension.
                int enums = (int)SD.ObjectionStatus.Review;
                var objection = new ObjectionsTable
                {
                    ReferenceId = SD.GenerateUniqueNumberWithInnitials("OBJ-"),
                    AuditCaseId = "N/A",
                    FileTaxId = incomingId,
                    FileName = document_type,
                    Message = message,
                    ObjectionReason = reason,
                    SuggectedAmount = amount,
                    ProcessStatus = enums.ToString(),
                    FilePath = await fileLoader.Fileupload(uploadedFile)
                };
                unitOfWork.ObjectionsTables.Add(objection);
                int result =  unitOfWork.SaveChanges();
                if (result > 0)
                {
                    TempData["success"] = "Objections has been successfully raised, and undertaking review, with 48 workimg hours ";
                    return Page();
                }
                TempData["error"] = "An error ocured while processing this request";
                return Page();
            }
            // If model state is invalid, return the same page with validation errors
            TempData["error"] = "Provide all fields";
            return RedirectToPage(new { id = incomingId });
        }
        //Pending – Under review.
        //In Progress – Being processed.
        //Resolved/Closed – Decision made.
        //Additional Info Required – You may need to submit more documents.
        //Now generate a fuction that will generate a list of objections with the following properties for each objection message each individual objection should have the following properties:
        public class ObjectionStates
        {
            public int Review { get; set; }
            public int Progress { get; set; }
            public int Resolved { get; set; }
            public int Closed { get; set; }
            public int Total { get; set; }
        }
    }
}
