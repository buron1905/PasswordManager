using Models.DTOs;
using Services.Abstraction.Data;
using Services.Abstraction.Data.Persistance;

namespace Services.Data
{
    public class SyncService : ISyncService
    {
        private readonly IDataServiceWrapper _dataServiceWrapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public SyncService(IDataServiceWrapper dataServiceWrapper, IRepositoryWrapper repositoryWrapper)
        {
            _dataServiceWrapper = dataServiceWrapper;
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<LastChangeResponseDTO?> GetLastChangeDateTime(Guid userId)
        {
            var lastChangeUser = (await _dataServiceWrapper.UserService.GetByIdAsync(userId)).UDT;
            var passwords = await _dataServiceWrapper.PasswordService.GetAllByUserIdAsync(userId);
            var lastChangePassword = GetLastChangeDateTime(passwords);

            var response = new LastChangeResponseDTO() { LastChangeUser = lastChangeUser, LastChangePassword = lastChangePassword };
            return response;
        }

        public async Task<SyncResponseDTO?> SyncAccount(SyncRequestDTO data)
        {
            var response = new SyncResponseDTO();
            try
            {
                var userId = data.UserDTO.Id;
                var userDTO = await _dataServiceWrapper.UserService.GetByIdAsync(userId);

                var offlinePasswords = data.Passwords.ToList();
                var syncedOfflinePasswords = data.Passwords.Where(x => x.Synced).ToList();
                var unSyncedOfflinePasswords = data.Passwords.Where(x => !x.Synced).ToList();
                var onlinePasswords = (await _dataServiceWrapper.PasswordService.GetAllByUserIdAsync(data.UserDTO.Id)).ToList();
                var newOnlinePasswords = onlinePasswords.Where(x => !offlinePasswords.Any(o => o.Id == x.Id)).ToList();
                var responsePasswords = new List<PasswordDTO>();

                // offline DB has no rows
                if (syncedOfflinePasswords.Count() == 0)
                {
                    response.SyncSuccessful = true;
                    response.SendingNewData = true;
                    response.SyncCollision = false;
                    response.UserDTO = userDTO;
                    var lastChangePassword = GetLastChangeDateTime(onlinePasswords);
                    response.LastChangeDate = userDTO.UDT > lastChangePassword ? userDTO.UDT : lastChangePassword;
                    response.Passwords = onlinePasswords;
                    return response;
                }

                // offline DB has unsynced rows
                if (unSyncedOfflinePasswords.Count() > 0)
                {
                    response.SendingNewData = true;
                    foreach (var password in unSyncedOfflinePasswords)
                    {
                        responsePasswords.Add(await _dataServiceWrapper.PasswordService.CreateAsync(userId, password));
                    }
                }

                // Adding new online passwords
                if (newOnlinePasswords.Count() > 0)
                {

                    responsePasswords.AddRange(newOnlinePasswords);
                }

                // handling conflicts of synced passwords
                foreach (var password in onlinePasswords)
                {
                    var offlinePassword = syncedOfflinePasswords.SingleOrDefault(x => x.Id == password.Id);
                    if (offlinePassword is null)
                    {
                        response.SendingNewData = true;
                        responsePasswords.Add(password);
                        continue;
                    }

                    // Server has newer data (this propagates also deletion)
                    if (password.UDT > offlinePassword.UDT)
                    {
                        response.SendingNewData = true;
                        responsePasswords.Add(password);
                        continue;
                    }

                    // offline DB has newer data
                    if (password.UDT < offlinePassword.UDT)
                    {
                        // the newest change is saved
                        if (password.UDT <= offlinePassword.UDTServerTime)
                        {
                            response.SendingNewData = true;
                            var newPassword = await _dataServiceWrapper.PasswordService.UpdateAsync(userId, offlinePassword);
                            responsePasswords.Add(newPassword);
                        }
                        // server data was changed between syncing - collision
                        else
                        {
                            response.SyncCollision = true;
                            response.SendingNewData = true;
                            // the saved password between syncing is used for new password so no data is accidentaly lost
                            password.PasswordName = $"Collision (data from {password.UDT}): {password.PasswordName}";
                            var newPasswordConflicting = await _dataServiceWrapper.PasswordService.CreateAsync(userId, password);
                            responsePasswords.Add(newPasswordConflicting);
                            // the newest change is saved
                            var updatedPassword = await _dataServiceWrapper.PasswordService.UpdateAsync(userId, offlinePassword);
                            responsePasswords.Add(updatedPassword);
                        }
                    }
                    // For last option where UDT is same so no changes were made no code is necessary...

                }

                response.SyncSuccessful = true;
                response.Passwords = responsePasswords;
            }
            catch (Exception ex)
            {
                return new SyncResponseDTO() { SyncSuccessful = false };
            }

            return response;
        }

        public static DateTime GetLastChangeDateTime(IEnumerable<PasswordDTO> passwords)
        {
            return passwords.Max(x => x.UDT);
        }

    }
}
