using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Models.Validation.ModelsValidationProperties;

namespace Models.DTOs
{
    public class PasswordDTO : EntityDTO
    {
        [Required(ErrorMessage = RequiredPasswordNameErrorMessage)]
        [MaxLength(MaxPasswordNameLength, ErrorMessage = MaxPasswordNameLengthErrorMessage)]
        public string? PasswordName { get; set; }

        [MaxLength(MaxUserNameLength, ErrorMessage = MaxUserNameLengthErrorMessage)]
        public string? UserName { get; set; }

        public string? PasswordEncrypted { get; set; }

        [NotMapped]
        [MaxLength(MaxPasswordLength, ErrorMessage = MaxPasswordLengthErrorMessage)]
        public string? PasswordDecrypted { get; set; }

        [MaxLength(MaxURLLength, ErrorMessage = MaxURLLengthErrorMessage)]
        public string? URL { get; set; }


        [MaxLength(MaxNotesLength, ErrorMessage = MaxNotesLengthErrorMessage)]
        public string? Notes { get; set; }

        public bool Favorite { get; set; }

    }
}
