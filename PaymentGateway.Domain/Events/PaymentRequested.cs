using PaymentGateway.Domain.Values;
using PaymentGateway.EventSourcing.Core.Event;

namespace PaymentGateway.Domain.Events
{
    public class PaymentRequested : DomainEvent<PaymentId>
    {
        public PaymentId  Id         { get; private set; }
        public CreditCard CreditCard { get; private set; }
        public Money      Amount     { get; private set; }

        public PaymentRequested(PaymentId id, CreditCard creditCard, Money amount) : base(id)
        {
            Id         = id;
            CreditCard = creditCard;
            Amount     = amount;
        }
    }
}
