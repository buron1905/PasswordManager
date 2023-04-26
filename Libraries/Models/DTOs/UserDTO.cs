using System.ComponentModel.DataAnnotations;
using static Models.Validation.ModelsValidationProperties;

namespace Models.DTOs
{
    public class UserDTO : EntityDTO
    {

        [Required]
        [EmailAddress]
        [MaxLength(MaxEmailLength, ErrorMessage = MaxEmailLengthErrorMessage)]
        public string? EmailAddress { get; set; }

        [MaxLength(MaxPasswordLength, ErrorMessage = MaxPasswordLengthErrorMessage)]
        public string? Password { get; set; }

        public string? PasswordHASH { get; set; }

        public bool EmailConfirmed { get; set; }

        public string? EmailConfirmationToken { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public string? TwoFactorSecret { get; set; }

    }
}
