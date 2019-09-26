using PaymentGateway.Domain.Events;
using PaymentGateway.Domain.Exceptions;
using PaymentGateway.Domain.Values;
using PaymentGateway.EventSourcing.Core.Aggregate;

namespace PaymentGateway.Domain.Entities
{
    public class Payment : AggregateRootEntity<PaymentId>
    {
        // ReSharper disable once UnusedMember.Local
        private Payment()
        {
        }

        public Payment(PaymentId id, CreditCard creditCard, Money amount)
        {
            RaiseEvent(new PaymentRequested(id, creditCard, amount));
        }

        public Money             Amount        { get; private set; }
        public CreditCard        CreditCard    { get; private set; }
        public PaymentStatus     Status        { get; private set; }
        public BankTransactionId TransactionId { get; private set; }

        public void Approve(BankTransactionId transactionId)
        {
            if (Status != PaymentStatus.Pending)
            {
                throw new PaymentAlreadyProcessedException(Id);
            }

            if (transactionId == null)
            {
                throw new TransactionIdCannotBeNullException(Id);
            }

            RaiseEvent(new PaymentApproved(Id, transactionId));
        }

        public void Reject(BankTransactionId transactionId)
        {
            if (Status != PaymentStatus.Pending)
            {
                throw new PaymentAlreadyProcessedException(Id);
            }

            if (transactionId == null)
            {
                throw new TransactionIdCannotBeNullException(Id);
            }

            RaiseEvent(new PaymentRejected(Id, transactionId));
        }

        internal void Apply(PaymentRequested @event)
        {
            Id         = @event.Id;
            Amount     = @event.Amount;
            CreditCard = @event.CreditCard;
            Status     = PaymentStatus.Pending;
        }

        internal void Apply(PaymentApproved @event)
        {
            Status        = PaymentStatus.Approved;
            TransactionId = @event.TransactionId;
        }

        internal void Apply(PaymentRejected @event)
        {
            Status        = PaymentStatus.Rejected;
            TransactionId = @event.TransactionId;
        }
    }
}
