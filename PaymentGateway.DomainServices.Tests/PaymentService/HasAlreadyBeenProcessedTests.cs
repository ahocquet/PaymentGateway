using System;
using FluentAssertions;
using NSubstitute;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Values;
using PaymentGateway.SharedKernel.Date;
using PaymentGateway.TestKernel;
using Xunit;

namespace PaymentGateway.DomainServices.Tests.PaymentService
{
    public class HasAlreadyBeenProcessedTests
    {
        private readonly Payment _payment;

        public HasAlreadyBeenProcessedTests()
        {
            var today = new DateTimeOffset(new DateTime(2019, 08, 19, 0, 0, 0, DateTimeKind.Utc));

            var dateProvider = Substitute.For<IProvideDateTime>();
            dateProvider.UtcNow().Returns(today);

            _payment = PaymentFactory.Create(dateProvider);
        }

        [Fact]
        public void Should_return_true_when_payment_has_been_approved()
        {
            // Arrange
            var service = new DomainServices.PaymentService();
            _payment.Approve(BankTransactionId.Create());

            // Act
            var result = service.HasAlreadyBeenProcessed(_payment);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Should_return_true_when_payment_has_been_rejected()
        {
            // Arrange
            var service = new DomainServices.PaymentService();
            _payment.Reject(BankTransactionId.Create());

            // Act
            var result = service.HasAlreadyBeenProcessed(_payment);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Should_return_false_when_payment_is_pending()
        {
            // Arrange
            var service = new DomainServices.PaymentService();

            // Act
            var result = service.HasAlreadyBeenProcessed(_payment);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Should_throw_when_payment_is_null()
        {
            // Arrange
            var service = new DomainServices.PaymentService();

            // Act
            Action action = () => service.HasAlreadyBeenProcessed(null);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
