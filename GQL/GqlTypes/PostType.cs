using AspCoreGraphQL.Entities;
using GraphQL.Types;

namespace AspCoreGraphQL.GQL.GqlTypes
{
    public class PostType : ObjectGraphType<Post>
    {
        public PostType()
        {
            this.Field(p => p.Id, type: typeof(IdGraphType)).Description("Post ID");
            this.Field(p => p.Title).Description("Post title");
            this.Field(p => p.Text).Description("Post text");
        }
    }
}