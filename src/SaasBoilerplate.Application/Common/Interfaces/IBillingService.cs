using System.Threading;
using System.Threading.Tasks;

namespace SaasBoilerplate.Application.Common.Interfaces
{
    public interface IBillingService
    {
        Task<string> CreateCustomerAsync(string name, string email, CancellationToken cancellationToken = default);
        Task<string> CreateSubscriptionAsync(string customerId, string planId, CancellationToken cancellationToken = default);
    }
}
