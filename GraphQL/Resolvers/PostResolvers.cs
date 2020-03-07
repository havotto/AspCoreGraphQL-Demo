using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspCoreGraphQL.Entities;
using AspCoreGraphQL.Entities.Context;
using HotChocolate;
using HotChocolate.DataLoader;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
    public class PostResolvers : ResolverBase
    {
        private readonly ILogger<PostResolvers> logger;

        public PostResolvers([Service] IHttpContextAccessor httpContextAccessor, [Service] ILogger<PostResolvers> logger)
        : base(httpContextAccessor)
        {
            this.logger = logger;
        }
        public async Task<Comment[]> Comments([Parent]Post post, IResolverContext context)
        {
            var dataLoader = context.GroupDataLoader<int, Comment>("postComments", async keys =>
            {
                var db = CreateDataContext();
                var comments = await db.Comments.Where(c => keys.Contains(c.PostId)).ToListAsync();
                return comments.ToLookup(c => c.PostId);
            });
            return await dataLoader.LoadAsync(post.Id, CancellationToken.None);
        }

        public async Task<Tag[]> Tags([Parent]Post post, IResolverContext context)
        {
            var dataLoader = context.GroupDataLoader<int, Tag>("postTags", async keys =>
            {
                var db = CreateDataContext();
                var q = from pt in db.PostTags
                        join t in db.Tags on pt.TagId equals t.Id
                        where keys.Contains(pt.PostId)
                        select new { pt.PostId, Tag = t };

                var children = await q.ToListAsync();
                return children.ToLookup(c => c.PostId, c => c.Tag);
            });
            return await dataLoader.LoadAsync(post.Id, CancellationToken.None);
        }
    }
}