using MediatR;
using Microsoft.EntityFrameworkCore;
using SaasBoilerplate.Application.Common.Interfaces;
using SaasBoilerplate.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SaasBoilerplate.Application.Tenants.Queries.GetTenant
{
    public record GetTenantQuery(Guid Id) : IRequest<Tenant?>;

    public class GetTenantQueryHandler : IRequestHandler<GetTenantQuery, Tenant?>
    {
        private readonly IApplicationDbContext _context;

        public GetTenantQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Tenant?> Handle(GetTenantQuery request, CancellationToken cancellationToken)
        {
            return await _context.Tenants
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        }
    }
}
