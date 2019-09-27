using System;
using System.Threading.Tasks;
using FluentAssertions;
using PaymentGateway.Api.Client;
using PaymentGateway.Api.IntegrationTests.Fixtures;
using Xunit;
using Currency = PaymentGateway.Api.Client.Currency;

namespace PaymentGateway.Api.IntegrationTests.Payment
{
    [Collection(nameof(DependencyInjectionCollection))]
    // ReSharper disable once InconsistentNaming
    public class Retrieving_a_payment_details
    {
        private readonly PaymentGatewayClient _client;

        public Retrieving_a_payment_details(DependencyInjectionFixture diFixture)
        {
            _client = new PaymentGatewayClient(diFixture.HttpClient);
        }

        [Fact]
        public async Task Should_return_a_response_with_a_payment_view()
        {
            // Arrange
            var command = new Command
            {
                CardNumber = "4111 1111 1111 1111",
                Ccv        = 123,
                ExpiryDate = new ExpiryDateDto {Year = 2025, Month  = 03},
                Money      = new MoneyDto {Amount    = 10, Currency = Currency.EUR}
            };

            // Act
            var response    = await _client.Payment_SubmitAsync(command);
            var paymentView = await _client.Payment_GetAsync(Guid.Parse(response.Id));

            // Assert
            paymentView.Should().NotBeNull();
            paymentView.Id.Should().Be(response.Id);
            paymentView.Status.Should().Be(response.Status);
            paymentView.CardNumber.Should().EndWith("1111");
            paymentView.Amount.Should().Be(command.Money.Amount);
            paymentView.Currency.Should().Be(command.Money.Currency.ToString());
        }
    }
}
