using BDIC_TAXATION_MODELS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_ACCESS.Repositories.IRepositories
{
    public interface IRepositorySpecialization : IRepository<Specialization>
    {
        void Update(Specialization specialization);
    }
}
