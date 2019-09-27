using PaymentGateway.Api.Client;

namespace PaymentGateway.Api.IntegrationTests.Payment
{
    public static class SubmitCommandFactory
    {
        public static Command CreateValidCommand()
        {
            var command = new Command
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
