using System.ComponentModel.DataAnnotations;

namespace UNN1N9_SOF_2022231_BACKEND.Models
{
    public class LikedSong
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int MusicId { get; set; }
        public virtual AppUser User { get; set; }
        public virtual Music Music { get; set; }
    }
}
