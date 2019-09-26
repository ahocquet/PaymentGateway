using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using PaymentGateway.EventSourcing.Core;

namespace PaymentGateway.Domain.Values
{
    public class CardNumber : ValueObject
    {
        private static readonly CardNumberValidator Validator = new CardNumberValidator();

        private CardNumber()
        {
        }

        public static CardNumber Create(string cardNumber)
        {
            var instance = CreateInstanceImpl(cardNumber);
            Validator.ValidateAndThrow(instance);
            return instance;
        }

        public static ValidationResult Validate(string cardNumber)
        {
            var instance = CreateInstanceImpl(cardNumber);
            return Validator.Validate(instance);
        }

        private static CardNumber CreateInstanceImpl(string cardNumber)
        {
            var number = cardNumber.Replace(" ", "");
            var instance = new CardNumber
            {
                Value = number
            };
            return instance;
        }

        public string Value { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        private class CardNumberValidator : AbstractValidator<CardNumber>
        {
            public CardNumberValidator()
            {
                RuleFor(c => c.Value)
                   .CreditCard()
                   .WithMessage("Please specify a valid credit card number");
            }
        }
    }
}
