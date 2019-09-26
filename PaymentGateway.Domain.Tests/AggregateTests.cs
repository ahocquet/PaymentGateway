using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using PaymentGateway.Domain.Entities;
using PaymentGateway.EventSourcing.Core.Aggregate;
using Xunit;

namespace PaymentGateway.Domain.Tests
{
    public class AggregateTests
    {
        private readonly IEnumerable<Type> _aggregateTypes;

        public AggregateTests()
        {
            _aggregateTypes = typeof(Payment).Assembly
                                             .GetTypes()
                                             .Where(t => t.BaseType != null && t.BaseType.IsGenericType)
                                             .Where(t => typeof(AggregateRootEntity<>).IsAssignableFrom(t.BaseType.GetGenericTypeDefinition()))
                                             .ToArray();
        }

        /// <summary>
        /// For deserialization purpose, all aggregate root need a parameter-less constructor
        /// </summary>
        [Fact]
        public void Aggregate_root_should_have_a_parameterless_constructor()
        {
            foreach (var type in _aggregateTypes)
            {
                var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                       .Where(t => t.GetParameters().Length == 0);
                constructors.Should().HaveCount(1);
            }
        }


        [Fact]
        public void Value_objects_properties_should_have_private_setter_only()
        {
            foreach (var type in _aggregateTypes)
            {
                var writableProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                             .Where(t => t.DeclaringType == type)
                                             .Where(p => p.SetMethod.IsPublic);

                writableProperties.Should().HaveCount(0);
            }
        }

        [Fact]
        public void Value_objects_public_fields_should_be_readonly_or_const()
        {
            foreach (var type in _aggregateTypes)
            {
                var writableProperties = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
                                             .Where(t => t.DeclaringType == type)
                                             .Where(p => !p.IsInitOnly && !p.IsLiteral);

                writableProperties.Should().HaveCount(0);
            }
        }
    }
}
