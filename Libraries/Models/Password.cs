using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Models.Validation.ModelsValidationProperties;

namespace Models
{
    public class Password : Entity
    {

        [Required]
        [SQLite.NotNull]
        [MaxLength(MaxPasswordNameLength, ErrorMessage = MaxPasswordNameLengthErrorMessage)]
        public string? PasswordName { get; set; }

        [MaxLength(MaxUserNameLength, ErrorMessage = MaxUserNameLengthErrorMessage)]
        public string? UserName { get; set; }

        public string? PasswordEncrypted { get; set; }

        [NotMapped]
        [SQLite.Ignore]
        [MaxLength(MaxPasswordLength, ErrorMessage = MaxPasswordLengthErrorMessage)]
        public string? PasswordDecrypted { get; set; }

        [MaxLength(MaxURLLength, ErrorMessage = MaxURLLengthErrorMessage)]
        public string? URL { get; set; }

        [MaxLength(MaxNotesLength, ErrorMessage = MaxNotesLengthErrorMessage)]
        public string? Notes { get; set; }

        [Required]
        [SQLite.NotNull]
        public bool Favorite { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

    }
}
