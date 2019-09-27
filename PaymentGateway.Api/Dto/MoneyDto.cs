using PaymentGateway.Domain.Values;

namespace PaymentGateway.Api.Dto
{
    public class MoneyDto
    {
        public double   Amount   { get; set; }
        public Currency Currency { get; set; }
    }
}
