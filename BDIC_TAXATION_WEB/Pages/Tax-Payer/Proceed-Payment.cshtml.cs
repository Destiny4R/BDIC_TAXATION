using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_MODELS.Models;
using BDIC_TAXATION_UTILITIES;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BDIC_TAXATION_WEB.Pages.Tax_Payer
{

    public class Proceed_PaymentModel : PageModel
    {
        //Create a constructor and inject IUnitOfWork
        //Create a public class from payment
        public payment pay { get; set; }

        private readonly IUnitOfWork _unitOfWork;
        public Proceed_PaymentModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void OnGet(int id)
        {
            //Get the individual taxpayer by id
            try
            {
                var fileTax = _unitOfWork.FileReturnIndividuals.GetFirstOrDefaultAsync(x => x.Id == id, includes: q => q.Include(q => q.IndividualTaxpayer.Applicationuser)).Result;
                if (fileTax == null)
                {
                    //Handle the case when the taxpayer is not found
                    //Redirect to an error page or show a message
                    TempData["error"] = "Taxpayer not found.";
                    RedirectToPage("File-Tax");
                }
                //Create a Make-Payment properties and add to unitofwork
                var makePay = new MakePayment
                {
                    Reference = SD.GenerateUniqueNumber(),
                    //Party_Reference = "null",
                    Amount = fileTax.TaxAmount,
                    PaymentMethod = "Credo",
                    FileidividualTaxId = fileTax.Id
                };
                _unitOfWork.MakePayments.Add(makePay);
                int result = _unitOfWork.SaveChanges();
                if (result > 0)
                {
                    //Set the payment properties
                    pay = new payment
                    {
                        Amount = fileTax.TaxAmount,
                        assetname = fileTax.AssetName,
                        ReferenceNo = makePay.Reference,
                        email = fileTax.IndividualTaxpayer.Applicationuser.Email,
                        firstname = fileTax.IndividualTaxpayer.FirstName,
                        lastname = fileTax.IndividualTaxpayer.Surname,
                        phone = fileTax.IndividualTaxpayer.Phone,
                        session = fileTax.SessionYear,
                        IndividualId = (int)fileTax.IndividualId
                    };
                }
                else
                {
                    TempData["error"] = "Failed to create payment.";
                    RedirectToPage("File-Tax");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error: "+ex.Message;
                RedirectToPage("File-Tax");
            }
        }
        public class payment
        {
            public double Amount { get; set; }
            public string assetname { get; set; }
            public string ReferenceNo { get; set; }
            public string email { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string phone { get; set; }
            public string session { get; set; }
            public int IndividualId { get; set; }
        }
    }
}