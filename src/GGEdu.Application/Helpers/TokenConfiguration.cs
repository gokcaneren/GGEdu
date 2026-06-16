using Microsoft.Extensions.Configuration;

namespace GGEdu.Application.Helpers
{
    public class TokenConfiguration
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryTime { get; set; }
        public string SecretKey { get; set; }

        public TokenConfiguration(IConfiguration configuration)
        {
            Issuer = configuration["TokenConfiguration:Issuer"]!;
            Audience = configuration["TokenConfiguration:Audience"]!;
            ExpiryTime = Convert.ToInt32(configuration["TokenConfiguration:ExpiryTime"]);
            SecretKey = configuration["TokenConfiguration:SecretKey"]!;
        }
    }
}
