using MediatR;
using SaasBoilerplate.Application.Common.Interfaces;
using SaasBoilerplate.Domain.Entities;
using SaasBoilerplate.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SaasBoilerplate.Application.Tenants.Commands.CreateTenant
{
    public record CreateTenantCommand : IRequest<Guid>
    {
        public string Name { get; init; } = string.Empty;
        public string Identifier { get; init; } = string.Empty;
        public string AdminEmail { get; init; } = string.Empty;
        public SubscriptionPlan Plan { get; init; } = SubscriptionPlan.Free;
    }

    public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateTenantCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {
            var entity = new Tenant
            {
                Name = request.Name,
                Identifier = request.Identifier,
                AdminEmail = request.AdminEmail,
                Plan = request.Plan,
                CreatedBy = "System", // Placeholder until CurrentUserService is implemented
                CreatedAt = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddDays(30)
            };

            _context.Tenants.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
