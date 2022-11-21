using Models;

namespace PasswordManager.MAUI.Services
{
    public class ActiveUserService      //normální heslo v ActiveUserService s vlastnosti heslo
    {
        public User User { get; set; }
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
                return User != null;
            }
        }

        public void Login(User user, string password)
        {
            User = user;
            Password = password;
        }

        public void Logout()
        {
            User = null;
            Password = null;
        }

    }
}
