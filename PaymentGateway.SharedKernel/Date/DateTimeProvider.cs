using System;

namespace PaymentGateway.SharedKernel.Date
{
    public class DateTimeProvider : IProvideDateTime
    {
        public DateTimeOffset Now()
        {
            return DateTimeOffset.Now;
        }

        public DateTimeOffset UtcNow()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}