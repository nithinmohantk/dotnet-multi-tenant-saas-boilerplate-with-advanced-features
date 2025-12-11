using SaasBoilerplate.Application.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SaasBoilerplate.Infrastructure.Services
{
    public class MockBillingService : IBillingService
    {
        public Task<string> CreateCustomerAsync(string name, string email, CancellationToken cancellationToken = default)
        {
            // Simulate API call to Stripe
            return Task.FromResult($"cus_{Guid.NewGuid().ToString("N").Substring(0, 10)}");
        }

        public Task<string> CreateSubscriptionAsync(string customerId, string planId, CancellationToken cancellationToken = default)
        {
            // Simulate API call to Stripe
            return Task.FromResult($"sub_{Guid.NewGuid().ToString("N").Substring(0, 10)}");
        }
    }
}
