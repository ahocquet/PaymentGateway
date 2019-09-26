using System;
using FluentAssertions;
using FluentValidation;
using PaymentGateway.Domain.Values;
using Xunit;

namespace PaymentGateway.Domain.Tests
{
    public class BankTransactionTests
    {
        private readonly PaymentId         _paymentId;
        private readonly BankTransactionId _transactionId;

        public BankTransactionTests()
        {
            _paymentId     = PaymentId.Create();
            _transactionId = new BankTransactionId(Guid.NewGuid());
        }

        [Fact]
        public void Should_raise_an_error_when_status_has_incorrect_value()
        {
            Action action = () => BankTransaction.Create(_transactionId, (BankTransactionStatus) 1337, _paymentId);

            action.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Should_raise_an_error_when_transaction_id_is_null()
        {
            Action action = () => BankTransaction.Create(null, BankTransactionStatus.Success, _paymentId);

            action.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Should_raise_an_error_when_payment_id_is_null()
        {
            Action action = () => BankTransaction.Create(_transactionId, BankTransactionStatus.Success, null);

            action.Should().Throw<ValidationException>();
        }
    }
}
