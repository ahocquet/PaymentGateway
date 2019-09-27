using Newtonsoft.Json;

namespace PaymentGateway.Read.Models
{
    public class PaymentView
    {
        [JsonIgnore]
        public string ETag { get; set; }

        public string CardNumber { get; set; }
        public string Id         { get; set; }
        public double Amount     { get; set; }
        public string Currency   { get; set; }
        public string Status     { get; set; }
    }
}
