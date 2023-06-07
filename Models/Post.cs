using System.Text.Json.Serialization;

namespace SnackisProject.Models
{
    public class Post
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }


        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("subCategoryId")]
        public string SubCategoryId { get; set; }
    }
}
