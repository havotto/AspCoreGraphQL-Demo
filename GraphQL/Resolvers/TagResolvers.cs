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
    [ExtendObjectType(Name = nameof(Tag))]
    public class TagResolvers : ResolverBase
    {
        private readonly ILogger<PostResolvers> logger;

        public TagResolvers([Service] IHttpContextAccessor httpContextAccessor, [Service] ILogger<PostResolvers> logger)
        : base(httpContextAccessor)
        {
            this.logger = logger;
        }

        public async Task<Post[]> Posts([Parent]Tag tag, IResolverContext context)
        {
            var dataLoader = context.GroupDataLoader<int, Post>("tagPosts", async keys =>
            {
                var db = CreateDataContext();
                var q = from pt in db.PostTags
                        join p in db.Posts on pt.PostId equals p.Id
                        where keys.Contains(pt.TagId)
                        select new { pt.TagId, Post = p };

                var children = await q.ToListAsync();
                return children.ToLookup(c => c.TagId, c => c.Post);
            });
            return await dataLoader.LoadAsync(tag.Id, CancellationToken.None);
        }
    }
}