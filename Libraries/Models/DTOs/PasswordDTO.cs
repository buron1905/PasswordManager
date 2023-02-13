using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Models.Validation.ModelsValidation;

namespace Models.DTOs
{
    public class PasswordDTO
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(MaxPasswordNameLength, ErrorMessage = MaxPasswordNameLengthErrorMessage)]
        public string? PasswordName { get; set; }

        [Required]
        [MaxLength(MaxUserNameLength, ErrorMessage = MaxUserNameLengthErrorMessage)]
        public string? UserName { get; set; }

        //[Required]
        public string? PasswordEncrypted { get; set; }

        [NotMapped]
        public string? PasswordDecrypted { get; set; }

        [MaxLength(MaxPasswordDescriptionLength, ErrorMessage = MaxPasswordDescriptionLengthErrorMessage)]
        public string? Description { get; set; }

        // public DateTime Updated { get; set; }
    }
}
