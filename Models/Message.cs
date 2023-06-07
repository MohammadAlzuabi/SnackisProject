using System.Text.Json.Serialization;

namespace SnackisProject.Models
{
    public class Message
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("toUser")]
        public string ToUser { get; set; }
        [JsonPropertyName("fromUser")]
        public string FromUser { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("sentAt")]
        public DateTime SentAt { get; set; }
        [JsonPropertyName("isRead")]
        public bool IsRead { get; set; }
    }
}
