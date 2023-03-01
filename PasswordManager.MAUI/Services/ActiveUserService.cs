using Models.DTOs;

namespace PasswordManager.MAUI.Services
{
    public class ActiveUserService
    {
        public UserDTO UserDTO { get; set; }
        public string Password { get; set; }

        private static ActiveUserService instance;
        private ActiveUserService()
        {

        }

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

        public bool IsActive
        {
            get
            {
                return UserDTO != null;
            }
        }

        public void Login(UserDTO user, string password)
        {
            UserDTO = user;
            Password = password;
        }

        public void Logout()
        {
            UserDTO = null;
            Password = null;
        }

    }
}
