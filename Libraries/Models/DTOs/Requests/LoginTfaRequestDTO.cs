using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class LoginTfaRequestDTO
    {
        [Required]
        public string? Token { get; set; }

        [Required]
        public string? Code { get; set; }
    }
}
