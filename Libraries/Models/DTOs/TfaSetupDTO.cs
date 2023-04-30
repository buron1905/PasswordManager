using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class TfaSetupDTO
    {
        public bool IsTfaEnabled { get; set; }

        [Required]
        public string Code { get; set; }

        public string? AuthenticatorKey { get; set; }

        public string? QrCodeSetupImageUrl { get; set; }
    }
}
