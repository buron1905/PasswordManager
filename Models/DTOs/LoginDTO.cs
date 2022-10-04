using System.ComponentModel.DataAnnotations;
using static Models.Helpers.Validation.User;

namespace Models.DTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string? EmailAddress { get; set; }
        
        [Required]
        [MaxLength(MaxEmailLength)]
        public string? Password { get; set; }
    }
}
