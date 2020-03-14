using AspCoreGraphQL.Entities;
using HotChocolate.Types;

namespace AspCoreGraphQL_Demo.GraphQL.Types
{
    public class PostType : ObjectType<Post>
    {
        protected override void Configure(IObjectTypeDescriptor<Post> descriptor)
        {
            descriptor.Field(_ => _.IgnoredMethod()).Ignore();
        }
    }
}