using System.Collections.Generic;
using HotChocolate.Configuration;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors.Definitions;

namespace AspCoreGraphQL.GraphQL.Directives
{
    /// <summary>
    /// Can be used like this:
    /// {
    ///  posts {
    ///    id
    ///    text  @append
    ///    publishedAt
    ///  }
    /// }
    /// </summary>
    public class AppenderDirectiveType : DirectiveType
    {
        protected override void Configure(IDirectiveTypeDescriptor descriptor)
        {
            descriptor.Name("append");
            descriptor.Location(DirectiveLocation.Field);
            descriptor.Use(next => async context =>
            {
                await next(context);
                if (context.Result is string)
                {
                    context.Result += " --";
                }
            });
        }

    }
}