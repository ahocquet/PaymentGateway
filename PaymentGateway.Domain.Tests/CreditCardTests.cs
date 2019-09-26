using System;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using PaymentGateway.Domain.Values;
using PaymentGateway.SharedKernel.Date;
using Xunit;

namespace PaymentGateway.Domain.Tests
{
    public class CreditCardTests
    {
        private readonly CardNumber       _cardNumber;
        private readonly Ccv              _ccv;
        private readonly ExpiryDate       _expiryDate;
        private readonly IProvideDateTime _dateProvider;

        public CreditCardTests()
        {
            // Substitutes
            var today = new DateTimeOffset(new DateTime(2019, 08, 19, 0, 0, 0, DateTimeKind.Utc));
            _dateProvider = Substitute.For<IProvideDateTime>();
            _dateProvider.UtcNow().Returns(today);

            // Shared objects
            _cardNumber = CardNumber.Create("4111 1111 1111 1111");
            _ccv        = Ccv.Create(123);
            _expiryDate = ExpiryDate.Create(09, 2019, _dateProvider);
        }

        [Fact]
        public void Should_raise_an_error_when_expiry_date_is_null()
        {
            Action action = () => CreditCard.Create(_cardNumber, null, _ccv);

            action.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Should_raise_an_error_when_card_number_is_null()
        {
            Action action = () => CreditCard.Create(null, _expiryDate, _ccv);

            action.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Should_raise_an_error_when_ccv_is_null()
        {
            Action action = () => CreditCard.Create(_cardNumber, _expiryDate, null);

            action.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Should_create_an_instance_when_required_parameters_are_set()
        {
            var creditCard = CreditCard.Create(_cardNumber, _expiryDate, _ccv);

            creditCard.Should().NotBeNull();
        }

        [Fact]
        public void Should_be_equal_to_another_instance_if_they_have_same_details()
        {
            var cardNumberA = CardNumber.Create("4111 1111 1111 1111");
            var ccvA        = Ccv.Create(123);
            var expiryDateA = ExpiryDate.Create(09, 2019, _dateProvider);
            var creditCardA = CreditCard.Create(cardNumberA, expiryDateA, ccvA);


            var cardNumberB = CardNumber.Create("4111 1111 1111 1111");
            var ccvB        = Ccv.Create(123);
            var expiryDateB = ExpiryDate.Create(09, 2019, _dateProvider);
            var creditCardB = CreditCard.Create(cardNumberB, expiryDateB, ccvB);

            creditCardA.Should().Be(creditCardB);
        }

        [Fact]
        public void Should_be_equal_to_another_instance_if_they_do_not_have_same_details()
        {
            var cardNumberA = CardNumber.Create("4111 1111 1111 1111");
            var ccvA        = Ccv.Create(123);
            var expiryDateA = ExpiryDate.Create(09, 2019, _dateProvider);
            var creditCardA = CreditCard.Create(cardNumberA, expiryDateA, ccvA);


            var cardNumberB = CardNumber.Create("4111 1111 1111 1111");
            var ccvB        = Ccv.Create(123);
            var expiryDateB = ExpiryDate.Create(09, 2020, _dateProvider); // Diff is here
            var creditCardB = CreditCard.Create(cardNumberB, expiryDateB, ccvB);

            creditCardA.Should().NotBe(creditCardB);
        }
    }
}
