using Newtonsoft.Json;

namespace UNN1N9_SOF_2022231_BACKEND.DTOs
{
    public class ResponseDTO
    {
        [JsonProperty("access_token")]
        public string access_token { get; set; }
        [JsonProperty("token_type")]
        public string token_type { get; set; }
        [JsonProperty("expires_in")]
        public int expires_in { get; set; }
        [JsonProperty("refresh_token")]
        public string refresh_token { get; set; }
        [JsonProperty("scope")]
        public string scope { get; set; }
    }
}
