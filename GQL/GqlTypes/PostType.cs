using System.Linq;
using AspCoreGraphQL.Entities;
using AspCoreGraphQL.Entities.Context;
using GraphQL.DataLoader;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace AspCoreGraphQL.GQL.GqlTypes
{
    public class PostType : ObjectGraphType<Post>
    {
        public PostType(DataContext db, IDataLoaderContextAccessor dataLoader)
        {
            this.Field(p => p.Id, type: typeof(IdGraphType)).Description("Post ID");
            this.Field(p => p.Title).Description("Post title");
            this.Field(p => p.Text).Description("Post text");
            Field<PostTypeEnumType>(nameof(Post.Type));
            this.Field<ListGraphType<CommentType>>(
                "comments",
                resolve: context =>
                {
                    var loader = dataLoader.Context.GetOrAddCollectionBatchLoader<int, Comment>("postComments", async keys =>
                    {
                        var comments = await db.Comments.Where(c => keys.Contains(c.PostId)).ToListAsync();
                        return comments.ToLookup(c => c.PostId);
                    });
                    return loader.LoadAsync(context.Source.Id);
                });
        }
    }
}