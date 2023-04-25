using Models.DTOs;

namespace Services.Abstraction.Data
{
    public interface IMauiSyncService : ISyncService
    {
        Task<LastChangeResponseDTO?> GetLastChangeDateTimeLocal(Guid userId);
        Task<SyncResponseDTO?> DoSync();

        // This is practicaly InsertOrUpdate functionality, so maybe rename
        Task<SyncResponseDTO?> SyncExistingAndNewUser(UserDTO newUserDTO);
    }
}
