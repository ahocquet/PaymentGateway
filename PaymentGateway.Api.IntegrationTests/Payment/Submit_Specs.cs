using System.Threading.Tasks;
using FluentAssertions;
using PaymentGateway.Api.Client;
using PaymentGateway.Api.IntegrationTests.Fixtures;
using PaymentGateway.Domain.Values;
using Xunit;

namespace PaymentGateway.Api.IntegrationTests.Payment
{
    [Collection(nameof(DependencyInjectionCollection))]
    // ReSharper disable once InconsistentNaming
    public class Submitting_a_new_payment_request
    {
        private readonly PaymentGatewayClient _client;

        public Submitting_a_new_payment_request(DependencyInjectionFixture diFixture)
        {
            _client = new PaymentGatewayClient(diFixture.HttpClient);
        }

        [Fact]
        public async Task Should_return_a_response_with_an_id_and_a_status()
        {
            // Arrange
            var command = SubmitCommandFactory.CreateValidCommand();

            // Act
            var response = await _client.Payment_SubmitAsync(command);

            // Assert
            response.Id.Should().NotBeNullOrEmpty();
            response.Status.Should().BeOneOf(PaymentStatus.Rejected.ToString(), PaymentStatus.Approved.ToString());
        }
    }
}
