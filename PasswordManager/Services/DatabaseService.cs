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

        public static async Task<int> Add(Settings settings)
        {
            return await DatabaseSQLiteMobile.Add(settings);
        }

        public static async Task<int> Add(Password password)
        {
            return await DatabaseSQLiteMobile.Add(password);
        }

        public static async Task RemoveUser(int id)
        {
            await DatabaseSQLiteMobile.RemoveUser(id);
        }

        public static async Task<User> GetUser(string email, string hashedPassword)
        {
            return await DatabaseSQLiteMobile.GetUser(email, hashedPassword);
        }

        public static async Task<User> GetUser(string email)
        {
            return await DatabaseSQLiteMobile.GetUser(email);
        }

        public static async Task<Settings> GetSettings(int userId)
        {
            return await DatabaseSQLiteMobile.GetSettings(userId);
        }

        public static async Task<List<User>> GetPeopleAsync()
        {
            return await DatabaseSQLiteMobile.GetPeopleAsync();
        }

        public static async Task<List<Password>> GetPasswords()
        {
            return await DatabaseSQLiteMobile.GetPasswords();
        }

        public static async Task<List<Settings>> GetSettingsTable()
        {
            return await DatabaseSQLiteMobile.GetSettingsTable();
        }

        public static void UpdateWithChildren(User user)
        {
            DatabaseSQLiteMobile.UpdateWithChildren(user);
        }

        public static User GetWithChildren(int userId)
        {
            return DatabaseSQLiteMobile.GetWithChildren(userId);
        }

    }


}
