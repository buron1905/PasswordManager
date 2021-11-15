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
    /*
    public static class DatabaseService
    {

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
    */

    public static class DatabaseService
    {
        static SQLiteAsyncConnection _connection;

        public static async void Init(string dbPath)
        {
            if (_connection != null)
                return;

            //dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "sqli.db");

            _connection = new SQLiteAsyncConnection(dbPath);

            await _connection.CreateTableAsync<User>();
            //await _connection.CreateTableAsync<Password>();
        }

        public static async Task<bool> RemoveUser(int id)
        {
            if (await _connection.DeleteAsync<User>(id) == 1)
                return true;
            else
                return false;
        }

        public static async Task<bool> AddUser(User user)
        {
            if (user == null)
                return false;

            var id = await _connection.InsertAsync(user);
            return true;
        }

        public static async Task<User?> GetUser(string email, string hashedPassword)
        {
            var queryResult = await _connection.QueryAsync<User>("SELECT * FROM User WHERE Email ='?' AND Password ='?'", email, hashedPassword);

            if (queryResult.Count == 1)
            {
                return queryResult[0];
            }
            else
            {
                return await Task.FromResult<User?>(null);
            }
        }

        //public static async Task<IEnumerable<User>> GetPasswords(User user)
        //{
        //    await Init();
        //    var users = await _connection.Table<User>().ToListAsync();
        //    return users;
        //}

    }
}
