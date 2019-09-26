using System;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using PaymentGateway.Domain.Values;
using PaymentGateway.SharedKernel.Date;
using Xunit;

namespace PaymentGateway.Domain.Tests
{
    public class ExpiryDateTests
    {
        private readonly IProvideDateTime _dateProvider;

        public ExpiryDateTests()
        {
            var today = new DateTimeOffset(new DateTime(2019, 08, 19, 0, 0, 0, DateTimeKind.Utc));
            _dateProvider = Substitute.For<IProvideDateTime>();

            _dateProvider.UtcNow().Returns(today);
        }

        [Fact]
        public void Should_raise_an_error_if_year_is_before_current_year()
        {
            Action action = () => ExpiryDate.Create(08, 2018, _dateProvider);

            action.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Should_raise_an_error_if_month_and_year_are_before_today()
        {
            Action action = () => ExpiryDate.Create(07, 2019, _dateProvider);

            action.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Should_create_an_instance_if_month_and_year_are_same_of_today()
        {
            var expiryDate = ExpiryDate.Create(08, 2019, _dateProvider);

            expiryDate.Should().NotBeNull();
        }

        [Fact]
        public void Should_be_equal_to_another_instance_if_they_have_same_date()
        {
            var expiryDateA = ExpiryDate.Create(08, 2019, _dateProvider);
            var expiryDateB = ExpiryDate.Create(08, 2019, _dateProvider);

            expiryDateA.Should().Be(expiryDateB);
        }

        [Fact]
        public void Should_not_be_equal_to_another_instance_if_they_dont_have_same_date()
        {
            var expiryDateA = ExpiryDate.Create(08, 2019, _dateProvider);
            var expiryDateB = ExpiryDate.Create(09, 2019, _dateProvider);

            expiryDateA.Should().NotBe(expiryDateB);
        }
    }
}
