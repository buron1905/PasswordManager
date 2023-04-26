namespace Models.DTOs
{
    public class SyncResponseDTO
    {
        public bool SyncSuccessful { get; set; }
        public bool SyncCollision { get; set; }
        public bool SendingNewData { get; set; }

        public DateTime LastChangeDate { get; set; }

        public UserDTO UserDTO { get; set; }
        public IEnumerable<PasswordDTO> Passwords { get; set; }
    }
}
