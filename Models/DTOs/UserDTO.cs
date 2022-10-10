using System.ComponentModel.DataAnnotations;
using static Models.Helpers.Validation;

namespace Models.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        
        [Required]
        [EmailAddress]
        [MaxLength(MaxEmailLength, ErrorMessage = MaxEmailLengthErrorMessage)]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
