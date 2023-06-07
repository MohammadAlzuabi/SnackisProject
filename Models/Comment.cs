using System.Text.Json.Serialization;

namespace SnackisProject.Models
{
    public class Comment
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("postId")]
        public string PostId { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }
    }
}
