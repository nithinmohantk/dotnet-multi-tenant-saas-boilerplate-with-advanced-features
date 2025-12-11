using System.Threading;
using System.Threading.Tasks;

namespace SaasBoilerplate.Application.Common.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T @event, string subject, CancellationToken cancellationToken = default);
    }
}
