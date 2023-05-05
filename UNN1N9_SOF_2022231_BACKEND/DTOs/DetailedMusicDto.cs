namespace UNN1N9_SOF_2022231_BACKEND.DTOs
{
    public class DetailedMusicDto
    {
        public int Id { get; set; }
        public string Genre { get; set; }
        public string ArtistName { get; set; }
        public string TrackName { get; set; }
        public int DurationMs { get; set; }
        public string TrackId { get; set; }
        public int Popularity { get; set; }
        public double Acousticness { get; set; }
        public double Danceability { get; set; }
        public double Energy { get; set; }
        public string Key { get; set; }
        public double Liveness { get; set; }
        public double Loudness { get; set; }
        public string Mode { get; set; }
        public double Speechiness { get; set; }
        public double Tempo { get; set; }
        public double Valence { get; set; }
    }
}
