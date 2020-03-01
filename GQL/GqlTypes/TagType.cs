using AspCoreGraphQL.Entities;
using GraphQL.Types;

namespace AspCoreGraphQL.GQL.GqlTypes
{
    public class TagType : ObjectGraphType<Tag>
    {
        public TagType()
        {
            Field(c => c.Id, type: typeof(IdGraphType)).Description("Tag ID");
            Field(c => c.Name).Description("Tag name");
        }
    }
}        
