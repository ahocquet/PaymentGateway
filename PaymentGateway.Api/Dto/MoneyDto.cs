using PaymentGateway.Domain.Values;

namespace PaymentGateway.Api.Dto
{
    public class MoneyDto
    {
        public decimal  Amount   { get; set; }
        public Currency Currency { get; set; }
    }
}