namespace UNN1N9_SOF_2022231_BACKEND.DTOs
{
    public class SpoitifyTrackMainFeaturesDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Artist[] Artists { get; set; }
        public int Popularity { get; set; }
    }
    public class Artist
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string[] Genres { get; set; }
    }
}
