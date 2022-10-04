namespace Models
{
    public class Password
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PasswordName { get; set; }
        public string UserName { get; set; }
        public string PasswordText { get; set; }
        public string PasswordDecrypted { get; set; }
        public string Description { get; set; }

        public void Encrypt()
        {
        }

        public void Decrypt()
        {
        }

        public Password DeepCopy()
        {
            Password other = (Password)this.MemberwiseClone();

            other.PasswordName = new string(PasswordName);
            other.PasswordText = new string(PasswordText);
            other.UserName = new string(UserName);
            other.Description = new string(Description);

            return other;
        }
    }
}
