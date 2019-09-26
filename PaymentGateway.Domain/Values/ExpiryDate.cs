using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using PaymentGateway.EventSourcing.Core;
using PaymentGateway.SharedKernel.Date;

namespace PaymentGateway.Domain.Values
{
    public class ExpiryDate : ValueObject
    {
        private ExpiryDate()
        {
        }

        public static ExpiryDate Create(int month, int year, IProvideDateTime dateProvider)
        {
            var instance = CreateInstance(month, year);
            new Validator(dateProvider).ValidateAndThrow(instance);

            return instance;
        }

        public static ValidationResult Validate(int month, int year, IProvideDateTime dateProvider)
        {
            var instance = CreateInstance(month, year);
            return new Validator(dateProvider).Validate(instance);
        }

        private static ExpiryDate CreateInstance(int month, int year)
        {
            var instance = new ExpiryDate
            {
                Month = month,
                Year  = year
            };
            return instance;
        }

        public int Month { get; private set; }
        public int Year  { get; private set; }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Month;
            yield return Year;
        }

        private class Validator : AbstractValidator<ExpiryDate>
        {
            public Validator(IProvideDateTime dateProvider)
            {
                var now = dateProvider.UtcNow();

                RuleFor(c => c.Month).InclusiveBetween(1, 12)
                                     .WithMessage("Please specify a valid month number between 1 and 12");

                RuleFor(c => c.Year).InclusiveBetween(now.Year, now.Year + 10)
                                    .WithMessage("The expiry year is not valid");

                RuleFor(c => c.Month).GreaterThanOrEqualTo(now.Month)
                                     .When(c => c.Year == now.Year)
                                     .WithMessage("The expiry date is outdated");
            }
        }
    }
}
