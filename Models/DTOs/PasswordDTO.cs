using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Models.Helpers.Validation;

namespace Models.DTOs
{
    public class PasswordDTO
    {
        [Required]
        [MaxLength(MaxPasswordNameLength, ErrorMessage = MaxPasswordNameLengthErrorMessage)]
        public string? PasswordName { get; set; }

        [Required]
        [MaxLength(MaxUserNameLength, ErrorMessage = MaxUserNameLengthErrorMessage)]
        public string? UserName { get; set; }

        [NotMapped]
        public string? PasswordDecrypted { get; set; }

        [MaxLength(MaxPasswordDescriptionLength, ErrorMessage = MaxPasswordDescriptionLengthErrorMessage)]
        public string? Description { get; set; }
    }
}
