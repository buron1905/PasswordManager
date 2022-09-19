namespace PasswordManager.WebAPI
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public AppSettings(string secret)
        {
            Secret = secret;
        }
    }
}
