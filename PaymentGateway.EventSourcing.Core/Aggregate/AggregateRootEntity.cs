using System;
using System.Collections.Generic;
using System.Linq;
using ReflectionMagic;
using PaymentGateway.EventSourcing.Core.Event;

namespace PaymentGateway.EventSourcing.Core.Aggregate
{
    /// <summary>
    ///  Base class for aggregate root entities that need some basic infrastructure for tracking state changes.
    /// </summary>
    public abstract class AggregateRootEntity<TAggregateId> : IAggregateRootEntity<TAggregateId> where TAggregateId : IAggregateId
    {
        private readonly List<IDomainEvent<TAggregateId>> _changes = new List<IDomainEvent<TAggregateId>>();

        private long         Version { get; set; }
        public  TAggregateId Id      { get; protected set; }

        public bool HasChanges()
        {
            return _changes.Any();
        }

        public IEnumerable<IDomainEvent<TAggregateId>> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadsFromHistory(IEnumerable<IDomainEvent<TAggregateId>> events)
        {
            if (events == null)
            {
                throw new ArgumentNullException(nameof(events));
            }

            foreach (var e in events)
            {
                ApplyEvent(e);
            }
        }

        protected void RaiseEvent(DomainEvent<TAggregateId> @event)
        {
            if (@event == null)
            {
                return;
            }

            @event.WithAggregateVersion(Version);
            ApplyEvent(@event);
            _changes.Add(@event);
        }

        private void ApplyEvent(IDomainEvent<TAggregateId> @event)
        {
            this.AsDynamic().Apply((dynamic) @event);
            Version++;
        }
    }
}
