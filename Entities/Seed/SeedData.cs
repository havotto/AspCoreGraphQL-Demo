using System.Collections.Generic;

namespace AspCoreGraphQL.Entities.Seed
{
    public static class SeedData
    {
        public static readonly List<Post> Posts = new List<Post>{
            new Post{
                Id=1,
                Title="Post1",
                Text="Post1 text",
            },
            new Post{
                Id=2,
                Title="Post2",
                Text="Post2 text",
            }

        };

        public static readonly List<Comment> Comment = new List<Comment>{
            new Comment{
                Text="Post1 Comment1 text",
                PostId=1,
            },
            new Comment{
                Text="Post1 Comment2 text",
                PostId=1,
            },
            new Comment{
                Text="Post2 Comment1 text",
                PostId=2,
            },
            new Comment{
                Text="Post2 Comment2 text",
                PostId=2,
            },
        };
    }
}