using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaasBoilerplate.Application.Tenants.Commands.CreateTenant;
using SaasBoilerplate.Application.Tenants.Queries.ExportTenantData;
using SaasBoilerplate.Application.Tenants.Queries.GetTenant;
using System;
using System.Threading.Tasks;

namespace SaasBoilerplate.WebApi.Controllers
{
    public class TenantsController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateTenantCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Domain.Entities.Tenant>> Get(Guid id)
        {
            var tenant = await Mediator.Send(new GetTenantQuery(id));
            
            if (tenant == null)
            {
                return NotFound();
            }

            return tenant;
        }

        [HttpGet("export")]
        [Authorize] // Requires authentication to identify which tenant data to export
        public async Task<ActionResult> ExportData()
        {
            var data = await Mediator.Send(new ExportTenantDataQuery());
            return Ok(data);
        }
    }
}
