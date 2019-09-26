using System;
using FluentAssertions;
using FluentValidation;
using PaymentGateway.Domain.Values;
using Xunit;

namespace PaymentGateway.Domain.Tests
{
    public class MoneyTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_raise_an_error_when_the_amount_is_lesser_than_0(int amount)
        {
            Action action = () => Money.Create(amount, Currency.EUR);

            action.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Should_create_an_instance_when_the_amount_is_greater_than_0()
        {
            var instance = Money.Create(1, Currency.EUR);

            instance.Should().NotBeNull();
        }

        [Fact]
        public void Should_have_a_valid_currency()
        {
            Action action = () => Money.Create(42, (Currency)1337);

            action.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Should_be_equal_to_another_instance_if_they_have_same_amount()
        {
            var moneyA = Money.Create(123, Currency.EUR);
            var moneyB = Money.Create(123, Currency.EUR);

            moneyA.Should().Be(moneyB);
        }

        [Fact]
        public void Should_not_be_equal_to_another_instance_if_they_dont_have_same_amount()
        {
            var moneyA = Money.Create(123, Currency.EUR);
            var moneyB = Money.Create(321, Currency.EUR);

            moneyA.Should().NotBe(moneyB);
        }
    }
}
