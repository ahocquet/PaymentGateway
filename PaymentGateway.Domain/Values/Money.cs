﻿using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using PaymentGateway.EventSourcing.Core;

namespace PaymentGateway.Domain.Values
{
    public class Money : ValueObject
    {
        private static readonly MoneyValidator Validator = new MoneyValidator();

        private Money()
        {
        }

        public static Money Create(double amount, Currency currency)
        {
            var instance = CreateInstance(amount, currency);
            Validator.ValidateAndThrow(instance);

            return instance;
        }

        public static ValidationResult Validate(double amount, Currency currency)
        {
            var instance = CreateInstance(amount, currency);
            return Validator.Validate(instance);
        }

        private static Money CreateInstance(double amount, Currency currency)
        {
            return new Money
            {
                Amount   = amount,
                Currency = currency
            };
        }

        public double   Amount   { get; private set; }
        public Currency Currency { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }

        private class MoneyValidator : AbstractValidator<Money>
        {
            public MoneyValidator()
            {
                RuleFor(c => c.Amount).GreaterThan(0)
                                      .WithMessage("Please specify a valid amount");
                RuleFor(c => c.Currency).IsInEnum();
            }
        }
    }
}
