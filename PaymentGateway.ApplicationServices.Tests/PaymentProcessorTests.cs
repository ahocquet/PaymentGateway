using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using PaymentGateway.BankProvider.ACL;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Exceptions;
using PaymentGateway.Domain.Values;
using PaymentGateway.DomainServices;
using PaymentGateway.EventSourcing.Core;
using PaymentGateway.SharedKernel.Date;
using PaymentGateway.TestKernel;
using Xunit;

namespace PaymentGateway.ApplicationServices.Tests
{
    public class PaymentProcessorTests
    {
        private readonly IRepository<Payment, PaymentId> _paymentRepo;
        private readonly PaymentId                       _paymentId;
        private readonly IPaymentService                 _paymentService;
        private readonly IAcquiringBank                  _bank;

        public PaymentProcessorTests()
        {
            var today        = new DateTimeOffset(new DateTime(2019, 08, 19, 0, 0, 0, DateTimeKind.Utc));
            var dateProvider = Substitute.For<IProvideDateTime>();
            dateProvider.UtcNow().Returns(today);

            var payment = PaymentFactory.Create(dateProvider);
            _paymentId = payment.Id;

            _paymentService = Substitute.For<IPaymentService>();
            _bank           = Substitute.For<IAcquiringBank>();
            _paymentRepo    = Substitute.For<IRepository<Payment, PaymentId>>();
            _paymentRepo.Get(Arg.Is(_paymentId)).Returns(payment);
        }

        [Fact]
        public void Should_throw_when_payment_has_already_been_processed()
        {
            // Arrange
            var service = new PaymentProcessor(_paymentRepo, _paymentService, _bank, new DateTimeProvider());
            _paymentService.HasAlreadyBeenProcessed(Arg.Any<Payment>()).Returns(true);

            // Act
            Func<Task> func = () => service.ProcessPayment(_paymentId);

            func.Should().Throw<PaymentAlreadyProcessedException>();
        }

        [Fact]
        public void Should_throw_when_payment_id_is_null()
        {
            // Arrange
            var service = new PaymentProcessor(_paymentRepo, _paymentService, _bank, new DateTimeProvider());

            // Act
            Func<Task> func = () => service.ProcessPayment(null);

            func.Should().Throw<ArgumentNullException>();
        }
    }
}
