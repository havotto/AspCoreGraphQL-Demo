using System.Security.Claims;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspCoreGraphQL.Entities;
using AspCoreGraphQL.Entities.Context;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AspCoreGraphQL.GraphQL
{
    public abstract class ResolverBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        protected ResolverBase(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            HttpContext = httpContextAccessor.HttpContext;
            User = HttpContext.User;
        }

        protected DataContext CreateDataContext()
        {
            var dbFactoryFunc = (Func<DataContext>)(httpContextAccessor.HttpContext.Items["dbFactoryFunc"]);
            return dbFactoryFunc();
        }

        protected HttpContext HttpContext { get; }
        protected ClaimsPrincipal User { get; }

        /// <summary>
        /// Loads entities with BatchDataLoader
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dataLoaderKey"></param>
        /// <param name="key"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        protected async Task<TValue> LoadByIdAsync<TKey, TValue>(
            IResolverContext context, TKey key)
        where TValue : class, IHasId<TKey>
        {
            var dataLoader = context.BatchDataLoader<TKey, TValue>(typeof(TValue).FullName, async keys =>
            {
                var db = CreateDataContext();
                return await db.Set<TValue>()
                    .Where(p => keys.Contains(p.Id))
                    .ToDictionaryAsync(p => p.Id);
            });
            return await dataLoader.LoadAsync(key, CancellationToken.None);
        }

    }
}