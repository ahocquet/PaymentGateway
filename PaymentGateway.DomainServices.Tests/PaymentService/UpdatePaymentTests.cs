using System;
using FluentAssertions;
using NSubstitute;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Exceptions;
using PaymentGateway.Domain.Values;
using PaymentGateway.SharedKernel.Date;
using PaymentGateway.TestKernel;
using PaymentGateway.TestKernel.Helpers;
using Xunit;

namespace PaymentGateway.DomainServices.Tests.PaymentService
{
    public class UpdatePaymentTests
    {
        private readonly Payment                 _payment;
        private readonly DomainServices.PaymentService _service;

        public UpdatePaymentTests()
        {
            var today = new DateTimeOffset(new DateTime(2019, 08, 19, 0, 0, 0, DateTimeKind.Utc));

            var dateProvider = Substitute.For<IProvideDateTime>();
            dateProvider.UtcNow().Returns(today);

            _payment = PaymentFactory.Create(dateProvider);
            _service = new DomainServices.PaymentService();
        }

        [Fact]
        public void Should_approve_payment_when_transaction_status_is_success()
        {
            // Arrange
            var transactionId = new BankTransactionId(Guid.NewGuid());
            var transaction   = BankTransaction.Create(transactionId, BankTransactionStatus.Success, _payment.Id);

            // Act
            _service.UpdatePayment(_payment, transaction);

            // Assert
            _payment.Status.Should().Be(PaymentStatus.Approved);
        }

        [Fact]
        public void Should_reject_payment_when_transaction_status_is_failed()
        {
            // Arrange
            var transactionId = new BankTransactionId(Guid.NewGuid());
            var transaction   = BankTransaction.Create(transactionId, BankTransactionStatus.Failed, _payment.Id);

            // Act
            _service.UpdatePayment(_payment, transaction);

            // Assert
            _payment.Status.Should().Be(PaymentStatus.Rejected);
        }

        [Fact]
        public void Should_throw_when_payment_is_null()
        {
            // Arrange
            var transactionId = new BankTransactionId(Guid.NewGuid());
            var transaction   = BankTransaction.Create(transactionId, BankTransactionStatus.Failed, _payment.Id);

            // Act
            Action action = () => _service.UpdatePayment(null, transaction);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_throw_when_transaction_is_null()
        {
            // Act
            Action action = () => _service.UpdatePayment(_payment, null);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_throw_when_bank_transaction_status_is_unknown()
        {
            // Arrange
            var transactionId = new BankTransactionId(Guid.NewGuid());
            var transaction   = BankTransaction.Create(transactionId, BankTransactionStatus.Failed, _payment.Id);
            ReflectionHelper.UpdatePrivateProperty(transaction, nameof(transaction.Status), 1337);

            // Act
            Action action = () => _service.UpdatePayment(_payment, transaction);

            // Assert
            action.Should().Throw<UnknownBankStatusException>();
        }
    }
}
