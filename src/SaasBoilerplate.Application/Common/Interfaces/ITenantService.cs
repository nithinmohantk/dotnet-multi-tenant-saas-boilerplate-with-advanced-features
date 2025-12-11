using SaasBoilerplate.Domain.Entities;

namespace SaasBoilerplate.Application.Common.Interfaces
{
    public interface ITenantService
    {
        Tenant? CurrentTenant { get; }
        string? TenantId { get; }
        void SetTenant(Tenant tenant);
    }
}
