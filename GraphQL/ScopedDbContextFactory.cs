using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AspCoreGraphQL.Entities.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspCoreGraphQL.GraphQL
{
    //this stores the requested dbcontexts, and disposes them in the dispose method
    public class ScopedDbContextFactory : IDisposable, IAsyncDisposable
    {
        static long idCounter = 0;
        long myId;

        List<DbContext> dbContexts = new List<DbContext>();
        private readonly ILogger<ScopedDbContextFactory> logger;
        private readonly IServiceProvider serviceProvider;

        public ScopedDbContextFactory(ILogger<ScopedDbContextFactory> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            myId = Interlocked.Increment(ref idCounter);
            logger.LogInformation($"DBFACT CREATED {myId}");
        }

        public GraphQLDataContext Create()
        {
            var db = serviceProvider.GetRequiredService<GraphQLDataContext>();
            dbContexts.Add(db);
            return db;
        }

        public void Dispose()
        {
            logger.LogInformation($"DBFACT DISPOSED {myId}");
            foreach (var db in dbContexts)
            {
                try
                {
                    db.Dispose();
                }
                catch (Exception ex)
                {
                    logger.LogError($"Failed dispose datacontext: {ex}");
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            logger.LogInformation($"DBFACT ASYNC DISPOSED {myId}");
            foreach (var db in dbContexts)
            {
                try
                {
                    await db.DisposeAsync();
                }
                catch (Exception ex)
                {
                    logger.LogError($"Failed disposeAsync datacontext: {ex}");
                }
            }
        }
    }
}