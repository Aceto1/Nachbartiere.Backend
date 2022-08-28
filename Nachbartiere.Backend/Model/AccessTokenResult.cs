using System.Text.Json.Serialization;

namespace Nachbartiere.Backend.Model
{
    public class AccessTokenResult
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
