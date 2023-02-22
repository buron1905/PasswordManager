namespace Models.DTOs
{
    public class AuthResponseDTO
    {
        public string? JweToken { get; set; }
        public DateTime ExpirationDateTime { get; set; }

    }
}
