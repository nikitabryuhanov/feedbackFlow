using System.Text.Json.Serialization;

namespace FeedbackFlow.DTOs
{
    public class RequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }
}