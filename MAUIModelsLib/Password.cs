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

        //[ManyToOne]
        //public User User { get; set; }

        /// <summary>
        /// URL/Application name for password
        /// </summary>
        [NotNull]
        public string PasswordName { get; set; }

        /// <summary>
        /// User name/email/field for login
        /// </summary>
        public string UserName { get; set; }
        [NotNull]
        public string PasswordEncrypted { get; set; }
        [Ignore]
        public string PasswordDecrypted { get; set; }
        public string Description { get; set; }
    }
}
