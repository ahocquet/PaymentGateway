using System;
using PaymentGateway.Domain.Values;

namespace PaymentGateway.Domain.Exceptions
{
    public class PaymentAlreadyProcessedException: Exception
    {
        public PaymentAlreadyProcessedException(PaymentId id) : base($"The payment [{id.IdAsString()}] has already been processed")
        {
        }
    }
}