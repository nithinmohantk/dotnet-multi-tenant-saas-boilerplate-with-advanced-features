using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SaasBoilerplate.Application.Common.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace SaasBoilerplate.WebApi.Middleware
{
    public class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantResolutionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantService tenantService, IApplicationDbContext dbContext)
        {
            var tenantIdentifier = context.Request.Headers["X-Tenant-ID"].FirstOrDefault();

            if (!string.IsNullOrEmpty(tenantIdentifier))
            {
                // In a real scenario, we might cache this lookup
                var tenant = await dbContext.Tenants
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Identifier == tenantIdentifier);

                if (tenant != null)
                {
                    tenantService.SetTenant(tenant);
                }
            }

            await _next(context);
        }
    }
}
