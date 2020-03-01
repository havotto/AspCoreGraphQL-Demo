using AspCoreGraphQL.GQL.GqlQueries;
using GraphQL;
using GraphQL.Types;

namespace AspCoreGraphQL.GQL.GqlSchema
{
    public class AppSchema : Schema
    {
        public AppSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<AppQuery>();
            Mutation = resolver.Resolve<AppMutation>();
        }
    }
}