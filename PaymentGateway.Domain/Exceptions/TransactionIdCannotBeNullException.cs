using System;
using PaymentGateway.Domain.Values;

namespace PaymentGateway.Domain.Exceptions
{
    public class TransactionIdCannotBeNullException : Exception
    {
        public TransactionIdCannotBeNullException(PaymentId id)
            : base($"The transaction id is required. Payment id: {id.IdAsString()}")
        {
        }
    }
}