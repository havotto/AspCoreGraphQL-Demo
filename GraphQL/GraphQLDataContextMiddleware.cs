using System;
using System.Threading.Tasks;
using AspCoreGraphQL.Entities.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspCoreGraphQL.GraphQL
{
    /// <summary>
    /// This is responsible for creating a DI scope, and construct a dbcontext factory function.
    /// </summary>
    public class GraphQLDataContextMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<GraphQLDataContextMiddleware> logger;

        public GraphQLDataContextMiddleware(RequestDelegate next, IServiceProvider serviceProvider, ILogger<GraphQLDataContextMiddleware> logger)
        {
            this.logger = logger;
            this.next = next;
            this.serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                Func<DataContext> dbFactoryFunc = () => scope.ServiceProvider.GetRequiredService<GraphQLDataContext>();
                context.Items.Add("dbFactoryFunc", dbFactoryFunc);

                await next(context);
            }
        }
    }
}