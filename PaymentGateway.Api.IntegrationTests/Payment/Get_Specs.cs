using System;
using System.Threading.Tasks;
using FluentAssertions;
using PaymentGateway.Api.Client;
using PaymentGateway.Api.IntegrationTests.Fixtures;
using Xunit;

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
            var command = SubmitCommandFactory.CreateValidCommand();

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
