using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIModelsLib
{
    [Table("Passwords")]
    public class Password
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(User))]
        public int UserId { get; set; }

        /// <summary>
        /// URL/Application name for password
        /// </summary>
        [NotNull]
        public string PasswordName { get; set; }

        /// <summary>
        /// User name/email/field for login
        /// </summary>
        [NotNull]
        public string UserName { get; set; }
        [NotNull]
        public string PasswordText { get; set; }
        [Ignore]
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
