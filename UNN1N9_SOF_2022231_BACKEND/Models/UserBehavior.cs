using System.ComponentModel.DataAnnotations;

namespace UNN1N9_SOF_2022231_BACKEND.Models
{
    public class UserBehavior
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int MusicId { get; set; }

        [Required]
        public int ListeningCount { get; set; }

        [Required]
        [StringLength(10)]
        public string NameOfDay { get; set; }

        [Required]
        [StringLength(50)]
        public string TimeOfDay { get; set; }

        public DateOnly Date { get; set; }

        public virtual AppUser User { get; set; }
        public virtual Music Music { get; set; }
    }
}
