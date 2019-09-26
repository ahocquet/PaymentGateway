using System.Collections.Generic;
using FluentValidation;
using PaymentGateway.EventSourcing.Core;

namespace PaymentGateway.Domain.Values
{
    public class CreditCard : ValueObject
    {
        private static readonly CreditCardValidator Validator = new CreditCardValidator();

        private CreditCard()
        {
        }

        public static CreditCard Create(CardNumber cardNumber, ExpiryDate expiryDate, Ccv ccv)
        {
            var instance = new CreditCard
            {
                CardNumber = cardNumber,
                ExpiryDate = expiryDate,
                Ccv        = ccv
            };

            Validator.ValidateAndThrow(instance);
            return instance;
        }

        public Ccv        Ccv        { get; private set; }
        public ExpiryDate ExpiryDate { get; private set; }
        public CardNumber CardNumber { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CardNumber;
            yield return Ccv;
            yield return ExpiryDate;
        }

        private class CreditCardValidator : AbstractValidator<CreditCard>
        {
            public CreditCardValidator()
            {
                RuleFor(c => c.CardNumber).NotNull();
                RuleFor(c => c.ExpiryDate).NotNull();
                RuleFor(c => c.Ccv).NotNull();
            }
        }
    }
}