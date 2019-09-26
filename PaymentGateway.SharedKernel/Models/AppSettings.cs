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

    public class SwaggerSettings
    {
        public string ClientId     { get; set; }
        public string ClientSecret { get; set; }
        public string AppName      { get; set; }
    }
}
