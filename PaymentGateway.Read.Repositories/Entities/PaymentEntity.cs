using System.Runtime.CompilerServices;
using Microsoft.Azure.Cosmos.Table;

[assembly: InternalsVisibleTo("PaymentGateway.Read.Repositories.Tests")]

namespace PaymentGateway.Read.Repositories.Entities
{
    internal class PaymentEntity : TableEntity
    {
        public string CardNumber { get; set; }
        public double Amount     { get; set; }
        public string Currency   { get; set; }
        public string Status     { get; set; }
    }
}
