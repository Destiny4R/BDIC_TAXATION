using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_MODELS.DTOs;
using BDIC_TAXATION_MODELS.Models;
using BDIC_TAXATION_MODELS.ViewModels;
using BDIC_TAXATION_UTILITIES;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BDIC_TAXATION_WEB.Controllers
{
    public class v1Controller : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly SignInManager<ApplicationUser> signInManager;

        public v1Controller(IUnitOfWork unitOfWork, SignInManager<ApplicationUser> signInManager)
        {
            this.unitOfWork = unitOfWork;
            this.signInManager = signInManager;
        }
        //SIGN OUT ACTION
        public IActionResult SignOut()
        {
            try
            {
                if(signInManager.IsSignedIn(User))
                {
                    //Sign out the user
                    signInManager.SignOutAsync();
                    return RedirectToPage("/Account/Login");
                }
                return RedirectToPage("/Account/Login");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        //DELETE LOCAL GOVERNMENT
        [HttpDelete]
        public IActionResult deletelga(int id)
        {
            try
            {
                if (id > 0)
                {
                    //Check if the LG has station register under it
                    var checkStations = unitOfWork.LGAStations.GetAllAsync(predicate: x => x.LGId == id).Result;
                    if(checkStations.Any())
                    {
                        return Json(new { success = false, message = "Item cannot be deleted because it has associated stations" });
                    }
                    //Get Local Government data
                    var item = unitOfWork.LocalGovernments.GetByIdSingle(id);
                    if (item == null)
                    {
                        return Json(new { success = false, message = "Item not found" });
                    }
                    //Delete the item at this point
                    unitOfWork.LocalGovernments.Remove(item);
                    var result = unitOfWork.SaveChanges();
                    if (result > 0)
                    {
                        return Json(new { success = true, message = "Item deleted successfully" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Failed to delete item" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Invalid ID" });
                }
            }
            catch (Exception ex)
            {
                return Json( new { success = false, message = ex.Message });
            }
        }
        //DELETE LOCAL GOVERNMENT STATION
        [HttpDelete]
        public IActionResult deletelgastation(int id)
        {
            try
            {
                if (id > 0)
                {
                    //Check if the Station is in use before deleting
                    //var checkStations = unitOfWork.LGAStations.GetAllAsync(predicate: x => x.LGId == id).Result;
                    //if (checkStations.Any())
                    //{
                    //    return Json(new { success = false, message = "Item cannot be deleted because it has associated stations" });
                    //}
                    //Get Local Government data
                    var item = unitOfWork.LGAStations.GetByIdSingle(id);
                    if (item == null)
                    {
                        return Json(new { success = false, message = "Item not found" });
                    }
                    //Delete the item at this point
                    unitOfWork.LGAStations.Remove(item);
                    var result = unitOfWork.SaveChanges();
                    if (result > 0)
                    {
                        return Json(new { success = true, message = "Item deleted successfully" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Failed to delete item" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Invalid ID" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        //GET SPECIALIZATION FOR EDIT
        [HttpPost]
        public IActionResult GetSpecializationForEdit(int id)
        {
            if (id>0)
            {
                try
                {
                    var special = unitOfWork.Specializations.FindSingle(x => x.Id == id);
                    if(special == null)
                    {
                        return Json(new { success = false, message = "Specialization not found" });
                    }
                    var data = new SpecialDTOs
                    {
                        id = special.Id,
                        name = special.Name,
                        code = special.Code
                    };
                    return Json(new { success = true, data= data});
                }
                catch (Exception ex)
                {
                    return Json(new {success = false, message= "An error occurred while updating the specialization" });
                }
            }
            return Json(new { success = false, message = "We couldn't identify this record!" });
        }

        [HttpDelete]
        public IActionResult GetSpecializationToDelete(int id)
        {
            if (id > 0)
            {
                try
                {
                    //Check if the Specialization has station register under it in Consultancy Table
                    var checkStations = unitOfWork.ConsultantUsers.GetAllAsync(predicate: x => x.SpecialId == id).Result;
                    if (checkStations.Any())
                    {
                        return Json(new { success = false, message = "Item cannot be deleted because it has associated stations" });
                    }

                    var special = unitOfWork.Specializations.FindSingle(x => x.Id == id);
                    if (special == null)
                    {
                        return Json(new { success = false, message = "Specialization not found" });
                    }
                    unitOfWork.Specializations.Remove(special);
                    unitOfWork.SaveChanges();
                    return Json(new { success = true, message = "Specialization successfully removed!" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "An error occurred while updating the specialization" });
                }
            }
            return Json(new { success = false, message = "We couldn't identify this record!" });
        }
        //GET Payee Setting FOR EDIT
        [HttpPost]
        public IActionResult GetPayeeSettingForEdit(int id)
        {
            if (id > 0)
            {
                try
                {
                    var special = unitOfWork.PayeeCalculators.FindSingle(x => x.Id == id);
                    if (special == null)
                    {
                        return Json(new { success = false, message = "Payee Setting not found" });
                    }
                    var data = new PayeeSettingsVM
                    {
                        id = special.Id,
                        low = special.LowestAmount.ToString(),
                        high = special.HighestAmount.ToString(),
                        code = special.Code,
                        percent = (float)special.Percent
                    };
                    return Json(new { success = true, data = data });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "An error occurred while updating the Payee Setting" });
                }
            }
            return Json(new { success = false, message = "We couldn't identify this record!" });
        }

        //GetPayeeSettingToDelete
        [HttpDelete]
        public IActionResult GetPayeeSettingToDelete(int id)
        {
            if (id > 0)
            {
                try
                {
                    var payee = unitOfWork.PayeeCalculators.FindSingle(x => x.Id == id);
                    if (payee == null)
                    {
                        return Json(new { success = false, message = "Payee Setting not found" });
                    }
                    unitOfWork.PayeeCalculators.Remove(payee);
                    unitOfWork.SaveChanges();
                    return Json(new { success = true, message = "Payee Setting successfully removed!" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "An error occurred while updating the Payee Setting" });
                }
            }
            return Json(new { success = false, message = "We couldn't identify this record!" });
        }
        //DeleteFileTaxReturnIndividual
        [HttpDelete]
        public IActionResult DeleteFileTaxReturnIndividual(int id)
        {
            if (id > 0)
            {
                try
                {
                    //Check if the File Tax Return has been paid for in the Payment Table
                    var checkPayments = unitOfWork.Payment.GetAllAsync(predicate: x => x.FileidividualTaxId == id).Result;
                    if (checkPayments.Any())
                    {
                        return Json(new { success = false, message = "Item cannot be deleted because it has associated payments" });
                    }
                    var fileTax = unitOfWork.FileReturnIndividuals.FindSingle(x => x.Id == id);
                    if (fileTax == null)
                    {
                        return Json(new { success = false, message = "File Tax Return not found" });
                    }
                    unitOfWork.FileReturnIndividuals.Remove(fileTax);
                    unitOfWork.SaveChanges();
                    return Json(new { success = true, message = "File Tax Return successfully removed!" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "An error occurred while updating the File Tax Return" });
                }
            }
            return Json(new { success = false, message = "We couldn't identify this record!" });
        }
        //create a json method to to return FileReturnIndividual single data taking in an id
        [HttpPost]
        public IActionResult GetFileReturnIndividual(int id)
        {
            if (id > 0)
            {
                try
                {
                    var fileTax = unitOfWork.FileReturnIndividuals.FindSingle(x => x.Id == id);
                    if (fileTax == null)
                    {
                        return Json(new { success = false, message = "File Tax Return not found" });
                    }
                    var data = new firereturnDTO
                    {
                        id = fileTax.Id,
                        income = SD.ToNaira(fileTax.IncomeAmount),
                        assetname = fileTax.AssetName,
                        session = fileTax.SessionYear,
                        assettaxable = fileTax.AssetTaxable,
                        referenceno = fileTax.ReferenceNo,
                        taxamount = SD.ToNaira(fileTax.TaxAmount),
                        percent = fileTax.Percent,
                        createddate = fileTax.CreatedDate,
                        status = fileTax.CanPaidStatus,
                        message = fileTax.Message,
                        transactionstate = fileTax.CertificateStatus
                    };
                    return Json(new { success = true, data = data });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "An error occurred while updating the File Tax Return" });
                }
            }
            return Json(new { success = false, message = "We couldn't identify this record!" });
        }

        //Write a function to Delete a Tax Office
        [HttpDelete]
        public IActionResult DeleteFileTaxOffices(int id)
        {
            if (id > 0)
            {
                try
                {
                    var officeTax = unitOfWork.TaxOffices.FindSingle(x => x.Id == id);
                    if (officeTax == null)
                    {
                        return Json(new { success = false, message = "Tax Office not found" });
                    }
                    unitOfWork.TaxOffices.Remove(officeTax);
                    unitOfWork.SaveChanges();
                    return Json(new { success = true, message = "Tax Office successfully removed!" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "An error occurred while Deleting the Tax office" });
                }
            }
            return Json(new { success = false, message = "We couldn't identify this record!" });
        }
        //Get Tax Office data for editting/Update
        [HttpPost]
        public IActionResult GetTaxOfficedata(int id)
        {
            if (id > 0)
            {
                try
                {
                    var taxoffice = unitOfWork.TaxOffices.FindSingle(x => x.Id == id, q=>q.Include(k=>k.LocalGovernment));
                    if (taxoffice == null)
                    {
                        return Json(new { success = false, message = "Tax Office not found" });
                    }
                    var data = new TaxOfficesDTOs
                    {
                        id = taxoffice.Id,
                        code = taxoffice.Code,
                        name = taxoffice.Name,
                        lgaId = taxoffice.LgaId,
                        lga = taxoffice.LocalGovernment.Name,
                        officerincharge = taxoffice.OfficerInCharge,
                        status = taxoffice.Status,
                        address = taxoffice.Address,
                        email = taxoffice.Email,
                        phone = taxoffice.Phone
                    };
                    return Json(new { success = true, data = data });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "An error occurred while getting Tax Office Information" });
                }
            }
            return Json(new { success = false, message = "We couldn't identify this record!" });
        }
        //Write IActionResult to Disable an Tax Office by changing the status to false
        [HttpPost]
        public IActionResult DisableTaxOffice(int id)
        {
            if (id > 0)
            {
                try
                {
                    var taxoffice = unitOfWork.TaxOffices.FindSingle(x => x.Id == id);
                    if (taxoffice == null)
                    {
                        return Json(new { success = false, message = "Tax Office not found" });
                    }
                    string msg = string.Empty;
                    //Same function should work for enabling the Tax Office by changing the status to true
                    if (taxoffice.Status)
                    {
                        msg = "Tax Office has been successfully disabled!";
                        taxoffice.Status = false;
                    }
                    else
                    {
                        msg = "Tax Office successfully enabled!";
                        taxoffice.Status = true;
                    }
                    unitOfWork.TaxOffices.Update(taxoffice);
                    int result = unitOfWork.SaveChanges();
                    if (result <= 0)
                    {
                        return Json(new { success = false, message = "Failed to update Tax Office status" });
                    }
                    return Json(new { success = true, message  = msg});
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "An error occurred while disabling the Tax Office" });
                }
            }
            return Json(new { success = false, message = "We couldn't identify this record!" });
        }

        //GET TAX ASSESSMENT INFORMATION FOR EDITING/UPDATE
        [HttpPost]
        public IActionResult GetTaxAssessmentedata(int id)
        {
            if (id > 0)
            {
                try
                {
                    var taxoffice = unitOfWork.FileReturnIndividuals.FindSingle(x => x.Id == id, q => q.Include(k => k.IndividualTaxpayer.Applicationuser).Include(q=>q.TaxIndividualCertificate));
                    if (taxoffice == null)
                    {
                        return Json(new { success = false, message = "Tax Assessment information not found" });
                    }
                    var data = new AssessmentDTO
                    {
                        id = taxoffice.Id,
                        companyname = taxoffice.IndividualTaxpayer.Surname + " " + taxoffice.IndividualTaxpayer.OtherName + " " + taxoffice.IndividualTaxpayer.FirstName,
                        username = taxoffice.IndividualTaxpayer.Applicationuser.UserName,
                        session = taxoffice.SessionYear,
                        income = SD.ToNaira(taxoffice.IncomeAmount),
                        taxamount = SD.ToNaira(taxoffice.TaxAmount),
                        amountleft = SD.ToNaira(taxoffice.IncomeAmount - taxoffice.TaxAmount),
                        phone = taxoffice.IndividualTaxpayer.Applicationuser.PhoneNumber,
                        status = taxoffice.CanPaidStatus,
                        createdate = taxoffice.CreatedDate,
                        filepath = taxoffice?.TaxIndividualCertificate?.FilePath ?? "",
                        filename = taxoffice.TaxIndividualCertificate.FileName
                    };
                    return Json(new { success = true, data = data });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "An error occurred while getting Tax Assessment Information" });
                }
            }
            return Json(new { success = false, message = "We couldn't identify this record!" });
        }
        //RejectTaxRequest?id=${id}&reason=${msg}
        [HttpPost]
        public IActionResult RejectTaxRequest(int id, string reason, bool isoption, string status)
        {
            if (id > 0)
            {
                try
                {
                    var taxoffice = unitOfWork.FileReturnIndividuals.FindSingle(x => x.Id == id);
                    if (taxoffice == null)
                    {
                        return Json(new { success = false, message = "Tax Assessment information not found" });
                    }
                    string msg = "";
                    if (isoption)
                    {
                        taxoffice.CanPaidStatus = true;
                        msg = "Tax return assessment successfully processed for payment.";
                    }
                    else
                    {
                        taxoffice.CanPaidStatus = false;
                        taxoffice.IsReject = true;
                        taxoffice.Message = reason;
                        taxoffice.FlaggedReason = status;
                        msg = "Tax return assessment successfully rejected and flagged for further evaluation!";
                    }
                    unitOfWork.FileReturnIndividuals.Update(taxoffice);
                    int result = unitOfWork.SaveChanges();
                    if (result <= 0)
                    {
                        return Json(new { success = false, message = "Failed to update Tax Assessment status" });
                    }
                    return Json(new { success = true, message = msg });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "An error occurred while rejecting the Tax Assessment request" });
                }
            }
            return Json(new { success = false, message = "We couldn't identify this record!" });
        }

        [Authorize]
        [HttpPost]
        public IActionResult GetObjectionDetailsIndividual(int id)
        {
            if (id > 0)
            {
                try
                {
                   ObjectionsTable q;
                        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (User.IsInRole(SD.Role_Admin))
                    {
                        q = unitOfWork.ObjectionsTables.FindSingle(predicate: q=>q.Id==id, q => q.Include(q => q.FileReturnIndividual.IndividualTaxpayer.Applicationuser));
                    }
                    else
                    {
                        q = unitOfWork.ObjectionsTables.FindSingle(predicate: q =>q.Id==id && q.FileReturnIndividual.IndividualTaxpayer.ApplicationUserId == userId, q => q.Include(q => q.FileReturnIndividual.IndividualTaxpayer.Applicationuser));
                    }
                    //Check if userId is null or empty
                    if (string.IsNullOrEmpty(userId))
                    {
                        return Json(new { success = false, message = "User not found" });
                    }
                    
                    if (q == null)
                    {
                        return Json(new { success = false, message = "Objections information not found" });
                    }
                    var certi = unitOfWork.TaxIndividualCertificate.FindSingle(q => q.FileReturnId == q.FileReturnId, q => q.Include(i => i.FileReturnIndividual));
                    var data = new ObjectionsVM
                    {
                        id = q.Id,
                        name = q.FileReturnIndividual.IndividualTaxpayer.Surname + " " + q.FileReturnIndividual.IndividualTaxpayer.OtherName + " " + q.FileReturnIndividual.IndividualTaxpayer.FirstName,
                        payerid = q.FileReturnIndividual.IndividualTaxpayer.Applicationuser.UserName,
                        session = q.FileReturnIndividual.SessionYear,
                        reference = q.ReferenceId,
                        objectiontype = q.ProcessStatus,
                        message = q.Message,
                        income = SD.ToNaira(q.FileReturnIndividual.IncomeAmount),
                        taxamount = SD.ToNaira(q.FileReturnIndividual.TaxAmount),
                        createdate = q.CreatedDate,
                        status = q.FileReturnIndividual.Message,
                        reason = q.ObjectionReason,
                        filepath = certi?.FilePath??"",
                        filename = certi?.FileName??""
                    };
                    if (User.IsInRole(SD.Role_Admin))
                    {
                        //LETS UPDATE THE OBJECTION STATUS TO PROCESSING AS THE PERSON HAS OPEN IT
                        if (q.ProcessStatus == "0")
                        {
                            q.ProcessStatus = ((int)SD.ObjectionStatus.Progress).ToString();
                            unitOfWork.ObjectionsTables.Update(q);
                           int result =  unitOfWork.SaveChanges();
                        }
                    }
                    return Json(new { success = true, data = data });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "An error occurred while processing this request" });
                }
            }
            return Json(new { success = false, message = "We couldn't identify this record!" });
        }
        //CANCEL OBJECTION
        [Authorize(Roles =SD.Role_Individual)]
        [HttpPost]
        public IActionResult CancelObjectionIndividual(int id)
        {
            if (id>0)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    var obj = unitOfWork.ObjectionsTables.FindSingle(predicate: q => q.Id == id && q.FileReturnIndividual.IndividualTaxpayer.ApplicationUserId == userId, q => q.Include(q => q.FileReturnIndividual.IndividualTaxpayer.Applicationuser));
                    if (obj != null)
                    {
                        //Check if the objection is already closed
                        if (obj.ProcessStatus == ((int)SD.ObjectionStatus.Closed).ToString())
                        {
                            return Json(new { success = false, message = "This objection has already been closed!" });
                        }
                        obj.ProcessStatus = ((int)SD.ObjectionStatus.Closed).ToString();
                        unitOfWork.ObjectionsTables.Update(obj);
                        int result = unitOfWork.SaveChanges();
                        //Check and return appropriate response
                        if (result > 0)
                        {
                            return Json(new { success = true, message = "Objection successfully cancelled!" });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Failed to cancel objection" });
                        }
                    }
                }
            }
            //Return error if userId is null or empty or if the objection is not found
            return Json(new { success = false, message = "We couldn't identify this record or you are not authorized to perform this action!" });
        }
        //FLAG OBJECT RESOLVED BY ADMIN
        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        public IActionResult ResolvedObjectionAdmin(int id, string remark)
        {
            if (id > 0 && !string.IsNullOrEmpty(remark))
            {
                var obj = unitOfWork.ObjectionsTables.FindSingle(predicate: q => q.Id == id , q => q.Include(q => q.FileReturnIndividual.IndividualTaxpayer.Applicationuser));
                    if (obj != null)
                    {
                        //Check if the objection is already closed
                        if (obj.ProcessStatus == ((int)SD.ObjectionStatus.Resolved).ToString())
                        {
                            return Json(new { success = false, message = "This objection has already been resolved!" });
                        }
                    if (obj.ProcessStatus == ((int)SD.ObjectionStatus.Closed).ToString())
                    {
                        return Json(new { success = false, message = "This objection has already been Closed by client!" });
                    }
                    obj.ProcessStatus = ((int)SD.ObjectionStatus.Resolved).ToString();
                    obj.Remarks = remark;
                        unitOfWork.ObjectionsTables.Update(obj);
                        int result = unitOfWork.SaveChanges();
                        //Check and return appropriate response
                        if (result > 0)
                        {
                            return Json(new { success = true, message = "Objection successfully resolved!" });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Failed to resolved objection" });
                        }
                    }
            }
            return Json(new { success = false, message = "We couldn't identify this record or you are not authorized to perform this action!" });
        }
    }
}
