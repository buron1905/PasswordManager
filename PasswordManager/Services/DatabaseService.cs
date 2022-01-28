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


        public static async Task<int> AddUser(User user)
        {
            return await DatabaseSQLiteMobile.AddUser(user);
        }

        public static async Task<int> AddSettings(Settings settings)
        {
            return await DatabaseSQLiteMobile.AddSettings(settings);
        }

        public static async Task<int> AddPassword(Password password)
        {
            return await DatabaseSQLiteMobile.AddPassword(password);
        }


        public static async Task UpdateUser(User user)
        {
            await DatabaseSQLiteMobile.UpdateUser(user);
        }

        public static async Task UpdateSettings(Settings settings)
        {
            await DatabaseSQLiteMobile.UpdateSettings(settings);
        }

        public static async Task UpdatePassword(Password password)
        {
            await DatabaseSQLiteMobile.UpdatePassword(password);
        }


        public static async Task RemoveUser(int id)
        {
            await DatabaseSQLiteMobile.RemoveUser(id);
        }

        public static async Task RemoveSettings(int id)
        {
            await DatabaseSQLiteMobile.RemoveSettings(id);
        }

        public static async Task RemovePassword(int id)
        {
            await DatabaseSQLiteMobile.RemovePassword(id);
        }


        public static async Task<List<User>> GetUsersTableAsync()
        {
            return await DatabaseSQLiteMobile.GetUsersTableAsync();
        }

        public static async Task<List<Settings>> GetSettingsTableAsync()
        {
            return await DatabaseSQLiteMobile.GetSettingsTableAsync();
        }

        public static async Task<List<Password>> GetPasswordsTableAsync()
        {
            return await DatabaseSQLiteMobile.GetPasswordsTable();
        }


        public static async Task<User> GetUser(string email, string hashedPassword)
        {
            return await DatabaseSQLiteMobile.GetUser(email, hashedPassword);
        }

        public static async Task<User?> GetUser(string email)
        {
            return await DatabaseSQLiteMobile.GetUser(email);
        }

        public static async Task<Settings> GetSettings(int userId)
        {
            return await DatabaseSQLiteMobile.GetSettings(userId);
        }
    }
}
