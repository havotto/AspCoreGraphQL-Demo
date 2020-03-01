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

        public static readonly List<Tag> Tags = new List<Tag>{
            new Tag{
                Id=1, Name="Tag1"
            },
            new Tag{
                Id=2, Name="Tag2"
            },
        };

        public static readonly List<PostTag> PostTags = new List<PostTag>{
            new PostTag{
                PostId=1, TagId=1,
            },
            new PostTag{
                PostId=1, TagId=2,
            },
            new PostTag{
                PostId=2, TagId=1,
            },
        };
    }
}