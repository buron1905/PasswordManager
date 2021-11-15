using ModelsLib;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLib
{
    public static class DatabaseSQLiteMobile
    {
        static SQLiteAsyncConnection _connection;

        public static async void Init(string dbPath)
        {
            if (_connection != null)
                return;

            dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PasswordManagerLocal.db");

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

            if (queryResult.Count() == 1)
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
