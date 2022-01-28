using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIModelsLib
{
    [Table("Settings")]
    public class Settings
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(User))]
        public int UserId { get; set; }
        [NotNull]
        public bool SavePassword { get; set; } = true;
    }
}
