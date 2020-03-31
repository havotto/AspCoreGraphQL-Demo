using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AspCoreGraphQL;
using AspCoreGraphQL_Demo.GraphQL.Types;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.IdentityModel.Tokens;

namespace AspCoreGraphQL_Demo.GraphQL.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    public class UserMutations
    {
        public async Task<LoginPayload> Login(LoginInput input)
        {
            var oneUser = new { username = "user", password = "pass" };
            
            if (input.Username == oneUser.username && input.Password == oneUser.password)
            {
                var identity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, oneUser.username),
                });

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = identity,
                    Expires = DateTime.UtcNow.AddHours(12),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Startup.SharedSecret),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                string tokenString = tokenHandler.WriteToken(token);

                return new LoginPayload(tokenString, "bearer");
            }

            throw new QueryException(
                ErrorBuilder.New()
                        .SetMessage("The specified username or password are invalid.")
                        .SetCode("INVALID_CREDENTIALS")
                        .Build());
        }
    }
}