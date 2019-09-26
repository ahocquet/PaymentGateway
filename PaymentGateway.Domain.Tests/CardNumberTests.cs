using System;
using FluentAssertions;
using FluentValidation;
using PaymentGateway.Domain.Values;
using Xunit;

namespace PaymentGateway.Domain.Tests
{
    public class CardNumberTests
    {
        [Fact]
        public void Should_raise_an_error_when_the_card_number_has_alpha_char()
        {
            Action action = () => CardNumber.Create("4111 A111 1111 1111");

            action.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Should_replace_spaces_in_credit_card_number()
        {
            var cardNumber = CardNumber.Create("4111 1111 1111 1111");

            cardNumber.Value.Should().NotContain(" ");
        }

        [Fact]
        public void Should_create_an_instance_when_card_number_is_correct()
        {
            var cardNumber = CardNumber.Create("4111111111111111");

            cardNumber.Should().NotBeNull();
        }

        [Fact]
        public void Should_be_equal_to_another_instance_if_they_have_same_number()
        {
            var cardNumberA = CardNumber.Create("4111111111111111");
            var cardNumberB = CardNumber.Create("4111 1111 1111 1111");

            cardNumberA.Should().Be(cardNumberB);
        }

        [Fact]
        public void Should_not_be_equal_to_another_instance_if_they_dont_have_same_number()
        {
            var cardNumberA = CardNumber.Create("5500 0000 0000 0004");
            var cardNumberB = CardNumber.Create("4111 1111 1111 1111");

            cardNumberA.Should().NotBe(cardNumberB);
        }
    }
}
