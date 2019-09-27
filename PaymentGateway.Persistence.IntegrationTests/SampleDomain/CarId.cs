using System;
using PaymentGateway.EventSourcing.Core.Aggregate;

namespace PaymentGateway.Persistence.IntegrationTests.SampleDomain
{
    public class CarId : IAggregateId
    {
        public readonly Guid Id;

        public CarId(Guid id)
        {
            Id = id;
        }

        public string IdAsString()
        {
            return Id.ToString("D");
        }
    }
}