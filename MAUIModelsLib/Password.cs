using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIModelsLib
{
    public class Password
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public int UserId { get; set; }
        public string PasswordName { get; set; }
        public string UserName { get; set; }
        public string PasswordEncrypted { get; set; }
        public string Description { get; set; }
    }
}
