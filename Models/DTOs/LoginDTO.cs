using System.ComponentModel.DataAnnotations;
using static Models.Helpers.Validation;

namespace Models.DTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        [MaxLength(MaxEmailLength, ErrorMessage = MaxEmailLengthErrorMessage)]
        public string? EmailAddress { get; set; }
        
        [Required]
        [MaxLength(MaxPasswordLength, ErrorMessage = MaxPasswordLengthErrorMessage)]
        public string? Password { get; set; }
    }
}
