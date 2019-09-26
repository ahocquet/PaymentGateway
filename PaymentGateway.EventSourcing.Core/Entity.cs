using PaymentGateway.EventSourcing.Core.Aggregate;

namespace PaymentGateway.EventSourcing.Core
{
    /// <summary>
    ///  Base class for entities 
    /// </summary>
    public abstract class Entity<TAggregateId> where TAggregateId : IAggregateId
    {
        public TAggregateId Id { get; protected set; }
    }
}
