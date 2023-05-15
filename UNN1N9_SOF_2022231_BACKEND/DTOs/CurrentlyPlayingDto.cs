using Newtonsoft.Json;

namespace UNN1N9_SOF_2022231_BACKEND.DTOs
{
    public class CurrentlyPlayingDto
    {
        [JsonProperty("item")]
        public TrackItem Item { get; set; }
    }
    public class TrackItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

    }
}
