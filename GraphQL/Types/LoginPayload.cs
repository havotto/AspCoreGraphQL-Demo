namespace AspCoreGraphQL_Demo.GraphQL.Types
{
    public class LoginPayload
    {
        public LoginPayload(string token, string scheme)
        {
            Token = token;
            Scheme = scheme;
        }

        public string Token { get; }
        public string Scheme { get; }
    }
}