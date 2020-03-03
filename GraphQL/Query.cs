using System.Linq;
using AspCoreGraphQL.Entities;
using AspCoreGraphQL.Entities.Context;

namespace AspCoreGraphQL.GraphQL
{
    public class Query
    {
        private readonly DataContext db;
        public Query(DataContext db)
        {
            this.db = db;
        }

        public IQueryable<Post> GetPosts() => db.Posts;
    }
}