using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class SettingsDTO : EntityDTO
    {
        [Required]
        public bool SavePassword { get; set; } = false;
    }
}
