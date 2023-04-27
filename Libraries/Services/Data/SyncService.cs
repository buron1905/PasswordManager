using Models.DTOs;
using Services.Abstraction.Data;

namespace Services.Data
{
    public class SyncService : ISyncService
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;

        public SyncService(IUserService userService, IPasswordService passwordService)
        {
            _userService = userService;
            _passwordService = passwordService;
        }

        public async Task<LastChangeResponseDTO?> GetLastChangeDateTime(Guid userId)
        {
            var lastChangeUser = (await _userService.GetByIdAsync(userId)).UDT;
            var passwords = await _passwordService.GetAllByUserIdAsync(userId);
            var lastChangePassword = GetLastChangeDateTime(passwords);

            var response = new LastChangeResponseDTO() { LastChangeUser = lastChangeUser, LastChangePassword = lastChangePassword };
            return response;
        }

        // SyncPasswords - jen pro hesla, uzivatel se da menit jen online, a pri authResponse se jeho DTO posila a synchronizuje s pouzitou lokalni DB
        public async Task<SyncResponseDTO?> SyncAccount(SyncRequestDTO data)
        {
            var response = new SyncResponseDTO();
            try
            {
                var userId = data.UserDTO.Id;
                var userDTO = await _userService.GetByIdAsync(userId);
                response.UserDTO = userDTO;

                var offlinePasswords = data.Passwords.ToList();
                var syncedOfflinePasswords = offlinePasswords.Where(x => x.UDT != default || x.UDT != DateTime.MinValue && x.Id != Guid.Empty).ToList();
                var onlinePasswords = (await _passwordService.GetAllByUserIdAsync(data.UserDTO.Id)).ToList();
                var newOfflinePasswords = offlinePasswords.Where(x => x.UDT == default || x.UDT == DateTime.MinValue
                                                            && !onlinePasswords.Any(o => o.Id == x.Id)).ToList();
                var newOnlinePasswords = onlinePasswords.Where(x => !offlinePasswords.Any(o => o.Id == x.Id)).ToList();
                var deletedOnlinePasswords = offlinePasswords.Where(x => !onlinePasswords.Any(o => o.Id == x.Id)
                                                                    && syncedOfflinePasswords.Any(s => s.Id == x.Id)).ToList();
                var responsePasswords = new List<PasswordDTO>();

                // offline DB has no rows, all from server is send
                if (syncedOfflinePasswords.Count() == 0)
                {
                    response.SyncSuccessful = true;
                    response.SendingNewData = true;
                    response.SyncCollision = false;
                    var lastChangePassword = GetLastChangeDateTime(onlinePasswords);
                    response.LastChangeDate = userDTO.UDT > lastChangePassword ? userDTO.UDT : lastChangePassword;
                    response.Passwords = onlinePasswords;
                    return response;
                }

                // offline DB has new passwords, save them online
                if (newOfflinePasswords.Count() > 0)
                {
                    response.SendingNewData = true;
                    foreach (var password in newOfflinePasswords)
                    {
                        responsePasswords.Add(await _passwordService.CreateAsync(userId, password));
                    }
                }

                // passwords stored in local DB were deleted online, delete them in local DB
                if (deletedOnlinePasswords.Count() > 0)
                {
                    response.SendingNewData = true;
                    foreach (var password in deletedOnlinePasswords)
                    {
                        password.UDT = DateTime.UtcNow;
                        password.UDTLocal = password.UDT;
                        password.DDT = password.UDT;
                        password.Deleted = true;

                        responsePasswords.Add(password);
                    }
                }

                // Adding new online passwords to local DB
                if (newOnlinePasswords.Count() > 0)
                {
                    response.SendingNewData = true;
                    responsePasswords.AddRange(newOnlinePasswords);
                }

                // handling conflicts of synced passwords
                foreach (var password in onlinePasswords)
                {
                    var offlinePassword = syncedOfflinePasswords.SingleOrDefault(x => x.Id == password.Id);
                    if (offlinePassword is null)
                        continue;

                    // Server has newer data
                    if (password.UDT > offlinePassword.UDTLocal)
                    {
                        response.SendingNewData = true;
                        responsePasswords.Add(password);
                        continue;
                    }

                    // offline DB has newer data
                    if (password.UDT < offlinePassword.UDTLocal)
                    {
                        // Offline DB was up to date - the change is saved
                        if (password.UDT <= offlinePassword.UDT)
                        {
                            response.SendingNewData = true;
                            if (offlinePassword.Deleted)
                            {
                                await _passwordService.DeleteAsync(userId, offlinePassword);
                                offlinePassword.UDT = DateTime.UtcNow;
                                offlinePassword.UDTLocal = offlinePassword.UDT;
                                offlinePassword.DDT = offlinePassword.UDT;
                                offlinePassword.Deleted = true;
                                responsePasswords.Add(offlinePassword);
                                continue;
                            }
                            var updatedPassword = await _passwordService.UpdateAsync(userId, offlinePassword);
                            responsePasswords.Add(updatedPassword);
                        }
                        // server data was changed between syncing - collision
                        else
                        {
                            response.SyncCollision = true;
                            response.SendingNewData = true;
                            // the saved password between syncing is used for new password so no data is accidentaly lost
                            password.PasswordName = $"Collision (data from {password.UDT}): {password.PasswordName}";
                            if (offlinePassword.Deleted)
                            {
                                await _passwordService.DeleteAsync(userId, offlinePassword);
                                offlinePassword.UDT = DateTime.UtcNow;
                                offlinePassword.UDTLocal = offlinePassword.UDT;
                                offlinePassword.DDT = offlinePassword.UDT;
                                offlinePassword.Deleted = true;
                                responsePasswords.Add(offlinePassword);
                            }
                            else
                            {
                                // the newest change is saved
                                var updatedPassword = await _passwordService.UpdateAsync(userId, offlinePassword);
                                responsePasswords.Add(updatedPassword);
                            }

                            var newPasswordConflicting = await _passwordService.CreateAsync(userId, password);
                            responsePasswords.Add(newPasswordConflicting);

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
            if (passwords.Count() == 0)
                return DateTime.MinValue;

            return passwords.Max(x => x.UDT);
        }

    }
}
