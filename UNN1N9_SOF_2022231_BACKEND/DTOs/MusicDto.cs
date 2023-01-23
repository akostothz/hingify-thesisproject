using System.ComponentModel.DataAnnotations;

namespace UNN1N9_SOF_2022231_BACKEND.DTOs
{
    public class MusicDto
    {
        public int Id { get; set; }
        public string Genre { get; set; }
        public string ArtistName { get; set; }
        public string TrackName { get; set; }
        public int DurationMs { get; set; }
        public string TrackId { get; set; }
    }
}
