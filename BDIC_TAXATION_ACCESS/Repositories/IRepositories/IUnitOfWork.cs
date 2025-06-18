using BDIC_TAXATION_ACCESS.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_ACCESS.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IRepositoryApplicationUser ApplicationUser { get; }
        IRepositoryBusinessTaxPayer BusinessTaxPayer { get; }
        IRepositoryDigitalTaxCard DigitalTaxCard { get; }
        IRepositoryDocument Document { get; }
        IRepositoryIncomeSource IncomeSource { get; }
        IRepositoryIndividualTaxpayer IndividualTaxpayer { get; }
        IRepositoryMDAsMinistry MDAsMinistry { get; }
        IRepositoryPayment Payment { get; }
        IRepositoryTaxPaymentPercentSettings TaxPaymentPercentSettings { get; }
        IRepositoryUserEmailSettings UserEmailSettings { get; }
        IRepositoryUsersAccount UsersAccount { get; }
        IRepositoryLogActivity LogActivities { get; }
        IRepositoryConsultantUser ConsultantUsers { get; }
        IRepositoryLocalGovernment LocalGovernments { get; }
        IRepositoryLGAStations LGAStations { get; }
        IRepositorySpecialization Specializations { get; }
        IRepositoryPayeeCalculator PayeeCalculators { get; }
        IRepositoryFileReturnIndividual FileReturnIndividuals { get; }
        IRepositoryTaxIndividualCertificate TaxIndividualCertificate { get; }
        IRepositoryTaxOffices TaxOffices { get; }
        IRepositoryMakePayment MakePayments { get; }
        IRepositoryObjectionsTable ObjectionsTables { get; }
        IRepositoryMDAsTaxAssets MDAsTaxAssets { get; }

        int SaveChanges();
        Task<int> SaveAsync();
    }
}
