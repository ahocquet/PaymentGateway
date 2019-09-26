using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using PaymentGateway.EventSourcing.Core;

namespace PaymentGateway.Domain.Values
{
    public class Ccv : ValueObject
    {
        private static readonly CcvValidator Validator = new CcvValidator();

        private Ccv()
        {
        }

        public static Ccv Create(int number)
        {
            var instance = CreateInstanceImpl(number);
            Validator.ValidateAndThrow(instance);
            return instance;
        }

        public static ValidationResult Validate(int number)
        {
            var instance = CreateInstanceImpl(number);
            return Validator.Validate(instance);
        }

        private static Ccv CreateInstanceImpl(int number)
        {
            var instance = new Ccv
            {
                Value = number
            };
            return instance;
        }

        public int Value { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        private class CcvValidator : AbstractValidator<Ccv>
        {
            public CcvValidator()
            {
                RuleFor(c => c.Value)
                   .InclusiveBetween(100, 999)
                    .WithMessage("Please specify a valid CCV");
            }
        }
    }
}
