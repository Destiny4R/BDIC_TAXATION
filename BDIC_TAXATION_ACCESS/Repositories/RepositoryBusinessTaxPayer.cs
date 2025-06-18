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
    public class RepositoryBusinessTaxPayer : Repository<BusinessTaxpayer>, IRepositoryBusinessTaxPayer
    {
        private readonly ApplicationDbContext context;

        public RepositoryBusinessTaxPayer(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public void Update(BusinessTaxpayer business)
        {
            context.Update(business);
        }
    }
}
