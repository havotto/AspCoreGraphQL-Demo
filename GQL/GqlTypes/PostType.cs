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
            this.Field(p => p.AverageRating, nullable: true).Description("Post rating");
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
            this.Field<ListGraphType<TagType>>(
                "tags",
                resolve: context =>
                {
                    var loader = dataLoader.Context.GetOrAddCollectionBatchLoader<int, Tag>("postTags", async keys =>
                    {
                        var tags = await
                            (from postTag in db.PostTags
                             from tag in db.Tags
                             where postTag.TagId == tag.Id
                             where keys.Contains(postTag.PostId)
                             select new { PostId = postTag.PostId, Tag = tag })
                             .ToListAsync();

                        return tags.ToLookup(x => x.PostId, x => x.Tag);

                    });
                    return loader.LoadAsync(context.Source.Id);
                });
        }
    }
}