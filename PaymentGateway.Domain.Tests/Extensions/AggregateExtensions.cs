using PaymentGateway.EventSourcing.Core.Aggregate;

namespace PaymentGateway.Domain.Tests.Extensions
{
    public static class AggregateExtensions
    {
        public static AggregateAssertions<TAggregateId> Should<TAggregateId>(this AggregateRootEntity<TAggregateId> instance) where TAggregateId : IAggregateId
        {
            return new AggregateAssertions<TAggregateId>(instance);
        }
    }
}