using SaasBoilerplate.Domain.Common;

namespace SaasBoilerplate.Domain.Entities
{
    public class Product : AuditableEntity, IMustHaveTenant
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string TenantId { get; set; } = string.Empty;
    }
}
