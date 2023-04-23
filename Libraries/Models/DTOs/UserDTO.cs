using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static Models.Validation.ModelsValidationProperties;

namespace Models.DTOs
{
    public class UserDTO : EntityDTO
    {

        [Required]
        [EmailAddress]
        [MaxLength(MaxEmailLength, ErrorMessage = MaxEmailLengthErrorMessage)]
        public string? EmailAddress { get; set; }

        [Required]
        public string? Password { get; set; }

        [JsonIgnore]
        public string? PasswordHASH { get; set; }

        public bool EmailConfirmed { get; set; }

        public string? EmailConfirmationToken { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public string? TwoFactorSecret { get; set; }

    }
}
