using MediatR;
using Microsoft.EntityFrameworkCore;
using SaasBoilerplate.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SaasBoilerplate.Application.Tenants.Queries.ExportTenantData
{
    public record ExportTenantDataQuery : IRequest<Dictionary<string, object>>;

    public class ExportTenantDataQueryHandler : IRequestHandler<ExportTenantDataQuery, Dictionary<string, object>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ITenantService _tenantService;

        public ExportTenantDataQueryHandler(IApplicationDbContext context, ITenantService tenantService)
        {
            _context = context;
            _tenantService = tenantService;
        }

        public async Task<Dictionary<string, object>> Handle(ExportTenantDataQuery request, CancellationToken cancellationToken)
        {
            var result = new Dictionary<string, object>();
            
            // This query relies on the Global Query Filters to only select data for the current tenant.
            // If the TenantId is not set in the service, this will return empty or throw depending on implementation.
            
            if (string.IsNullOrEmpty(_tenantService.TenantId))
            {
                return result;
            }

            // Export Tenant Details
            var tenant = await _context.Tenants
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Identifier == _tenantService.TenantId, cancellationToken);
            
            if (tenant != null)
            {
                result.Add("Tenant", tenant);
            }

            // Export Products
            var products = await _context.Products
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            
            result.Add("Products", products);

            return result;
        }
    }
}
