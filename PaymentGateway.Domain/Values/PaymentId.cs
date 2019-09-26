using System;
using PaymentGateway.EventSourcing.Core.Aggregate;

namespace PaymentGateway.Domain.Values
{
    public class PaymentId : IAggregateId
    {
        public Guid Id { get; private set; }

        public PaymentId(Guid id)
        {
            Id = id;
        }

        public static PaymentId Create()
        {
            return new PaymentId(Guid.NewGuid());
        }

        public string IdAsString()
        {
            return Id.ToString("D");
        }
    }
}