using Microsoft.Maui.Essentials;
using PasswordManager.Models;
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
        static SQLiteAsyncConnection _connection;

        static async Task Init()
        {
            if (_connection != null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "PasswordManagerLocal.db");

            _connection = new SQLiteAsyncConnection(dbPath);

            await _connection.CreateTableAsync<User>();
        }

        public static async Task RemoveUser(int id)
        {
            await Init();

            await _connection.DeleteAsync<User>(id);
        }

        public static async Task AddUser(User user)
        {
            if (user == null)
                return;

            await Init();

            var id = await _connection.InsertAsync(user);
        }

        public static async Task GetUser(string email, string password)
        {
            await Init();

            await _connection.QueryAsync<User>("SELECT * FROM User WHERE Email ='?' AND Password ='?'", email, password);
        }

        //public static async Task<IEnumerable<User>> GetPasswords(User user)
        //{
        //    await Init();
        //    var users = await _connection.Table<User>().ToListAsync();
        //    return users;
        //}
    }
}
