using System.Collections.Generic;
using PaymentGateway.EventSourcing.Core.Event;

namespace PaymentGateway.EventSourcing.Core.Aggregate
{
    /// <summary>
    /// Initializes an aggregate.
    /// </summary>
    public interface IAggregateInitializer<in TAggregateId> where TAggregateId : IAggregateId
    {
        /// <summary>
        /// Initializes this instance using the specified events.
        /// </summary>
        /// <param name="events">The events to initialize with.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="events"/> are null.</exception>
        void LoadsFromHistory(IEnumerable<IDomainEvent<TAggregateId>> events);
    }
}