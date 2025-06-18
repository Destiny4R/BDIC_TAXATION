using BDIC_TAXATION_MODELS.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_ACCESS.Repositories.IRepositories
{
    public interface IRepositoryUserEmailSettings : IRepository<UserEmailSettings>
    {
        void Update(UserEmailSettings userEmailSettings);
    }
}
