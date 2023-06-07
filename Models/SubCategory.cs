using System.Text.Json.Serialization;

namespace SnackisProject.Models
{
    public class SubCategory
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("categoryId")]
        public string CategoryId { get; set; }
    }
}
