using Models.DTOs;
using PasswordManager.MAUI.ViewModels;

namespace PasswordManager.MAUI.Services
{
    public class ActiveUserService
    {
        #region Properties

        public UserDTO ActiveUser { get; set; }
        public string CipherKey { get; set; }
        public string Token { get; set; }

        public bool IsActive
        {
            get
            {
                return ActiveUser != null;
            }
        }

        private static ActiveUserService instance;

        #endregion

        private ActiveUserService()
        {
        }

        #region Methods

        public static ActiveUserService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActiveUserService();
                }

                return instance;
            }
        }

        public void Login(UserDTO user, string cipherKey)
        {
            ActiveUser = user;
            ActiveUser.Password = null;
            CipherKey = cipherKey;
            AppShellViewModel.FlyoutHeaderRefresh();
        }

        public void Logout()
        {
            ActiveUser = null;
            CipherKey = null;
            instance = null;
            SecureStorage.Remove("token");
            AppShellViewModel.FlyoutHeaderRefresh();
        }

        #endregion

    }
}
