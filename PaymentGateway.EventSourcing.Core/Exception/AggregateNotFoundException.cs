namespace PaymentGateway.EventSourcing.Core.Exception
{
    public class AggregateNotFoundException : AggregateExceptionBase
    {
        public AggregateNotFoundException(string aggregateId) : base($"Partition Key {aggregateId} does not exist")
        {
        }
    }
}