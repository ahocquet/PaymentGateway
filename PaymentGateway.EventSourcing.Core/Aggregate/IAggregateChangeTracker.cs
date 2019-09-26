using System.Collections.Generic;
using PaymentGateway.EventSourcing.Core.Event;

namespace PaymentGateway.EventSourcing.Core.Aggregate
{
    /// <summary>
    /// Tracks changes that happen to an aggregate
    /// </summary>
    public interface IAggregateChangeTracker<out TAggregateId> where TAggregateId : IAggregateId
    {
        /// <summary>
        /// Determines whether this instance has state changes.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has state changes; otherwise, <c>false</c>.
        /// </returns>
        bool HasChanges();

        /// <summary>
        /// Gets the state changes applied to this instance.
        /// </summary>
        /// <returns>A list of recorded state changes.</returns>
        IEnumerable<IDomainEvent<TAggregateId>> GetUncommittedChanges();

        /// <summary>
        /// Clears the state changes.
        /// </summary>
        void MarkChangesAsCommitted();
    }
}
