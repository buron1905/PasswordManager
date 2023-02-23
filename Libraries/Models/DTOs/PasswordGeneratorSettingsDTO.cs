using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class PasswordGeneratorSettingsDTO
    {
        [Required]
        [Range(5, 256)]
        public int PasswordLength { get; set; } = 12;

        public bool UseNumbers { get; set; } = true;

        public bool UseSpecialChars { get; set; } = true;

        public bool UseUppercase { get; set; } = true;

        public bool UseLowercase { get; set; } = true;

    }
}
