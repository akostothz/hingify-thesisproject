namespace UNN1N9_SOF_2022231_BACKEND.DTOs
{
    public class SpotifyTrackDto
    {
        public float Danceability { get; set; }
        public float Energy { get; set; }
        public int Key { get; set; }
        public float Loudness { get; set; }
        public float Speechiness { get; set; }
        public float Acousticness { get; set; }
        public float Instrumentalness { get; set; }
        public float Liveness { get; set; }
        public int Mode { get; set; }
        public float Valence { get; set; }
        public float Tempo { get; set; }
        public string Id { get; set; }
        public int Duration_ms { get; set; }
        public int TimeSignature { get; set; }
        public List<string> Genres { get; set; }
    }
}
