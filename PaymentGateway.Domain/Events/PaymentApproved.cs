using PaymentGateway.Domain.Values;
using PaymentGateway.EventSourcing.Core.Event;

namespace PaymentGateway.Domain.Events
{
    public class PaymentApproved : DomainEvent<PaymentId>
    {
        public BankTransactionId TransactionId { get; private set; }

        public PaymentApproved(PaymentId aggregateId, BankTransactionId transactionId) : base(aggregateId)
        {
            TransactionId = transactionId;
        }
    }
}