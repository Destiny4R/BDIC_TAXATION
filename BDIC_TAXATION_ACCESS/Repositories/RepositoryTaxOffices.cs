using BDIC_TAXATION_ACCESS.Data;
using BDIC_TAXATION_ACCESS.Repositories.IRepositories;
using BDIC_TAXATION_MODELS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_ACCESS.Repositories
{
    public class RepositoryTaxOffices : Repository<TaxOffices>, IRepositoryTaxOffices
    {
        private readonly ApplicationDbContext context;

        public RepositoryTaxOffices(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public void Update(TaxOffices taxOffices)
        {
            context.Update(taxOffices);
        }
    }
}
