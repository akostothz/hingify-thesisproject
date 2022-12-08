using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UNN1N9_SOF_2022231_BACKEND.Models
{
    public class Music
    {
        [Required]
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Genre { get; set; }

        [Required]
        public string ArtistName { get; set; }

        [Required]
        public string TrackName { get; set; }

        [Required]
        public string TrackId { get; set; }

        [Range(0, 100)]
        public int Popularity { get; set; }

        [Range(0, 1)]
        public double Acousticness { get; set; }

        [Range(0, 1)]
        public double Danceability { get; set; }

        [Required]
        public int DurationMs { get; set; }

        [Range(0, 1)]
        public double Energy { get; set; }

        [StringLength(2)]
        public string Key { get; set; }

        [Range(0, 1)]
        public double Liveness { get; set; }

        public double Loudness { get; set; }

        [StringLength(5)]
        public string Mode { get; set; }

        [Range(0, 1)]
        public double Speechiness { get; set; }

        public double Tempo { get; set; }

        [Range(0, 1)]
        public double Valence { get; set; }

        [NotMapped]
        public virtual ICollection<AppUser> Users { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserBehavior> UserBehaviors { get; set; }

    }
}
