using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class SyncRequestDTO
    {

        [Required]
        public UserDTO UserDTO { get; set; }

        public IEnumerable<PasswordDTO> Passwords { get; set; }
    }
}
