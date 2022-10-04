using System.ComponentModel.DataAnnotations;
using static Models.Helpers.Validation.User;

namespace Models.DTOs
{
    public class UserDTO
    {
        [Required]
        [MaxLength(MaxEmailLength)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
