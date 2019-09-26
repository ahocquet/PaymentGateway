namespace PaymentGateway.EventSourcing.Core.Aggregate
{
    public interface IAggregateId
    {
        string IdAsString();
    }
}