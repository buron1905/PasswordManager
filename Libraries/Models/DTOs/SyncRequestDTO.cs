using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class SyncRequestDTO
    {
        public DateTime LastChangeDate { get; set; }

        [Required]
        public UserDTO UserDTO { get; set; }

        public IEnumerable<PasswordOfflineDTO> Passwords { get; set; }
    }
}
