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
    [ExtendObjectType(Name = nameof(Comment))]
    public class CommentResolvers : ResolverBase
    {
        private readonly ILogger<CommentResolvers> logger;

        public CommentResolvers([Service] IHttpContextAccessor httpContextAccessor, [Service] ILogger<CommentResolvers> logger)
        : base(httpContextAccessor)
        {
            this.logger = logger;
        }

        public async Task<Post> Post([Parent]Comment comment, IResolverContext context)
        {
            var dataLoader = context.BatchDataLoader<int, Post>("commentPost", async keys =>
            {
                var db = CreateDataContext();
                return await db.Posts
                    .Where(p => keys.Contains(p.Id))
                    .ToDictionaryAsync(p => p.Id);
            });
            return await dataLoader.LoadAsync(comment.PostId, CancellationToken.None);
        }
    }
}