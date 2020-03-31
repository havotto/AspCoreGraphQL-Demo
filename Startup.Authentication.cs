using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;


namespace AspCoreGraphQL
{

    public partial class Startup
    {
        //must be greater than 16 bytes to token generation work
        internal static byte[] SharedSecret = Encoding.ASCII.GetBytes(
        "myVeryLongSecretKey");

        private void ConfigureAuthenticationServices(IServiceCollection services)
        {
            services.AddAuthentication("jwt")

            .AddJwtBearer("jwt", x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(SharedSecret),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
        }
    }
}