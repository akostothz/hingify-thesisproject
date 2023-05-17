using Newtonsoft.Json;

namespace UNN1N9_SOF_2022231_BACKEND.DTOs
{
    public class RecentlyPlayedDto
    {
        [JsonProperty("items")]
        public List<RecentlyPlayedItem> Items { get; set; }
    }
    public class RecentlyPlayedItem
    {
        [JsonProperty("track")]
        public TrackItem Track { get; set; }
    }
}
