using System;
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
                CreatedAt=new DateTime(2020,03,5,12,34,56),
                PublishedAt=new DateTimeOffset(2020,3,7,20,0,0,TimeSpan.Zero),
            },
            new Post{
                Id=2,
                Title="Post2",
                Text="Post2 text",
                CreatedAt=new DateTime(2020,06,6,11,22,33),
            },
        };

        public static readonly List<Comment> Comments = new List<Comment>{
            new Comment{
                PostId=1,
                Text="Post1 comment1 text",
            },
            new Comment{
                PostId=1,
                Text="Post1 comment2 text",
            },
            new Comment{
                PostId=2,
                Text="Post2 comment1 text",
            },
            new Comment{
                PostId=2,
                Text="Post2 comment3 text",
            },
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