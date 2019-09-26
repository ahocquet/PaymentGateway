using System;

namespace PaymentGateway.SharedKernel.Date
{
    public interface IProvideDateTime
    {
        DateTimeOffset UtcNow();
    }
}
