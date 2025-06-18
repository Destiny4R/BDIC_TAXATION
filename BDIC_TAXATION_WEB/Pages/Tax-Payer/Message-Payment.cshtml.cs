using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_ACCESS.SystemRepos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BDIC_TAXATION_WEB.Pages.Tax_Payer
{
    public class Message_PaymentModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITransactionService transaction;
        public bool message = false;
        //create a constructor and inject iunitofwork respo
        public Message_PaymentModel(IUnitOfWork unitOfWork, ITransactionService transaction)
        {
            this.unitOfWork = unitOfWork;
            this.transaction = transaction;
        }
        public async Task<IActionResult> OnGet(string reference)
        {
            if (!string.IsNullOrEmpty(reference))
            {

                try
                {
                    var makepayment = unitOfWork.MakePayments.FindSingle(predicate: q => q.Reference == reference, includes: q => q.Include(q => q.fileReturnIndividual.IndividualTaxpayer.Applicationuser));
                    if (makepayment != null)
                    {
                        var response = await transaction.GetTransactionDetailsAsync(reference);
                        if (response != null)
                        {
                            if (response.Status == 200)
                            {
                                var fileindividual = makepayment.fileReturnIndividual;
                                fileindividual.CertificateStatus = true;
                                fileindividual.UpdatedDate = Convert.ToDateTime(response.Data.TransactionDate);

                                //Update makepayment table now
                                makepayment.Status = true;
                                makepayment.PaymentDate = Convert.ToDateTime(response.Data.TransactionDate);
                                makepayment.Party_Reference = response.Data.TransRef;
                                unitOfWork.MakePayments.Update(makepayment);
                                unitOfWork.FileReturnIndividuals.Update(fileindividual);
                                int result = unitOfWork.SaveChanges();
                                if (result > 0)
                                {
                                    message = true;
                                    return Page();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    message = false;
                    Console.WriteLine($"ERROR: {ex.Message}");
                }
            }
            return Page();
        }
    }
}
