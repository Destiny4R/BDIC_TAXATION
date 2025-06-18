using BDIC_TAXATION_ACCESS.Data;
using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_MODELS.DTOs;
using BDIC_TAXATION_MODELS.Models;
using BDIC_TAXATION_MODELS.ViewModels;
using BDIC_TAXATION_UTILITIES;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace BDIC_TAXATION_WEB.Controllers
{
    public class viewController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public viewController(IUnitOfWork unitOfWork, ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        [HttpPost]
        public async Task<JsonResult> lga()
        {
            try
            {
                // Base query (IQueryable for deferred execution)
                IEnumerable<LocalGovernment> query = await unitOfWork.LocalGovernments.GetAllAsync(predicate:null, includes: x => x.Include(x => x.LgaStation));

                // Get DataTables parameters
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal;

                // Apply search filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(m => (m.Name.Contains(searchValue)));
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(sortColumn))
                {
                    //query = query.Order($"{sortColumn} {sortColumnDirection}");
                }
                else
                {
                    query = query.OrderByDescending(k => k.Id);
                }

                recordsTotal = query.Count(); // Total records (after filtering)

                // Pagination
                var data = query.Skip(skip).Take(pageSize).ToList();

                // Map to DTOs
                var newdata = data.Select(q => new Government
                {
                    Id = q.Id,
                    Name = q.Name,
                    Code = q.LGACode
                }).ToList();

                return Json(new {draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = newdata });
            }
            catch (Exception ex)
            {
                // Log the error (e.g., using ILogger)
                return Json(new
                {
                    draw = Request.Form["draw"].FirstOrDefault(),
                    recordsFiltered = 0,
                    recordsTotal = 0,
                    data = new List<Government>(),
                    error = "An error occurred."
                });
            }
        }
        [HttpPost]
        public async Task<JsonResult> lgastations()
        {
            try
            {
                // Base query (IQueryable for deferred execution)
                IEnumerable<LGAStations> query = await unitOfWork.LGAStations.GetAllAsync(predicate: null, includes: x => x.Include(x => x.Localgovernment));

                // Get DataTables parameters
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal;

                // Apply search filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(m => (m.Name.Contains(searchValue)));
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(sortColumn))
                {
                    //query = query.OrderBy($"{sortColumn} {sortColumnDirection}");
                    query = query.OrderByDescending(x => EF.Property<object>(x, sortColumn));
                }
                else
                {
                    query = query.OrderByDescending(k => k.Id);
                }

                recordsTotal = query.Count(); // Total records (after filtering)

                // Pagination
                var data = query.Skip(skip).Take(pageSize).ToList();

                // Map to DTOs
                var newdata = data.Select(q => new Stations
                {
                    Id = q.Id,
                    Name = q.Name,
                    StationCode = q.StationCode,
                    LGId = q.LGId,
                    Lgas = unitOfWork.LocalGovernments.FindSingle(lg => lg.Id == q.LGId)?.Name ?? "N/A"
                }).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = newdata });
            }
            catch (Exception ex)
            {
                // Log the error (e.g., using ILogger)
                return Json(new
                {
                    draw = Request.Form["draw"].FirstOrDefault(),
                    recordsFiltered = 0,
                    recordsTotal = 0,
                    data = new List<Stations>(),
                    error = "An error occurred." + ex.StackTrace
                });
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetConsultants()
        {
            try
            {
                // Base query (IQueryable for deferred execution)
                IEnumerable<ConsultantUser> query = await unitOfWork.ConsultantUsers.GetAllAsync(predicate: null, includes: x => x.Include(x => x.Applicationuser));

                // Get DataTables parameters
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal;

                // Apply search filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(m => (m.Name.Contains(searchValue))
                    || m.Applicationuser.UserName.Contains(searchValue)
                    || m.Email.Contains(searchValue)
                    || m.Phone.Contains(searchValue)
                    || m.TaxIdentificationNumber.Contains(searchValue)
                    || m.RC_Number.Contains(searchValue));
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(sortColumn))
                {
                    //query = query.OrderBy($"{sortColumn} {sortColumnDirection}");
                    //query = query.OrderByDescending(x => EF.Property<object>(x, sortColumn));
                }
                else
                {
                    query = query.OrderByDescending(k => k.Id);
                }

                recordsTotal = query.Count(); // Total records (after filtering)

                // Pagination
                var data = query.Skip(skip).Take(pageSize).ToList();

                // Map to DTOs
                var newdata = data.Select(q => new consultantcy
                {
                    id = q.Id,
                    name = q.Name,
                    email = q.Email,
                    phone = q.Phone,
                    rcnumber = q.RC_Number,
                    address = q.Address,
                    tin = q.TaxIdentificationNumber,
                    username = q.Applicationuser.UserName
                }).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = newdata });
            }
            catch (Exception ex)
            {
                // Log the error (e.g., using ILogger)
                return Json(new
                {
                    draw = Request.Form["draw"].FirstOrDefault(),
                    recordsFiltered = 0,
                    recordsTotal = 0,
                    data = new List<consultantcy>(),
                    error = "An error occurred." + ex.StackTrace
                });
            }
        }
        //SpecializationView
        [HttpPost]
        public async Task<JsonResult> SpecializationView()
        {
            try
            {
                // Base query (IQueryable for deferred execution)
                IEnumerable<Specialization> query = await unitOfWork.Specializations.GetAllAsync(predicate: null, includes: x => x.Include(x => x.Applicationuser));
                query = query.OrderByDescending(q => q.CreatedDate);
                // Get DataTables parameters
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal;

                // Apply search filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(m => (m.Name.Contains(searchValue))
                    || m.Applicationuser.UserName.Contains(searchValue)
                    || m.Code.Contains(searchValue));
                }

                // Apply sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    query = query.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }

                recordsTotal = query.Count(); // Total records (after filtering)

                // Pagination
                var data = query.Skip(skip).Take(pageSize).ToList();

                // Map to DTOs
                var newdata = data.Select(q => new SpecialDTOs
                {
                    id = q.Id,
                    name = q.Name,
                    code = q.Code,
                    createdate = q.CreatedDate
                }).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = newdata });
            }
            catch (Exception ex)
            {
                // Log the error (e.g., using ILogger)
                return Json(new
                {
                    draw = Request.Form["draw"].FirstOrDefault(),
                    recordsFiltered = 0,
                    recordsTotal = 0,
                    data = new List<SpecialDTOs>(),
                    error = "An error occurred." + ex.StackTrace
                });
            }
        }
        //TaxPayeeSettingVM
        [HttpPost]
        public async Task<JsonResult> TaxPayeeSettingVM()
        {
            try
            {
                // Base query (IQueryable for deferred execution)
                IEnumerable<PayeeCalculator> query = await unitOfWork.PayeeCalculators.GetAllAsync(predicate: null);
                query = query.OrderByDescending(q => q.CreatedDate);
                // Get DataTables parameters
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal;

                // Apply search filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(m => (m.Code.Contains(searchValue)));
                }

                // Apply sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    query = query.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }

                recordsTotal = query.Count(); // Total records (after filtering)

                // Pagination
                var data = query.Skip(skip).Take(pageSize).ToList();

                // Map to DTOs
                var newdata = data.Select(q => new PayeeSettingsVM
                {
                    id = q.Id,
                    high = SD.ToNaira(q.HighestAmount),
                    low = SD.ToNaira(q.LowestAmount),
                    code = q.Code,
                    percent = (float)q.Percent,
                    createddate = q.CreatedDate
                }).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = newdata });
            }
            catch (Exception ex)
            {
                // Log the error (e.g., using ILogger)
                return Json(new
                {
                    draw = Request.Form["draw"].FirstOrDefault(),
                    recordsFiltered = 0,
                    recordsTotal = 0,
                    data = new List<PayeeCalculator>(),
                    error = "An error occurred." + ex.StackTrace
                });
            }
        }
        //TaxPayeeSettingVM
        [HttpPost]
        public async Task<JsonResult> FiLeReturnTaxIndividual()
        {
            try
            {
                //Get the current user
                var user = userManager.GetUserAsync(User).Result;
                // Base query (IQueryable for deferred execution)
                IEnumerable<FileReturnIndividual> query = await unitOfWork.FileReturnIndividuals.GetAllAsync(predicate: q=>q.IndividualTaxpayer.ApplicationUserId==user.Id, q=>q.Include(q=>q.IndividualTaxpayer.Applicationuser));
                query = query.OrderByDescending(q => q.CreatedDate);
                // Get DataTables parameters
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal;

                // Apply search filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(m => (m.AssetName.Contains(searchValue)
                    || m.AssetTaxable.Contains(searchValue) 
                    || m.SessionYear.Contains(searchValue) 
                    || m.ReferenceNo.Contains(searchValue)));
                }

                // Apply sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    query = query.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }

                recordsTotal = query.Count(); // Total records (after filtering)

                // Pagination
                var data = query.Skip(skip).Take(pageSize).ToList();

                // Map to DTOs
                var newdata = data.Select(q => new firereturnDTO
                {
                    id = q.Id,
                    income = SD.ToNaira(q.IncomeAmount),
                    taxamount = SD.ToNaira(q.TaxAmount),
                    referenceno = q.ReferenceNo,
                    assetname = q.AssetName,
                    assettaxable = q.AssetTaxable,
                    percent = (float)q.Percent,
                    session = q.SessionYear,
                    status = q.CanPaidStatus,
                    createddate = q.CreatedDate
                }).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = newdata });
            }
            catch (Exception ex)
            {
                // Log the error (e.g., using ILogger)
                return Json(new
                {
                    draw = Request.Form["draw"].FirstOrDefault(),
                    recordsFiltered = 0,
                    recordsTotal = 0,
                    data = new List<firereturnDTO>(),
                    error = "An error occurred." + ex.Message
                });
            }
        }

        //TAX OFFICE VIEWS
        [HttpPost]
        public async Task<JsonResult> TaxOfficesViews()
        {
            try
            {
                IEnumerable<TaxOffices> query  = await unitOfWork.TaxOffices.GetAllAsync(predicate: null, q => q.Include(q => q.ApplicationUser).Include(q=>q.LocalGovernment));

                    query = query.OrderByDescending(q => q.CreatedDate);
                // Get DataTables parameters
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal;

                // Apply search filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(m => (m.Name.Contains(searchValue)
                    || m.Email.Contains(searchValue)
                    || m.Code.Contains(searchValue)
                    || m.OfficerInCharge.Contains(searchValue)
                    || m.LocalGovernment.Name.Contains(searchValue)));
                }

                // Apply sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    query = query.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }

                recordsTotal = query.Count(); // Total records (after filtering)

                // Pagination
                var data = query.Skip(skip).Take(pageSize).ToList();

                // Map to DTOs
                var newdata = data.Select(q => new TaxOfficesDTOs
                {
                    id = q.Id,
                    name = q.Name,
                    code = q.Code,
                    lga = q.LocalGovernment.Name,
                    officerincharge = q.OfficerInCharge,
                    address = q.Address,
                    email = q.Email,
                    phone = q.Phone,
                    status = q.Status,
                    createdate = q.CreatedDate
                }).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = newdata });
            }
            catch (Exception ex)
            {
                // Log the error (e.g., using ILogger)
                return Json(new
                {
                    draw = Request.Form["draw"].FirstOrDefault(),
                    recordsFiltered = 0,
                    recordsTotal = 0,
                    data = new List<TaxOfficesDTOs>(),
                    error = "An error occurred." + ex.Message
                });
            }
        }

        //TAX ASSESSMENT VIEWS
        [HttpPost]
        public async Task<JsonResult> TaxAssessmentViews()
        {
            try
            {
                IEnumerable<FileReturnIndividual> query = await unitOfWork.FileReturnIndividuals.GetAllAsync(predicate: null, q => q.Include(q => q.IndividualTaxpayer.Applicationuser));

                query = query.OrderByDescending(q => q.CreatedDate);
                // Get DataTables parameters
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal;

                // Apply search filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(m => (m.AssetName.Contains(searchValue)
                    || m.AssetTaxable.Contains(searchValue)
                    || m.ReferenceNo.Contains(searchValue)
                    || m.SessionYear.Contains(searchValue)
                    || m.IndividualTaxpayer.BusinessName.Contains(searchValue)
                    || m.IndividualTaxpayer.TIN.Contains(searchValue)
                    || m.IndividualTaxpayer.Phone.Contains(searchValue)
                    || m.IndividualTaxpayer.TaxIdentificationNumber.Contains(searchValue)
                    || m.IndividualTaxpayer.Email.Contains(searchValue)
                    || m.IndividualTaxpayer.NationalIdentificationNumber.Contains(searchValue)
                    || m.IndividualTaxpayer.BusinessName.Contains(searchValue)));
                }

                // Apply sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    query = query.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }

                recordsTotal = query.Count(); // Total records (after filtering)

                // Pagination
                var data = query.Skip(skip).Take(pageSize).ToList();

                // Map to DTOs
                var newdata = data.Select(q => new AssessmentDTO
                {
                    id = q.Id,
                    companyname = q.IndividualTaxpayer.Surname+" "+ q.IndividualTaxpayer.OtherName+" "+ q.IndividualTaxpayer.FirstName,
                    username = q.IndividualTaxpayer.Applicationuser.UserName,
                    session = q.SessionYear,
                    income = SD.ToNaira(q.IncomeAmount),
                    taxamount = SD.ToNaira(q.TaxAmount),
                    phone = q.IndividualTaxpayer.Applicationuser.PhoneNumber,
                    status = q.CanPaidStatus,
                    createdate = q.CreatedDate
                }).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = newdata });
            }
            catch (Exception ex)
            {
                // Log the error (e.g., using ILogger)
                return Json(new
                {
                    draw = Request.Form["draw"].FirstOrDefault(),
                    recordsFiltered = 0,
                    recordsTotal = 0,
                    data = new List<AssessmentDTO>(),
                    error = "An error occurred." + ex.Message
                });
            }
        }

        //TAX OBJECTIONS FOR INDIVIDUAL VIEWS
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> taxObjectionsIndividual()
        {
            try
            {
                //Get The Current User Id
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //Check if userId is null or empty
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { draw = Request.Form["draw"].FirstOrDefault(), recordsFiltered = 0, recordsTotal = 0, data = new List<ObjectionsVM>(), error = "User not found." });
                }
                IEnumerable<ObjectionsTable> query = await unitOfWork.ObjectionsTables.GetAllAsync(predicate: q=>q.FileReturnIndividual.IndividualTaxpayer.ApplicationUserId==userId, q => q.Include(q => q.FileReturnIndividual.IndividualTaxpayer.Applicationuser));

                query = query.OrderByDescending(q => q.CreatedDate);
                // Get DataTables parameters
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal;

                // Apply search filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(m => (m.FileReturnIndividual.AssetName.Contains(searchValue)
                    || m.FileReturnIndividual.AssetTaxable.Contains(searchValue)
                    || m.FileReturnIndividual.ReferenceNo.Contains(searchValue)
                    || m.FileReturnIndividual.SessionYear.Contains(searchValue)
                    || m.FileReturnIndividual.IndividualTaxpayer.BusinessName.Contains(searchValue)
                    || m.FileReturnIndividual.IndividualTaxpayer.TIN.Contains(searchValue)
                    || m.FileReturnIndividual.IndividualTaxpayer.Phone.Contains(searchValue)
                    || m.FileReturnIndividual.IndividualTaxpayer.TaxIdentificationNumber.Contains(searchValue)
                    || m.FileReturnIndividual.IndividualTaxpayer.Email.Contains(searchValue)
                    || m.FileReturnIndividual.IndividualTaxpayer.NationalIdentificationNumber.Contains(searchValue)
                    || m.FileReturnIndividual.IndividualTaxpayer.BusinessName.Contains(searchValue)));
                }

                // Apply sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    query = query.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }

                recordsTotal = query.Count(); // Total records (after filtering)

                // Pagination
                var data = query.Skip(skip).Take(pageSize).ToList();

                // Map to DTOs
                var newdata = data.Select(q => new ObjectionsVM
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
                    createdate = q.CreatedDate
                }).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = newdata });
            }
            catch (Exception ex)
            {
                // Log the error (e.g., using ILogger)
                return Json(new
                {
                    draw = Request.Form["draw"].FirstOrDefault(),
                    recordsFiltered = 0,
                    recordsTotal = 0,
                    data = new List<ObjectionsVM>(),
                    error = "An error occurred." + ex.Message
                });
            }
        }

        //TAX OBJECTIONS FOR INDIVIDUAL VIEWS
        [Authorize(Roles =SD.Role_Admin)]
        [HttpPost]
        public async Task<JsonResult> taxObjectionsheads()
        {
            try
            {
                IEnumerable<ObjectionsTable> query = await unitOfWork.ObjectionsTables.GetAllAsync(predicate: null, q => q.Include(q => q.FileReturnIndividual.IndividualTaxpayer.Applicationuser));

                query = query.OrderByDescending(q => q.CreatedDate);
                // Get DataTables parameters
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal;

                // Apply search filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(m => (m.FileReturnIndividual.AssetName.Contains(searchValue)
                    || m.FileReturnIndividual.AssetTaxable.Contains(searchValue)
                    || m.FileReturnIndividual.ReferenceNo.Contains(searchValue)
                    || m.FileReturnIndividual.SessionYear.Contains(searchValue)
                    || m.FileReturnIndividual.IndividualTaxpayer.BusinessName.Contains(searchValue)
                    || m.FileReturnIndividual.IndividualTaxpayer.TIN.Contains(searchValue)
                    || m.FileReturnIndividual.IndividualTaxpayer.Phone.Contains(searchValue)
                    || m.FileReturnIndividual.IndividualTaxpayer.TaxIdentificationNumber.Contains(searchValue)
                    || m.FileReturnIndividual.IndividualTaxpayer.Email.Contains(searchValue)
                    || m.FileReturnIndividual.IndividualTaxpayer.NationalIdentificationNumber.Contains(searchValue)
                    || m.FileReturnIndividual.IndividualTaxpayer.BusinessName.Contains(searchValue)));
                }

                // Apply sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    query = query.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }

                recordsTotal = query.Count(); // Total records (after filtering)

                // Pagination
                var data = query.Skip(skip).Take(pageSize).ToList();

                // Map to DTOs
                var newdata = data.Select(q => new ObjectionsVM
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
                    createdate = q.CreatedDate
                }).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = newdata });
            }
            catch (Exception ex)
            {
                // Log the error (e.g., using ILogger)
                return Json(new
                {
                    draw = Request.Form["draw"].FirstOrDefault(),
                    recordsFiltered = 0,
                    recordsTotal = 0,
                    data = new List<ObjectionsVM>(),
                    error = "An error occurred." + ex.Message
                });
            }
        }

        //TAX OBJECTIONS FOR INDIVIDUAL VIEWS
        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        public async Task<JsonResult> ministryaccount()
        {
            try
            {
                IEnumerable<MDAsMinistry> query = await unitOfWork.MDAsMinistry.GetAllAsync(predicate: null, q => q.Include(q => q.Applicationuser));

                query = query.OrderByDescending(q => q.CreatedDate);
                // Get DataTables parameters
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal;

                // Apply search filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(m => (m.Applicationuser.UserName.Contains(searchValue)
                    || m.Applicationuser.Email.Contains(searchValue)
                    || m.Name.Contains(searchValue)));
                }

                // Apply sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    query = query.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }

                recordsTotal = query.Count(); // Total records (after filtering)

                // Pagination
                var data = query.Skip(skip).Take(pageSize).ToList();

                // Map to DTOs
                var newdata = data.Select(q => new MDAsMinistryVM
                {
                    id = q.Id,
                    name = q.Name,
                    code = q.Applicationuser.UserName,
                    description = SD.TruncateStringToMaxLength(q.Description, 20),
                    establistdate = q.EstablistedDate,
                    status = q.Applicationuser.LockoutEnabled,
                    createdate = q.CreatedDate
                }).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = newdata });
            }
            catch (Exception ex)
            {
                // Log the error (e.g., using ILogger)
                return Json(new
                {
                    draw = Request.Form["draw"].FirstOrDefault(),
                    recordsFiltered = 0,
                    recordsTotal = 0,
                    data = new List<MDAsMinistryVM>(),
                    error = "An error occurred." + ex.Message
                });
            }
        }
    }
}
