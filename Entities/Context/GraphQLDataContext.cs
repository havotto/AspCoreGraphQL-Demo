using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AspCoreGraphQL.Entities.Context
{
    public class GraphQLDataContext : DataContext
    {
        public GraphQLDataContext(DbContextOptions<DataContext> options, ILogger<DataContext> logger) : base(options, logger)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override ValueTask DisposeAsync()
        {
            return base.DisposeAsync();
        }

    }
}