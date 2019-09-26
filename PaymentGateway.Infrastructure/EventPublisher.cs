using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaymentGateway.EventSourcing.Core;

namespace PaymentGateway.Infrastructure
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IMediator _mediator;

        // In a real world, replace MediatR by a messaging bus
        public EventPublisher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Publish<T>(T @event, CancellationToken token = default) where T : INotification
        {
            // Call the non generic method in order to get a correct topic name
            // based on runtime instance of @event
            return _mediator.Publish(@event, token);
        }
    }
}
