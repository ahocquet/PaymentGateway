using PaymentGateway.Api.Dto;
using PaymentGateway.Api.Features.Payment;
using PaymentGateway.Domain.Values;

namespace PaymentGateway.Api.Tests
{
    public static class SubmitCommandFactory
    {
        public static Submit.Command CreateValidCommand()
        {
            var command = new Submit.Command
            {
                CardNumber = "4111 1111 1111 1111",
                Ccv        = 123,
                ExpiryDate = new ExpiryDateDto {Year = 2025, Month  = 03},
                Money      = new MoneyDto {Amount    = 10, Currency = Currency.EUR}
            };

            return command;
        }
    }
}
