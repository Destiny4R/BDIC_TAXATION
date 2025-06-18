using BDIC_TAXATION_MODELS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Net;

namespace BDIC_TAXATION_ACCESS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>(q=>q.HasIndex(q=>q.PhoneNumber).IsUnique());
            modelBuilder.Entity<BusinessTaxpayer>(q => q.HasIndex(q => q.BusinessName).IsUnique());
            modelBuilder.Entity<BusinessTaxpayer>(q => q.HasIndex(q => q.TaxIdentificationNumber).IsUnique());
            modelBuilder.Entity<BusinessTaxpayer>(q => q.HasIndex(q => q.RegistrationNumber).IsUnique());
            modelBuilder.Entity<IndividualTaxpayer>(q => q.HasIndex(q => q.NationalIdentificationNumber).IsUnique());
            modelBuilder.Entity<IndividualTaxpayer>(q => q.HasIndex(q => q.TaxIdentificationNumber).IsUnique());
            modelBuilder.Entity<MDAsMinistry>(q => q.HasIndex(q => q.Name).IsUnique());
            modelBuilder.Entity<Payment>(q => q.HasIndex(q => q.PaymentReference).IsUnique());
            modelBuilder.Entity<Specialization>(q => q.HasIndex(q => q.Name).IsUnique());
            modelBuilder.Entity<MakePayment>(q => q.HasIndex(q => q.Reference).IsUnique());
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<BusinessTaxpayer> BusinessTaxpayers { get; set; }
        public DbSet<IndividualTaxpayer> IndividualTaxpayers { get; set; }
        public DbSet<MDAsMinistry> MDAsMinistries { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<IncomeSource> IncomeSources { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DigitalTaxCard> DigitalTaxCards { get; set; }
        public DbSet<TaxPaymentPercentSettings> TaxPaymentPercentSettings { get; set; }
        public DbSet<UserEmailSettings> UserEmailSettings { get; set; }
        public DbSet<UsersAccount> UsersAccounts { get; set; }
        public DbSet<SystemManagers> SystemManagers { get; set; }
        public DbSet<ConsultantUser> ConsultantUsers { get; set; }
        public DbSet<LogActivity> LogActivities { get; set; }
        public DbSet<LocalGovernment> LocalGovernments { get; set; }
        public DbSet<LGAStations> LGAstations { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<PayeeCalculator> PayeeCalculators { get; set; }
        public DbSet<FileReturnIndividual> FileReturnIndividuals { get; set; }
        public DbSet<TaxIndividualCertificate> TaxIndividualCertificates { get; set; }
        public DbSet<TaxOffices> TaxOfficeses { get; set; }
        public DbSet<MakePayment> MakePayments { get; set; }
        public DbSet<ObjectionsTable> ObjectionsTables { get; set; }
    }
}
