using SaasBoilerplate.Domain.Common;
using SaasBoilerplate.Domain.Enums;

namespace SaasBoilerplate.Domain.Entities
{
    public class Tenant : AuditableEntity
    {
        public string Identifier { get; set; } = string.Empty; // Used for subdomain or path resolution
        public string Name { get; set; } = string.Empty;
        public string? ConnectionString { get; set; } // Nullable if using shared DB
        public string AdminEmail { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime ValidUntil { get; set; }
        public SubscriptionPlan Plan { get; set; } = SubscriptionPlan.Free;
    }
}
