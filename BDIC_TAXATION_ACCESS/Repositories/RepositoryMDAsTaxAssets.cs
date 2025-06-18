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
    public class RepositoryMDAsTaxAssets : Repository<MDAsTaxAssets>, IRepositoryMDAsTaxAssets
    {
        private readonly ApplicationDbContext context;

        public RepositoryMDAsTaxAssets(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public void Update(MDAsTaxAssets taxasset)
        {
            context.Update(taxasset);
        }
    }
}
