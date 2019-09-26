using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace PaymentGateway.EventSourcing.Core
{
    public interface IEventPublisher
    {
        Task Publish<T>(T @event, CancellationToken token = default) where T : INotification;
    }
}
