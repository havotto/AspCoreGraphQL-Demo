using System;
using System.ComponentModel.DataAnnotations;
using HotChocolate;

namespace AspCoreGraphQL.Entities
{
    public class Post
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = "";
        public string? Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTimeOffset? PublishedAt { get; set; }
    }
}