using System.Collections.Generic;

namespace AspCoreGraphQL.Entities.Seed
{
    public static class SeedData
    {
        public static readonly List<Post> Posts = new List<Post>{
            new Post{
                Title="Post1",
                Text="Post1 text",
            }
        };
    }
}