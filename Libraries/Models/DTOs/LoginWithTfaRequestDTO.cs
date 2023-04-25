using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class LoginWithTfaRequestDTO : LoginRequestDTO
    {
        [Required]
        public string? Code { get; set; }
    }
}
