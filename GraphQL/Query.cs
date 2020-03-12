using System.Collections.Generic;
using System;
using System.Linq;
using AspCoreGraphQL.Entities;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using AspCoreGraphQL.Entities.Context;
using HotChocolate.Types.Relay;

namespace AspCoreGraphQL.GraphQL
{
    public class Query : ResolverBase
    {
        private readonly ILogger<Query> logger;

        public Query([Service] IHttpContextAccessor httpContextAccessor, [Service] ILogger<Query> logger)
        : base(httpContextAccessor)
        {
            this.logger = logger;
        }

        public IQueryable<Tag> Tags() => CreateDataContext().Tags;

        [UseSorting]
        [UseFiltering]
        public IQueryable<Post> Posts() => CreateDataContext().Posts;

        //Attribute order matters
        [UsePaging]
        [UseSorting]
        [UseFiltering]
        public IQueryable<Comment> Comments() => CreateDataContext().Comments;

        public IQueryable<Comment> PostComments(int postId) => CreateDataContext().Comments.Where(c => c.PostId == postId);
    }
}