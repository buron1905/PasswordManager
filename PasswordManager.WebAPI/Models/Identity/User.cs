using System.ComponentModel.DataAnnotations;
using static PasswordManager.WebAPI.Data.Validation.User;

namespace PasswordManager.WebAPI.Models.Identity
{
    public class User
    {
        [Required]
        [MaxLength(MaxEmailLength)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
