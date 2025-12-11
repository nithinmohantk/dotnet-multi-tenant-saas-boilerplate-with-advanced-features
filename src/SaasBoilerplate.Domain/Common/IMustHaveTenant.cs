using System;

namespace SaasBoilerplate.Domain.Common
{
    public interface IMustHaveTenant
    {
        string TenantId { get; set; }
    }
}
