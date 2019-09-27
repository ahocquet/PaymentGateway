using AutoBogus;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Read.Models;
using PaymentGateway.Read.Repositories.Entities;
using PaymentGateway.Read.Repositories.Tests.Fixtures;
using Xunit;

namespace PaymentGateway.Read.Repositories.Tests
{
    [Collection(nameof(DependencyInjectionCollection))]
    public class PaymentViewMappingTests
    {
        private readonly IMapper _mapper;

        public PaymentViewMappingTests(DependencyInjectionFixture diFixture)
        {
            _mapper = diFixture.Container.GetRequiredService<IMapper>();
        }

        [Fact]
        public void Should_map_a_PaymentView_to_a_PaymentEntity()
        {
            // Arrange
            var view = AutoFaker.Generate<PaymentView>();

            // Act
            var entity = _mapper.Map<PaymentEntity>(view);

            // Assert
            entity.PartitionKey.Should().Be(view.Id);
            entity.Should().BeEquivalentTo(view, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void Should_map_a_PaymentEntity_to_a_PaymentView()
        {
            // Arrange
            var entity = AutoFaker.Generate<PaymentEntity>();

            // Act
            var view = _mapper.Map<PaymentView>(entity);

            // Assert
            view.Id.Should().Be(entity.PartitionKey);
            view.Should().BeEquivalentTo(entity, options => options.ExcludingMissingMembers());
        }
    }
}
