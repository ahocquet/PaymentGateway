using PaymentGateway.Domain.Values;
using PaymentGateway.EventSourcing.Core.Event;

namespace PaymentGateway.Domain.Events
{
    public class PaymentRejected : DomainEvent<PaymentId>
    {
        public BankTransactionId TransactionId { get; private set; }

        public PaymentRejected(PaymentId aggregateId, BankTransactionId transactionId) : base(aggregateId)
        {
            TransactionId = transactionId;
        }
    }
}
