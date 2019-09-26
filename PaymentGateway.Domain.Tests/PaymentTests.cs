using System;
using FluentAssertions;
using NSubstitute;
using PaymentGateway.Domain.Events;
using PaymentGateway.Domain.Exceptions;
using PaymentGateway.Domain.Tests.Extensions;
using PaymentGateway.Domain.Values;
using PaymentGateway.SharedKernel.Date;
using PaymentGateway.TestKernel;
using Xunit;

namespace PaymentGateway.Domain.Tests
{
    public class PaymentTests
    {
        private readonly IProvideDateTime _dateProvider;

        public PaymentTests()
        {
            var today = new DateTimeOffset(new DateTime(2019, 08, 19, 0, 0, 0, DateTimeKind.Utc));
            _dateProvider = Substitute.For<IProvideDateTime>();
            _dateProvider.UtcNow().Returns(today);
        }

        [Fact]
        public void Should_raise_an_PaymentRequested_event_when_a_new_aggregate_instance_is_created()
        {
            // Act
            var aggregate = PaymentFactory.Create(_dateProvider);

            // Assert
            aggregate.Should().HaveRaisedEventOfType<PaymentRequested>();
            aggregate.Should().HaveEventMatching<PaymentRequested>(@event => @event.Id == aggregate.Id);
            aggregate.Should().HaveEventMatching<PaymentRequested>(@event => @event.CreditCard == aggregate.CreditCard);
            aggregate.Should().HaveEventMatching<PaymentRequested>(@event => @event.Amount == aggregate.Amount);
        }

        [Fact]
        public void Should_have_an_initial_status_set_to_pending()
        {
            // Act
            var aggregate = PaymentFactory.Create(_dateProvider);

            // Assert
            aggregate.Status.Should().Be(PaymentStatus.Pending);
        }

        [Fact]
        public void Should_raise_a_PaymentApproved_event_when_Approve_command_is_executed()
        {
            // Arrange
            var aggregate = PaymentFactory.Create(_dateProvider);

            // Act
            aggregate.Approve(BankTransactionId.Create());

            // Assert
            aggregate.Should().HaveRaisedEventOfType<PaymentApproved>();
        }

        [Fact]
        public void Should_set_the_status_to_approved_when_Approve_command_is_executed()
        {
            // Arrange
            var aggregate     = PaymentFactory.Create(_dateProvider);
            var transactionId = BankTransactionId.Create();

            // Act
            aggregate.Approve(transactionId);

            // Assert
            aggregate.Status.Should().Be(PaymentStatus.Approved);
            aggregate.TransactionId.Should().Be(transactionId);
        }

        [Fact]
        public void Should_raise_a_PaymentRejected_event_when_Reject_command_is_executed()
        {
            // Arrange
            var aggregate     = PaymentFactory.Create(_dateProvider);
            var transactionId = BankTransactionId.Create();

            // Act
            aggregate.Reject(transactionId);

            // Assert
            aggregate.Should().HaveRaisedEventOfType<PaymentRejected>();
            aggregate.TransactionId.Should().Be(transactionId);
        }

        [Fact]
        public void Should_set_the_status_to_rejected_when_Reject_command_is_executed()
        {
            // Arrange
            var aggregate = PaymentFactory.Create(_dateProvider);

            // Act
            aggregate.Reject(BankTransactionId.Create());

            // Assert
            aggregate.Status.Should().Be(PaymentStatus.Rejected);
        }

        [Fact]
        public void Should_throw_when_Approve_is_called_and_status_is_not_pending()
        {
            // Arrange
            var aggregate = PaymentFactory.Create(_dateProvider);
            aggregate.Reject(BankTransactionId.Create());

            // Act
            Action action = () => aggregate.Approve(BankTransactionId.Create());

            // Assert
            action.Should().Throw<PaymentAlreadyProcessedException>();
        }

        [Fact]
        public void Should_throw_when_Reject_is_called_and_status_is_not_pending()
        {
            // Arrange
            var aggregate = PaymentFactory.Create(_dateProvider);
            aggregate.Reject(BankTransactionId.Create());

            // Act
            Action action = () => aggregate.Reject(BankTransactionId.Create());

            // Assert
            action.Should().Throw<PaymentAlreadyProcessedException>();
        }


        [Fact]
        public void Should_throw_when_the_transaction_id_null()
        {
            // Arrange
            var aggregate = PaymentFactory.Create(_dateProvider);

            // Act
            Action action = () => aggregate.Reject(null);

            // Assert
            action.Should().Throw<TransactionIdCannotBeNullException>();
        }
    }
}
