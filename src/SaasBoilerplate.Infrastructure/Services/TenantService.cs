using SaasBoilerplate.Application.Common.Interfaces;
using SaasBoilerplate.Domain.Entities;

namespace SaasBoilerplate.Infrastructure.Services
{
    public class TenantService : ITenantService
    {
        public Tenant? CurrentTenant { get; private set; }

        public string? TenantId => CurrentTenant?.Identifier;

        public void SetTenant(Tenant tenant)
        {
            CurrentTenant = tenant;
        }
    }
}
