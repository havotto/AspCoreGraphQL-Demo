using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AspCoreGraphQL.Entities.Context
{
    /// <summary>
    /// This is registered transiently
    /// </summary>
    public class GraphQLDataContext : DataContext
    {
        public GraphQLDataContext(DbContextOptions<DataContext> options, ILogger<DataContext> logger) : base(options, logger)
        {
        }
    }
}