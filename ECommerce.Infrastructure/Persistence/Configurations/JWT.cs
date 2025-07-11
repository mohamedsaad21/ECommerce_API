namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public class JWT
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public decimal DurationInDays { get; set; }
    }
}
