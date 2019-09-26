using PaymentGateway.EventSourcing.Core.Aggregate;

namespace PaymentGateway.EventSourcing.Core.Event
{
    public abstract class DomainEvent<TAggregateId> : IDomainEvent<TAggregateId> where TAggregateId : IAggregateId
    {
        protected DomainEvent(TAggregateId aggregateId)
        {
            AggregateId = aggregateId;
        }

        internal void WithAggregateVersion(long aggregateVersion)
        {
            AggregateVersion = aggregateVersion;
        }

        public TAggregateId AggregateId      { get; private set; }
        public long         AggregateVersion { get; private set; }
    }
}
