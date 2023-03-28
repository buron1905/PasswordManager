using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class PasswordOffline : Password
    {
        [Required]
        public DateTime UDTOffline { get; set; }
    }
}
