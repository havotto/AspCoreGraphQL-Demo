using System;
using AspCoreGraphQL.Entities.Context;
using Microsoft.AspNetCore.Http;

namespace AspCoreGraphQL.GraphQL
{
    public abstract class ResolverBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        protected ResolverBase(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected DataContext CreateDataContext()
        {
            var dbFactoryFunc = (Func<DataContext>)(httpContextAccessor.HttpContext.Items["dbFactoryFunc"]);
            return dbFactoryFunc();
        }
    }
}