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
    public class RepositoryIncomeSource : Repository<IncomeSource>, IRepositoryIncomeSource
    {
        private readonly ApplicationDbContext context;

        public RepositoryIncomeSource(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public void Update(IncomeSource income)
        {
            context.Update(income);
        }
    }
}
