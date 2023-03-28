using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class PasswordOfflineDTO : PasswordDTO
    {
        [Required]
        public DateTime UDTServerTime { get; set; }
        public bool Synced { get; set; }
    }
}
