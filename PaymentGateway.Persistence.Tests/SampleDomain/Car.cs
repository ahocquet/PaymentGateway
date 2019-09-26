using PaymentGateway.EventSourcing.Core.Aggregate;
using PaymentGateway.EventSourcing.Core.Event;

namespace PaymentGateway.Persistence.Tests.SampleDomain
{
    public class Car : AggregateRootEntity<CarId>
    {
        // Parameterless constructor required for persistence in repository
        // ReSharper disable once UnusedMember.Local
        private Car()
        {
        }

        public Car(CarId id)
        {
            RaiseEvent(new CarCreated(id));
        }

        public Distance TraveledDistance { get; private set; }

        public void Travel(int kilometers)
        {
            RaiseEvent(new CarHasTraveledADistance(Id, kilometers));
        }

        internal void Apply(CarCreated @event)
        {
            Id               = @event.Id;
            TraveledDistance = Distance.Empty();
        }

        internal void Apply(CarHasTraveledADistance @event)
        {
            TraveledDistance = TraveledDistance.Add(Distance.FromKilometers(@event.DistanceInKilometers));
        }
    }

    public class CarHasTraveledADistance : DomainEvent<CarId>
    {
        public readonly int DistanceInKilometers;

        public CarHasTraveledADistance(CarId id, int distanceInKilometers) : base(id)
        {
            DistanceInKilometers = distanceInKilometers;
        }
    }

    public class CarCreated : DomainEvent<CarId>
    {
        public readonly CarId Id;

        public CarCreated(CarId id) : base(id)
        {
            Id = id;
        }
    }
}
