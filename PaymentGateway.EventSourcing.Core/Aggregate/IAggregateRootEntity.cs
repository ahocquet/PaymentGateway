namespace PaymentGateway.EventSourcing.Core.Aggregate
{
    /// <summary>
    /// Aggregate root entity marker interface.
    /// </summary>
    public interface IAggregateRootEntity<TAggregateId> : IAggregateInitializer<TAggregateId>, IAggregateChangeTracker<TAggregateId> where TAggregateId : IAggregateId
    {
    }
}
