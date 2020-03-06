using System;
using System.Linq;
using System.Threading;
using AspCoreGraphQL.Entities;
using AspCoreGraphQL.Entities.Context;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AspCoreGraphQL.GraphQL.Resolvers
{
    //Error: A second operation started on this context before a previous operation completed. This is usually caused by different threads using the same instance of DbContext.
    //Resolvers run parallel
    //Maybe this: 
    //  https://github.com/ChilliCream/hotchocolate/issues/635 
    //  https://github.com/graphql-dotnet/graphql-dotnet/issues/863
    //      func<T>:
    //      https://github.com/graphql-dotnet/graphql-dotnet/issues/863#issuecomment-578616371
    //      serial:
    //      https://github.com/graphql-dotnet/graphql-dotnet/issues/863#issuecomment-468981901
    //
    //None of these solved that error when I return IQueryable

    [ExtendObjectType(Name = nameof(Post))]
    public class PostResolvers
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<PostResolvers> logger;

        public PostResolvers([Service] IHttpContextAccessor httpContextAccessor, [Service] ILogger<PostResolvers> logger)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
        }
        public IQueryable<Comment> Comments([Parent]Post post)
        {
            var dbFactory = (ScopedDbContextFactory)(httpContextAccessor.HttpContext.Items["dbFactory"]);
            var db = dbFactory.Create();
            return db.Comments.Where(c => c.PostId == post.Id);
        }
    }
}