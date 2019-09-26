using System;
using System.Linq;
using System.Threading.Tasks;
using PaymentGateway.EventSourcing.Core;
using PaymentGateway.EventSourcing.Core.Aggregate;

namespace PaymentGateway.Persistence
{
    public class AggregateRepository<TAggregateRoot, TAggregateId> : IAggregateRepository<TAggregateRoot, TAggregateId>
        where TAggregateRoot : AggregateRootEntity<TAggregateId>
        where TAggregateId : IAggregateId
    {
        private readonly IEventStore _eventStore;

        public AggregateRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<TAggregateRoot> Get(TAggregateId aggregateId)
        {
            var events    = await _eventStore.GetEventsForAggregate(aggregateId);
            var aggregate = (TAggregateRoot) Activator.CreateInstance(typeof(TAggregateRoot), true);
            aggregate.LoadsFromHistory(events);

            return aggregate;
        }

        public async Task Save(TAggregateRoot aggregateRoot)
        {
            if (!aggregateRoot.HasChanges())
            {
                return;
            }

            var events = aggregateRoot.GetUncommittedChanges()
                                      .OrderBy(t => t.AggregateVersion)
                                      .ToArray();
            var expectedVersion = events.First().AggregateVersion;

            await _eventStore.SaveEvents(aggregateRoot.Id, events, expectedVersion);

            aggregateRoot.MarkChangesAsCommitted();
        }
    }
}
