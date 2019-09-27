using System;
using System.Collections.Generic;
using PaymentGateway.EventSourcing.Core;

namespace PaymentGateway.Persistence.IntegrationTests.SampleDomain
{
    public class Distance : ValueObject
    {
        private readonly int _value;

        private Distance(int distanceInKilometers)
        {
            _value = distanceInKilometers;
        }


        public static Distance FromKilometers(int distanceInKilometers)
        {
            if (distanceInKilometers <= 0)
            {
                throw new Exception("Distance must be a positive number");
            }

            return new Distance(distanceInKilometers);
        }

        public static Distance Empty()
        {
            return new Distance(0);
        }

        public Distance Add(Distance distance)
        {
            return new Distance(_value + distance._value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _value;
        }
    }
}
