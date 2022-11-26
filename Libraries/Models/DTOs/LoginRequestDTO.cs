using System.ComponentModel.DataAnnotations;
using static Models.Validation.ModelsValidation;

namespace Models.DTOs
{
    public class LoginRequestDTO
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
