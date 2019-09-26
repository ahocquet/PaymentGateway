namespace PaymentGateway.EventSourcing.Core.Exception
{
    public class ConcurrencyException : AggregateExceptionBase
    {
        public ConcurrencyException(string message) : base(message)
        {
        }

        public ConcurrencyException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}