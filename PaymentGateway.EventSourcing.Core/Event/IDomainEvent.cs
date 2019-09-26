using MediatR;
using PaymentGateway.EventSourcing.Core.Aggregate;

namespace PaymentGateway.EventSourcing.Core.Event
{
    public interface IDomainEvent<out TAggregateId> : INotification where TAggregateId : IAggregateId
    {
        /// <summary>
        /// The identifier of the aggregate which has generated the event
        /// </summary>
        TAggregateId AggregateId { get; }

        /// <summary>
        /// The version of the aggregate when the event has been generated
        /// </summary>
        long AggregateVersion { get; }
    }
}
