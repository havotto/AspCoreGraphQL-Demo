using HotChocolate.Types;

namespace AspCoreGraphQL.GraphQL.Types
{
    public class QueryType : ObjectType<Query>
    {

        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Field(q => q.GetPosts())
                .Type<ListType<PostType>>();
        }

    }
}