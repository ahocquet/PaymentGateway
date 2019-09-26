using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using PaymentGateway.EventSourcing.Core.Aggregate;

namespace PaymentGateway.Domain.Tests.Extensions
{
    public class AggregateAssertions<TAggregateId> : ReferenceTypeAssertions<AggregateRootEntity<TAggregateId>, AggregateAssertions<TAggregateId>> where TAggregateId : IAggregateId
    {
        public AggregateAssertions(AggregateRootEntity<TAggregateId> instance)
        {
            Subject    = instance;
            Identifier = instance?.Id?.IdAsString();
        }

        protected override string Identifier { get; }

        public AndConstraint<AggregateAssertions<TAggregateId>> HaveRaisedEventOfType<TEvent>(int? expectedNumberOfEvent = null, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .Given(() => Subject.GetUncommittedChanges())
                   .ForCondition(stream =>
                    {
                        var count = stream.Count(@event => @event.GetType() == typeof(TEvent));
                        if (expectedNumberOfEvent.HasValue)
                            return count == expectedNumberOfEvent;
                        return count > 0;
                    })
                   .FailWith($"Expected to contain event type of {typeof(TEvent).Name}, but couldn't find it.");

            return new AndConstraint<AggregateAssertions<TAggregateId>>(this);
        }

        public AndConstraint<AggregateAssertions<TAggregateId>> HaveEventMatching<TEvent>(Func<TEvent, bool> func, string because = "", params object[] becauseArgs)
        {
            HaveRaisedEventOfType<TEvent>(null, because, becauseArgs);

            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .Given(() => Subject.GetUncommittedChanges()
                                       .Where(@event => @event.GetType() == typeof(TEvent))
                                       .Cast<TEvent>())
                   .ForCondition(events => events.Any(func))
                   .FailWith("Couldn't find any event matching the supplied predicate.");

            return new AndConstraint<AggregateAssertions<TAggregateId>>(this);
        }
    }
}
