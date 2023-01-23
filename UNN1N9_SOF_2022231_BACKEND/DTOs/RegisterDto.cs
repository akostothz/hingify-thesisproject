using System.ComponentModel.DataAnnotations;

namespace UNN1N9_SOF_2022231_BACKEND.DTOs
{
    public class RegisterDto
    {
        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string Password { get; set; }

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
    }
}
