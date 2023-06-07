using System.Text.Json.Serialization;

namespace SnackisProject.Models
{
    public class Report
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("byUser")]
        public string ByUser { get; set; }

        [JsonPropertyName("postId")]
        public string PostId { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}
