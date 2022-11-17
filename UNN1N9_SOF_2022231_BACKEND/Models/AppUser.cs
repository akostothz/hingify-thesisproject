using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN1N9_SOF_2022231_BACKEND.Models
{
    public class AppUser
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        [StringLength(100)]
        public string Country { get; set; }

        [NotMapped]
        public virtual ICollection<Music> Musics { get; set; }

        public virtual ICollection<UserBehavior> Behaviors { get; set; }
    }
}
