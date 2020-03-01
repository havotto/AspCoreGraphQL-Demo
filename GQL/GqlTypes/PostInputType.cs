using GraphQL.Types;

namespace AspCoreGraphQL.GQL.GqlTypes
{
    public class PostInputType:InputObjectGraphType
    {
        public PostInputType()
        {
            Name="postInput";
            Field<NonNullGraphType<StringGraphType>>("title");
            Field<NonNullGraphType<StringGraphType>>("text");
        }
    }
}