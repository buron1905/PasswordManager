using System.ComponentModel.DataAnnotations;
using static PasswordManager.WebAPI.Data.Validation.User;

namespace PasswordManager.WebAPI.Models.Identity
{
    public class LoginDTO
    {
        [Required]
        public string? EmailAddress { get; set; }
        
        [Required]
        [MaxLength(MaxEmailLength)]
        public string? Password { get; set; }
    }
}
