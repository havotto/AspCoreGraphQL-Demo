using System.ComponentModel.DataAnnotations;

namespace AspCoreGraphQL.Entities
{
    public class Post
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Text is required")]
        public string Text { get; set; }
    }
}