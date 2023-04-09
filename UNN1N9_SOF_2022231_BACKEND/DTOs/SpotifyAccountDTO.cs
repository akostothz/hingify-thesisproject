namespace UNN1N9_SOF_2022231_BACKEND.DTOs
{
    public class SpotifyAccountDTO
    {
        public string country { get; set; }
        public string display_name { get; set; }
        public string email { get; set; }
        public ExplicitContentDTO explicit_content { get; set; }
        public SpotifyExternalUrlsDTO external_urls { get; set; }
        public SpotifyFollowersDTO followers { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<SpotifyImageDTO> images { get; set; }
        public string product { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }
}
