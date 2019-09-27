using System;
using FluentAssertions;
using NSubstitute;
using PaymentGateway.Api.Dto;
using PaymentGateway.Api.Features.Payment;
using PaymentGateway.Domain.Values;
using PaymentGateway.SharedKernel.Date;
using Xunit;

namespace PaymentGateway.Api.Tests
{
    public class SubmitCommandValidationTests
    {
        private readonly Submit.Validator _validator;

        public SubmitCommandValidationTests()
        {
            var today        = new DateTimeOffset(new DateTime(2019, 08, 19, 0, 0, 0, DateTimeKind.Utc));
            var dateProvider = Substitute.For<IProvideDateTime>();
            dateProvider.UtcNow().Returns(today);

            _validator = new Submit.Validator(dateProvider);
        }

        [Fact]
        public void Should_validate_successfully_when_the_command_is_valid()
        {
            // Arrange
            var command = SubmitCommandFactory.CreateValidCommand();

            // Act
            var validation = _validator.Validate(command);

            // Assert
            validation.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(2019, 7)]
        [InlineData(0, 7)]
        [InlineData(2020, 0)]
        public void Should_fail_validation_when_the_expiry_date_is_outdated(int year, int month)
        {
            // Arrange
            var command = SubmitCommandFactory.CreateValidCommand();
            command.ExpiryDate = new ExpiryDateDto {Year = year, Month = month};

            // Act
            var validation = _validator.Validate(command);

            // Assert
            validation.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void Should_fail_validation_when_the_amount_is_not_greater_than_zero(int amount)
        {
            // Arrange
            var command = SubmitCommandFactory.CreateValidCommand();
            command.Money = new MoneyDto {Amount = amount, Currency = Currency.EUR};

            // Act
            var validation = _validator.Validate(command);

            // Assert
            validation.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_fail_validation_when_the_currency_is_invalid()
        {
            // Arrange
            var command = SubmitCommandFactory.CreateValidCommand();
            command.Money = new MoneyDto {Amount = 10, Currency = (Currency) 1337};

            // Act
            var validation = _validator.Validate(command);

            // Assert
            validation.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData(42)]
        [InlineData(1337)]
        public void Should_fail_validation_when_the_ccv_is_not_three_digits(int ccv)
        {
            // Arrange
            var command = SubmitCommandFactory.CreateValidCommand();
            command.Ccv = ccv;

            // Act
            var validation = _validator.Validate(command);

            // Assert
            validation.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("1234")]
        [InlineData("4111 A111 1111 1111")]
        public void Should_fail_validation_when_the_card_number_is_not_valid(string cardNumber)
        {
            // Arrange
            var command = SubmitCommandFactory.CreateValidCommand();
            command.CardNumber = cardNumber;

            // Act
            var validation = _validator.Validate(command);

            // Assert
            validation.IsValid.Should().BeFalse();
        }
    }
}
