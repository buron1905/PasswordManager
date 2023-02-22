using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class PasswordGeneratorSettingsDTO
    {

        [Required]
        [Range(5, 256)]
        public int PasswordLength { get; set; } = 12;

    }
}
