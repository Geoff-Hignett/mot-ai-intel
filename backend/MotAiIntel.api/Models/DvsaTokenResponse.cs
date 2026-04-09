using System.Text.Json.Serialization;

namespace MotAiIntel.api.Models
{
    public class DvsaTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = "";

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
