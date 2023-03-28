using Models.DTOs;

namespace Services.Abstraction.Data
{
    public interface ISyncService
    {
        Task<LastChangeResponseDTO?> GetLastChangeDateTime(Guid userId);
        Task<SyncResponseDTO?> SyncAccount(SyncRequestDTO data);
    }
}
