using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIModelsLib
{
    [Table("Users")]
    public class User
    {
        [PrimaryKey]
        public int Id { get; set; }
        [Unique, NotNull]
        public string Email { get; set; }
        [NotNull]
        public string PasswordHASH { get; set; }

    }
}
