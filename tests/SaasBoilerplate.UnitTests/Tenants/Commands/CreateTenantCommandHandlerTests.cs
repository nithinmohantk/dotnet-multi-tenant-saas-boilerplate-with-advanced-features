using Microsoft.EntityFrameworkCore;
using Moq;
using SaasBoilerplate.Application.Common.Interfaces;
using SaasBoilerplate.Application.Tenants.Commands.CreateTenant;
using SaasBoilerplate.Domain.Entities;
using SaasBoilerplate.Domain.Enums;
using Xunit;

namespace SaasBoilerplate.UnitTests.Tenants.Commands
{
    public class CreateTenantCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldCreateTenant_WhenCommandIsValid()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Tenant>>();
            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.Setup(m => m.Tenants).Returns(mockSet.Object);

            var handler = new CreateTenantCommandHandler(mockContext.Object);
            var command = new CreateTenantCommand
            {
                Name = "Test Tenant",
                Identifier = "test-tenant",
                AdminEmail = "admin@test.com",
                Plan = SubscriptionPlan.Pro
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            mockSet.Verify(m => m.Add(It.IsAny<Tenant>()), Times.Once());
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
