using System.Linq;
using AspCoreGraphQL.Entities;
using AspCoreGraphQL.Entities.Context;
using AspCoreGraphQL.GQL.GqlTypes;
using GraphQL.Types;

namespace AspCoreGraphQL.GQL.GqlQueries
{
    public class AppMutation : ObjectGraphType
    {
        public AppMutation(DataContext db)
        {
            Field<PostType>(
                "createPost",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<PostInputType>> { Name = "post" }),
                resolve: context =>
                {
                    var post = context.GetArgument<Post>("post");
                    db.Add(post);
                    db.SaveChanges();
                    return post;
                }
            );
            Field<PostType>(
                "updatePost",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "postId" },
                    new QueryArgument<NonNullGraphType<PostInputType>> { Name = "post" }
                ),
                resolve: context =>
                {
                    var postId = context.GetArgument<int>("postId");
                    var post = context.GetArgument<Post>("post");
                    var dbPost = db.Posts.Find(postId);
                    if (dbPost == null)
                    {
                        context.Errors.Add(new GraphQL.ExecutionError($"Could not find post with id: {postId}"));
                        return null;
                    }
                    dbPost.Title = post.Title;
                    dbPost.Text = post.Text;
                    db.SaveChanges();
                    return dbPost;
                }
            );
        }
    }
}