using System;
using FluentAssertions;
using FluentValidation;
using PaymentGateway.Domain.Values;
using Xunit;

namespace PaymentGateway.Domain.Tests
{
    public class CcvTests
    {
        [Theory]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(1)]
        [InlineData(99)]
        public void Should_raise_an_error_when_the_ccv_number_does_not_have_3_digits(int number)
        {
            Action action = () => Ccv.Create(number);

            action.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Should_create_an_instance_when_the_ccv_number_has_3_digits()
        {
            var instance = Ccv.Create(123);

            instance.Should().NotBeNull();
        }

        [Fact]
        public void Should_be_equal_to_another_instance_if_they_have_same_number()
        {
            var ccvA = Ccv.Create(123);
            var ccvB = Ccv.Create(123);

            ccvA.Should().Be(ccvB);
        }

        [Fact]
        public void Should_not_be_equal_to_another_instance_if_they_dont_have_same_number()
        {
            var ccvA = Ccv.Create(123);
            var ccvB = Ccv.Create(321);

            ccvA.Should().NotBe(ccvB);
        }
    }
}
