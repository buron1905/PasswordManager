namespace Models.DTOs
{
    public class RegisterResponseDTO
    {
        public bool IsRegistrationSuccessful { get; set; }

        public string? ErrorMessage { get; set; }

        public UserDTO? User { get; set; }

    }
}
