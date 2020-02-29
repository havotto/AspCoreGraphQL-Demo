using AspCoreGraphQL.Entities;
using GraphQL.Types;

namespace AspCoreGraphQL.GQL.GqlTypes
{
    public class PostTypeEnumType : EnumerationGraphType<EPostType>
    {
        public PostTypeEnumType()
        {
            Name = "EPostType";
            Description = "Post type";
        }
    }
}