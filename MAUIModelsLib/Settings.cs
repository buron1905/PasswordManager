using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIModelsLib
{
    public class Settings
    {
        [PrimaryKey]
        public int UserId { get; set; }
        [NotNull]
        public bool SavePassword { get; set; } = true;
    }
}
