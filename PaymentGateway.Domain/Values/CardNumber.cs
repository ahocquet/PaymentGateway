using System.Collections.Generic;
using System.Text.RegularExpressions;
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

        public string Value { get; private set; }

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
            var number = cardNumber?.Replace(" ", "");
            var instance = new CardNumber
            {
                Value = number
            };
            return instance;
        }

        public string MaskCardNumber()
        {
            var lastDigits   = Value.Substring(Value.Length - 4, 4);
            var requiredMask = new string('X', Value.Length - lastDigits.Length);

            var maskedString       = string.Concat(requiredMask, lastDigits);
            var maskedStringSpaced = Regex.Replace(maskedString, ".{4}", "$0 ");

            return maskedStringSpaced.Trim();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        private class CardNumberValidator : AbstractValidator<CardNumber>
        {
            public CardNumberValidator()
            {
                RuleFor(c => c.Value)
                   .NotEmpty()
                   .CreditCard()
                   .WithMessage("Please specify a valid credit card number");
            }
        }
    }
}
