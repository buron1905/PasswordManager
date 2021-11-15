using MAUIDatabaseLib;
using Microsoft.Maui.Essentials;
using MAUIModelsLib;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    public static class DatabaseService
    {
        public static void Init()
        {
            DatabaseSQLiteMobile.Init();
        }

        public static async Task AddUser(User user)
        {
            await DatabaseSQLiteMobile.AddUser(user);
        }

        public static async Task RemoveUser(int id)
        {
            await DatabaseSQLiteMobile.RemoveUser(id);
        }

        public static async Task<User> GetUser(string email, string hashedPassword)
        {
            return await DatabaseSQLiteMobile.GetUser(email, hashedPassword);
        }

    }
    

}
