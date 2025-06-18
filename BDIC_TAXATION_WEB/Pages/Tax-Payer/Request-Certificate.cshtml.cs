using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_ACCESS.SystemRepos;
using BDIC_TAXATION_MODELS.Models;
using BDIC_TAXATION_UTILITIES;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.RateLimiting;

namespace BDIC_TAXATION_WEB.Pages.Tax_Payer
{
    [Authorize]
    public class Request_CertificateModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileLoader fileLoader;

        public PagedResult<FileReturnIndividual> FileReturn { get; set; }
        public IndividualTaxpayer individual { get; set; }
        public Request_CertificateModel(IUnitOfWork unitOfWork, IFileLoader fileLoader)
        {
            this.unitOfWork = unitOfWork;
            this.fileLoader = fileLoader;
        }
        public void OnGet(int pg = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Filereturn = unitOfWork.FileReturnIndividuals.GetAllAsync(q=>q.IndividualTaxpayer.ApplicationUserId==userId, includes: q=>q.Include(q=>q.IndividualTaxpayer.Applicationuser)).GetAwaiter().GetResult();
            if (Filereturn.Any())
            {
                individual = Filereturn.FirstOrDefault().IndividualTaxpayer;
            }
            else
            {
                individual = unitOfWork.IndividualTaxpayer.FindSingle(q=>q.ApplicationUserId==userId);
            }
                int index = SD.ToIndex(pg);
            int pageSize = 10;
            //Implement PageResult function from cloudscribe.Pagination.Models
            FileReturn = new PagedResult<FileReturnIndividual>
            {
                Data = Filereturn.Skip(index*pageSize).Take(pageSize).ToList(),
                PageSize = pageSize,
                PageNumber = pg,
                TotalItems = Filereturn.Count()
            };

        }
        public async Task<IActionResult> OnPost(string purpose, IFormFileCollection uploadedfile, string taxPeriod, string filename)
        {
            if (ModelState.IsValid)
            {
                //check if the Iformfilecollections are pdf file
                if (uploadedfile != null || uploadedfile.Count > 0)
                {
                    foreach (var item in uploadedfile)
                    {
                        if (!item.ContentType.Contains("application/pdf"))
                        {
                            TempData["error"] = "One of the uploaded file is not a valid document.";
                            return RedirectToPage();
                        }
                    }
                }
                //Get the current user identifier id
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //Check if the user is an individual taxpayer
                var fileTax = unitOfWork.FileReturnIndividuals.FindSingle(x => x.IndividualTaxpayer.ApplicationUserId == userId && x.SessionYear==taxPeriod);
                if (fileTax == null)
                {
                    TempData["error"] = $"You have neither filed not paid {taxPeriod} tax; therefore, you can't apply for a certificate.";
                    return RedirectToPage();
                }
                //Check if the user has already requested a certificate for the current year
                var certificate = new TaxIndividualCertificate
                {
                    Description = purpose,
                    SessionYear = taxPeriod,
                    FileReturnId = fileTax.Id,
                    FileName = filename
                };
                certificate.Documents = [];
                if (uploadedfile != null || uploadedfile.Count > 0)
                {
                    foreach (var item in uploadedfile)
                    {
                        var file = new Document
                        {
                            TaxCertificateId = certificate.Id,
                            FilePath = await fileLoader.Fileupload(item),
                            FileNamePath = filename
                        };
                        certificate.Documents.Add(file);
                    }
                }
                //Add the certificate to the database
                unitOfWork.TaxIndividualCertificate.Add(certificate);
                int result = unitOfWork.SaveChanges();
                //Check if the certificate was added successfully
                if (result > 0)
                {
                    TempData["success"] = "Certificate request submitted successfully.";
                }
                else
                {
                    TempData["error"] = "Failed to submit certificate request.";
                }
                return RedirectToPage("Index");
            }
            TempData["error"] = "Missing information, check and try again.";
            return RedirectToPage();
        }
        
    }
}
