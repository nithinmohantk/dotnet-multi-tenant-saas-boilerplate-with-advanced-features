using Azure.Messaging.EventGrid;
using Microsoft.Extensions.Configuration;
using SaasBoilerplate.Application.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SaasBoilerplate.Infrastructure.Services
{
    public class EventGridPublisher : IEventPublisher
    {
        private readonly EventGridPublisherClient? _client;
        private readonly bool _isEnabled;

        public EventGridPublisher(IConfiguration configuration)
        {
            var topicEndpoint = configuration["EventGrid:TopicEndpoint"];
            var topicKey = configuration["EventGrid:TopicKey"];

            if (!string.IsNullOrEmpty(topicEndpoint) && !string.IsNullOrEmpty(topicKey))
            {
                _client = new EventGridPublisherClient(new Uri(topicEndpoint), new Azure.AzureKeyCredential(topicKey));
                _isEnabled = true;
            }
            else
            {
                _isEnabled = false;
            }
        }

        public async Task PublishAsync<T>(T @event, string subject, CancellationToken cancellationToken = default)
        {
            if (!_isEnabled || _client == null)
            {
                // Log warning: Event Grid not configured
                return;
            }

            var eventGridEvent = new EventGridEvent(
                subject: subject,
                eventType: typeof(T).Name,
                dataVersion: "1.0",
                data: @event
            );

            await _client.SendEventAsync(eventGridEvent, cancellationToken);
        }
    }
}
