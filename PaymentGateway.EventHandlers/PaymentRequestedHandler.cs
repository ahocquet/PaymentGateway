using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaymentGateway.Domain.Events;

namespace PaymentGateway.EventHandlers
{
    public class PaymentRequestedHandler : INotificationHandler<PaymentRequested>
    {
        public Task Handle(PaymentRequested notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
