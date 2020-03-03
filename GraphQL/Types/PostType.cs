using System;
using AspCoreGraphQL.Entities;
using HotChocolate.Types;

namespace AspCoreGraphQL.GraphQL.Types
{
    public class PostType : ObjectType<Post>
    {
        protected override void Configure(IObjectTypeDescriptor<Post> descriptor)
        {
            descriptor.Field(p => p.Id).Type<NonNullType<IdType>>().Name("id");
            descriptor.Field(p => p.Title).Type<NonNullType<StringType>>();
            descriptor.Field(p => p.Text).Type<NonNullType<StringType>>();
        }
    }
}