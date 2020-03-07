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

namespace AspCoreGraphQL.GraphQL
{
    public class Query
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<Query> logger;

        public Query([Service] IHttpContextAccessor httpContextAccessor, [Service] ILogger<Query> logger)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
        }

        public IQueryable<Post> Posts()
        {
            var dbFactoryFunc = (Func<DataContext>)(httpContextAccessor.HttpContext.Items["dbFactoryFunc"]);
            var db = dbFactoryFunc();
            return db.Posts;
        }

        public IQueryable<Comment> Comments()
        {
            var dbFactoryFunc = (Func<DataContext>)(httpContextAccessor.HttpContext.Items["dbFactoryFunc"]);
            var db = dbFactoryFunc();
            return db.Comments;
        }
    }
}