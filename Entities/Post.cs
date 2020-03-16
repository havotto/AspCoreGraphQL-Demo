using System;
using System.ComponentModel.DataAnnotations;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;

namespace AspCoreGraphQL.Entities
{
    public class Post : IHasId<int>
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = "";

        /// <summary>
        /// The text of the post
        /// </summary>
        /// <value></value>
        public string? Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }

        [Authorize]
        public decimal Rating { get; set; }
        public int IgnoredMethod() => 2;
    }
}