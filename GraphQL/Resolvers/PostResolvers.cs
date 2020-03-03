using System.Linq;
using AspCoreGraphQL.Entities;
using AspCoreGraphQL.Entities.Context;
using HotChocolate;

namespace AspCoreGraphQL.GraphQL.Resolvers
{
    [GraphQLResolverOf(typeof(Post))]
    public class PostResolvers
    {
        private readonly DataContext db;
        public PostResolvers(DataContext db)
        {
            this.db = db;

        }
        public IQueryable<Comment> Comments([Parent]Post post)
        {
            return db.Comments.Where(c => c.PostId == post.Id);
        }
    }
}