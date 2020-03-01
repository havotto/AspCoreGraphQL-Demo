using AspCoreGraphQL.Entities;
using AspCoreGraphQL.Entities.Context;
using AspCoreGraphQL.GQL.GqlTypes;
using GraphQL.Types;

namespace AspCoreGraphQL.GQL.GqlQueries
{
    public class AppMutation:ObjectGraphType
    {
        public AppMutation(DataContext db)
        {
            Field<PostType>(
                "createPost",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<PostInputType>>{Name="post"}),
                resolve: context => {
                    var post = context.GetArgument<Post>("post");
                    db.Add(post);
                    db.SaveChangesAsync();
                    return post;
                }
            );
        }
    }
}