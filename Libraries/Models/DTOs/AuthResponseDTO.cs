namespace Models.DTOs
{
    public class AuthResponseDTO
    {
        public bool IsAuthSuccessful { get; set; }

        public bool EmailVerified { get; set; }

        public bool IsTfaEnabled { get; set; }

        public string? ErrorMessage { get; set; }

        public string? JweToken { get; set; }

        public DateTime ExpirationDateTime { get; set; }

    }
}
