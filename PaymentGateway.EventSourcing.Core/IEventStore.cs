using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentGateway.EventSourcing.Core.Aggregate;
using PaymentGateway.EventSourcing.Core.Event;

namespace PaymentGateway.EventSourcing.Core
{
    public interface IEventStore
    {
        Task SaveEvents<TAggregateId>(TAggregateId aggregateId, IDomainEvent<TAggregateId>[] events, long expectedVersion) where TAggregateId : IAggregateId;
        Task<List<IDomainEvent<TAggregateId>>> GetEventsForAggregate<TAggregateId>(TAggregateId aggregateId) where TAggregateId : IAggregateId;
    }
}
