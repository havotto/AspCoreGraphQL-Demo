using System.Linq;
using AspCoreGraphQL.Entities.Context;
using AspCoreGraphQL.GQL.GqlTypes;
using GraphQL;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace AspCoreGraphQL.GQL.GqlQueries
{
    public class AppQuery : ObjectGraphType
    {
        public AppQuery(DataContext db)
        {
            Field<ListGraphType<PostType>>("posts", resolve: context => db.Posts);
            Field<PostType>(
                "post",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "postId" }),
                resolve: context =>
                {
                    int id;
                    var argValue = context.GetArgument<string>("postId");
                    if(!int.TryParse(argValue, out id)){
                        context.Errors.Add(new ExecutionError($"Wrong value for int: {argValue}"));
                    }
                    return db.Posts.Where(p => p.Id == id).SingleOrDefaultAsync();
                }
            );
            Field<ListGraphType<CommentType>>("comments", resolve: context => db.Comments);
        }
    }
}