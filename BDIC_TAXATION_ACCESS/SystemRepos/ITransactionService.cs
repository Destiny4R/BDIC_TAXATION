using BDIC_TAXATION_MODELS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_ACCESS.SystemRepos
{
    public interface ITransactionService
    {
        Task<VerifyPaymentResponse> GetTransactionDetailsAsync(string reference);
    }
}
