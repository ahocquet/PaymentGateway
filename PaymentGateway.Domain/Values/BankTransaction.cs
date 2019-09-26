using System.Collections.Generic;
using FluentValidation;
using PaymentGateway.EventSourcing.Core;

namespace PaymentGateway.Domain.Values
{
    public class BankTransaction : ValueObject
    {
        private static readonly BankTransactionValidator Validator = new BankTransactionValidator();

        public BankTransactionId     Id        { get; private set; }
        public BankTransactionStatus Status    { get; private set; }
        public PaymentId             PaymentId { get; private set; }

        private BankTransaction()
        {
        }

        public static BankTransaction Create(BankTransactionId id, BankTransactionStatus status, PaymentId merchantId)
        {
            var instance = new BankTransaction
            {
                Id        = id,
                Status    = status,
                PaymentId = merchantId
            };

            Validator.ValidateAndThrow(instance);
            return instance;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
            yield return Status;
            yield return PaymentId;
        }

        private class BankTransactionValidator : AbstractValidator<BankTransaction>
        {
            public BankTransactionValidator()
            {
                RuleFor(c => c.Id).NotNull().WithName("Bank Transaction Id");
                RuleFor(c => c.Status).IsInEnum().WithName("Bank Transaction Status");
                RuleFor(c => c.PaymentId).NotNull().WithName("Payment Id");
            }
        }
    }
}
