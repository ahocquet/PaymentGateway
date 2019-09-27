namespace PaymentGateway.SharedKernel.Models
{
    public class AppSettings
    {
        public ApplicationInsights ApplicationInsights { get; set; }
        public StorageSettings     Storage             { get; set; }
    }

    public class ApplicationInsights
    {
        public string InstrumentationKey { get; set; }
    }

    public class StorageSettings
    {
        public string ConnectionString { get; set; }
    }
}
