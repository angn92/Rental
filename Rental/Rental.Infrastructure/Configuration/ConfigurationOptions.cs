namespace Rental.Infrastructure.Configuration
{
    public sealed class ConfigurationOptions
    {
        public bool SendRealEmail { get; set; }
        public string EmailAddress { get; set; }
    }
}
