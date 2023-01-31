using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Text.Json.Serialization;

namespace UNN1N9_SOF_2022231_BACKEND.Models
{
    public class AppUser/* : IdentityUser*/
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; } //ezt még kicserélni IdentityUser-es verzióra
        public byte[] PasswordSalt { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        public int YearOfBirth { get; set; }

        [Required]
        [StringLength(100)]
        public string Country { get; set; }
        
        [Required]
        public string Gender { get; set; }

        public string PhotoUrl { get; set; }
        public string PublicId { get; set; }

        [NotMapped]
        public virtual ICollection<Music> Musics { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserBehavior> Behaviors { get; set; }
    }
}
