using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace PasswordManager.Models
{
    internal class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id {  get; set; }
        [Unique]
        public string Email {  get; set; }
        public string Name {  get; set; }
        public string Surname {  get; set; }
        [NotNull]
        public string Password {  get; set; }
    }
}
