using AspCoreGraphQL.Entities.Context;
using AspCoreGraphQL.GQL.GqlTypes;
using GraphQL.Types;

namespace AspCoreGraphQL.GQL.GqlQueries
{
    public class AppQuery : ObjectGraphType
    {
        public AppQuery(DataContext db)
        {
            Field<ListGraphType<PostType>>("posts", resolve: context=>db.Posts);
        }
    }
}