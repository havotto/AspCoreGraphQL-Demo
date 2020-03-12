using System.Collections.Generic;
using AspCoreGraphQL.Entities;
using HotChocolate.Configuration;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors.Definitions;

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