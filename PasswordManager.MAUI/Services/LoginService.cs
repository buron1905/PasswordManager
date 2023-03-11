namespace PasswordManager.MAUI.Services
{
    public class LoginService
    {
        public static async Task<bool> Login(string email, string password)
        {
            if (email == "admin@admin" && password == "asdf")
                return true;

            //string passwordHASH = HashingService.HashSHA512ToString(password);

            //User user = null; // await DatabaseService.GetUser(email);
            //if (user != null)
            //{
            //    if (passwordHASH == user.PasswordHASH)
            //    {
            //        //ActiveUserService.Instance.Login(user, password);
            //        return true;
            //    }
            //}

            return false;
        }
    }
}
