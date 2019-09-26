using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using PaymentGateway.Domain.Values;
using PaymentGateway.EventSourcing.Core;
using Xunit;

namespace PaymentGateway.Domain.Tests
{
    /// <summary>
    /// We want to prevent public constructor with validation inside.
    /// Indeed, if a validation is made within a public constructor of a value object,
    /// and that the value object is part af a persisted event,
    /// then a modification to the validation could break the event deserialization!
    /// Therefore, validation is part of a factory static method, and a private constructor
    /// is defined for deserialization.
    /// </summary>
    public class ValueObjectTests
    {
        private readonly IEnumerable<Type> _valueObjectTypes;

        public ValueObjectTests()
        {
            _valueObjectTypes = typeof(CreditCard).Assembly
                                                  .GetTypes()
                                                  .Where(t => typeof(ValueObject).IsAssignableFrom(t));
        }

        [Fact]
        public void Value_objects_should_have_a_private_constructor()
        {
            foreach (var type in _valueObjectTypes)
            {
                var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                constructors.Should().HaveCount(1);
                constructors.Should().Contain(t => t.IsPrivate);
            }
        }

        [Fact]
        public void Value_objects_should_have_a_factory_static_method()
        {
            foreach (var type in _valueObjectTypes)
            {
                var factories = type.GetMembers(BindingFlags.Static | BindingFlags.Public)
                                    .Where(t => t.DeclaringType == type);

                factories.Should().HaveCountGreaterOrEqualTo(1);
            }
        }

        [Fact]
        public void Value_objects_properties_should_have_private_setter_only()
        {
            foreach (var type in _valueObjectTypes)
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
            foreach (var type in _valueObjectTypes)
            {
                var writableProperties = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
                                             .Where(t => t.DeclaringType == type)
                                             .Where(p => !p.IsInitOnly && !p.IsLiteral);

                writableProperties.Should().HaveCount(0);
            }
        }

    }
}
