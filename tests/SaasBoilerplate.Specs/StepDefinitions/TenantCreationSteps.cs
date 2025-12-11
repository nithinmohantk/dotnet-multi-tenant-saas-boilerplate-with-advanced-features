using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using SaasBoilerplate.Application.Common.Interfaces;
using SaasBoilerplate.Application.Tenants.Commands.CreateTenant;
using SaasBoilerplate.Domain.Entities;
using SaasBoilerplate.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SaasBoilerplate.Specs.StepDefinitions
{
    [Binding]
    public class TenantCreationSteps
    {
        private CreateTenantCommand? _command;
        private Guid _result;
        private Mock<IApplicationDbContext> _mockContext;
        private Mock<DbSet<Tenant>> _mockSet;
        private CreateTenantCommandHandler _handler;

        public TenantCreationSteps()
        {
            _mockSet = new Mock<DbSet<Tenant>>();
            _mockContext = new Mock<IApplicationDbContext>();
            _mockContext.Setup(m => m.Tenants).Returns(_mockSet.Object);
            _handler = new CreateTenantCommandHandler(_mockContext.Object);
        }

        [Given(@"I have a valid tenant creation request with the following details:")]
        public void GivenIHaveAValidTenantCreationRequestWithTheFollowingDetails(Table table)
        {
            var row = table.Rows[0];
            _command = new CreateTenantCommand
            {
                Name = row["Name"],
                Identifier = row["Identifier"],
                AdminEmail = row["AdminEmail"],
                Plan = Enum.Parse<SubscriptionPlan>(row["Plan"])
            };
        }

        [When(@"I submit the create tenant request")]
        public async Task WhenISubmitTheCreateTenantRequest()
        {
            _result = await _handler.Handle(_command, CancellationToken.None);
        }

        [Then(@"the tenant should be created successfully")]
        public void ThenTheTenantShouldBeCreatedSuccessfully()
        {
            _mockSet.Verify(m => m.Add(It.IsAny<Tenant>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Then(@"the tenant ID should be a valid GUID")]
        public void ThenTheTenantIDShouldBeAValidGUID()
        {
            _result.Should().NotBeEmpty();
        }
    }
}
