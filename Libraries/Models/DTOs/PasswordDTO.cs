using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Models.Validation.ModelsValidationProperties;

namespace Models.DTOs
{
    public class PasswordDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = RequiredPasswordNameErrorMessage)]
        [MaxLength(MaxPasswordNameLength, ErrorMessage = MaxPasswordNameLengthErrorMessage)]
        public string? PasswordName { get; set; }

        [Required]
        [MaxLength(MaxUserNameLength, ErrorMessage = MaxUserNameLengthErrorMessage)]
        public string? UserName { get; set; }

        //[Required]
        public string? PasswordEncrypted { get; set; }

        [NotMapped]
        public string? PasswordDecrypted { get; set; }

        [MaxLength(MaxURLLength, ErrorMessage = MaxURLLengthErrorMessage)]
        public string? URL { get; set; }


        [MaxLength(MaxNotesLength, ErrorMessage = MaxNotesLengthErrorMessage)]
        public string? Notes { get; set; }

        public bool Favorite { get; set; }

        public DateTime UDT { get; set; }

        public DateTime IDT { get; set; }

    }
}
