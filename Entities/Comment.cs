using System.ComponentModel.DataAnnotations.Schema;
namespace AspCoreGraphQL.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public int PostId { get; set; }
    }
}