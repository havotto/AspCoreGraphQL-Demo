using AspCoreGraphQL.Entities;
using GraphQL.Types;

namespace AspCoreGraphQL.GQL.GqlTypes
{
    public class CommentType : ObjectGraphType<Comment>
    {
        public CommentType()
        {
            Field(c => c.Id, type: typeof(IdGraphType)).Description("Comment ID");
            Field(c => c.Text).Description("Comment text");
            Field(c => c.PostId, type:typeof(IdGraphType)).Description("Comment postId");
        }
    }
}