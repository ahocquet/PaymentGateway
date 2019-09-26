using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Values;
using PaymentGateway.SharedKernel.Date;

namespace PaymentGateway.TestKernel
{
    public static class PaymentFactory
    {
        public static Payment Create(IProvideDateTime dateProvider)
        {
            var expiryDate = ExpiryDate.Create(08, 2020, dateProvider);
            var cardNumber = CardNumber.Create("4111 1111 1111 1111");
            var ccv        = Ccv.Create(123);

            var paymentId  = PaymentId.Create();
            var creditCard = CreditCard.Create(cardNumber, expiryDate, ccv);
            var amount     = Money.Create(12, Currency.EUR);
            return new Payment(paymentId, creditCard, amount);
        }
    }
}