using System.ComponentModel;
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
using System.Threading.Tasks;
using AspCoreGraphQL_Demo.Dto;
using HotChocolate.AspNetCore.Authorization;

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


        public ClaimsPrincipalDto CurrentPrincipal(IResolverContext context)
        {
            return new ClaimsPrincipalDto(User);
        }

        [Authorize]
        public IQueryable<Tag> Tags() => CreateDataContext().Tags;

        //[UseSelection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Post> Posts() => CreateDataContext().Posts;

        public async Task<Post?> Post(int id, IResolverContext context)
        {
            return await LoadByIdAsync<int, Post>(context, id);
        }


        //Attribute order matters: filter and sort must be before selection and paging
        [UsePaging]
        //[UseSelection]
        [UseSorting]
        [UseFiltering]
        public IQueryable<Comment> Comments() => CreateDataContext().Comments;

        public IQueryable<Comment> PostComments(int postId) => CreateDataContext().Comments.Where(c => c.PostId == postId);
    }
}