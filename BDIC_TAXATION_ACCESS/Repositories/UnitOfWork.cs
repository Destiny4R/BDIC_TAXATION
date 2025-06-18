using BDIC_TAXATION_ACCESS.Data;
using BDIC_TAXATION_ACCESS.Repositories;
using BDIC_TAXATION_ACCESS.Repositories.IRepositories;
using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_MODELS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_ACCESS.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            ApplicationUser = new RepositoryApplicationUser(dbContext);
            BusinessTaxPayer = new RepositoryBusinessTaxPayer(dbContext);
            DigitalTaxCard = new RepositoryDigitalTaxCard(dbContext);
            Document = new RepositoryDocument(dbContext);
            IncomeSource = new RepositoryIncomeSource(dbContext);
            IndividualTaxpayer = new RepositoryIndividualTaxpayer(dbContext);
            MDAsMinistry = new RepositoryMDAsMinistry(dbContext);
            Payment = new RepositoryPayment(dbContext);
            TaxPaymentPercentSettings = new RepositoryTaxPaymentPercentSettings(dbContext);
            UserEmailSettings = new RepositoryUserEmailSettings(dbContext);
            UsersAccount = new RepositoryUsersAccount(dbContext);
            LogActivities = new RepositoryLogActivity(dbContext);
            ConsultantUsers = new RepositoryConsultantUser(dbContext);
            LocalGovernments = new RepositoryLocalGovernment(dbContext);
            LGAStations = new RepositoryLGAStations(dbContext);
            Specializations = new RepositorySpecialization(dbContext);
            PayeeCalculators = new RepositoryPayeeCalculator(dbContext);
            FileReturnIndividuals = new RepositoryFileReturnIndividual(dbContext);
            TaxIndividualCertificate = new RepositoryTaxIndividualCertificate(dbContext);
            TaxOffices = new RepositoryTaxOffices(dbContext);
            MakePayments = new RepositoryMakePayment(dbContext);
            ObjectionsTables = new RepositoryObjectionsTable(dbContext);
            MDAsTaxAssets = new RepositoryMDAsTaxAssets(dbContext);
        }

        public IRepositoryApplicationUser ApplicationUser {get; private set;}

        public IRepositoryBusinessTaxPayer BusinessTaxPayer {get; private set;}

        public IRepositoryDigitalTaxCard DigitalTaxCard {get; private set;}

        public IRepositoryDocument Document {get; private set;}

        public IRepositoryIncomeSource IncomeSource {get; private set;}

        public IRepositoryIndividualTaxpayer IndividualTaxpayer {get; private set;}

        public IRepositoryMDAsMinistry MDAsMinistry {get; private set;}

        public IRepositoryPayment Payment {get; private set;}

        public IRepositoryTaxPaymentPercentSettings TaxPaymentPercentSettings {get; private set;}

        public IRepositoryUserEmailSettings UserEmailSettings {get; private set;}

        public IRepositoryUsersAccount UsersAccount {get; private set;}
        public IRepositoryLogActivity LogActivities { get; private set; }
        public IRepositoryConsultantUser ConsultantUsers { get; private set; }
        public IRepositoryLocalGovernment LocalGovernments { get; private set; }
        public IRepositoryLGAStations LGAStations { get; private set; }
        public IRepositorySpecialization Specializations { get; private set; }
        public IRepositoryPayeeCalculator PayeeCalculators { get; private set; }
        public IRepositoryFileReturnIndividual FileReturnIndividuals { get; private set; }
        public IRepositoryTaxIndividualCertificate TaxIndividualCertificate {  get; private set; }
        public IRepositoryTaxOffices TaxOffices {  get; private set; }
        public IRepositoryMakePayment MakePayments { get; private set; }
        public IRepositoryObjectionsTable ObjectionsTables { get; private set; }
        public IRepositoryMDAsTaxAssets MDAsTaxAssets { get; private set; }
        public async Task<int> SaveAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }
    }
}
