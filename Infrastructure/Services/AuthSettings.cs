using Application.Common;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class AuthSettings : IAuthSettings
    {
        private readonly IConfiguration _configuration;

        public AuthSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string DefaultUserRole => _configuration["Auth:DefaultUserRole"] ?? "user";
    }
}
