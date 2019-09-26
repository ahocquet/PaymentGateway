using System;
using PaymentGateway.EventSourcing.Core.Aggregate;

namespace PaymentGateway.Domain.Values
{
    public class BankTransactionId : IAggregateId
    {
        public Guid Id { get; private set; }

        public BankTransactionId(Guid id)
        {
            Id = id;
        }

        public static BankTransactionId Create()
        {
            return new BankTransactionId(Guid.NewGuid());
        }

        public string IdAsString()
        {
            return Id.ToString("D");
        }
    }
}
