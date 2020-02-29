using System.Linq;
using AspCoreGraphQL.Entities;
using AspCoreGraphQL.Entities.Context;
using GraphQL.Types;

namespace AspCoreGraphQL.GQL.GqlTypes
{
    public class PostType : ObjectGraphType<Post>
    {
        public PostType(DataContext db)
        {
            this.Field(p => p.Id, type: typeof(IdGraphType)).Description("Post ID");
            this.Field(p => p.Title).Description("Post title");
            this.Field(p => p.Text).Description("Post text");
            this.Field<ListGraphType<CommentType>>("comments", resolve: context=>db.Comments.Where(c=>c.PostId == context.Source.Id));
            Field<PostTypeEnumType>(nameof(Post.Type));
        }
    }
}