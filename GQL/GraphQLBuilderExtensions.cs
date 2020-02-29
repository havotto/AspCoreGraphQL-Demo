using GraphQL;
using GraphQL.Server;

namespace AspCoreGraphQL.GQL
{
    public static class GraphQLBuilderExtensions
    {
        public static IGraphQLBuilder AddValueConverters(this IGraphQLBuilder @this){
            ValueConverter.Register(typeof(int), typeof(string), intValue => intValue.ToString());
            return @this;
        }
    }
}