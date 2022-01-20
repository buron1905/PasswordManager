using MAUIModelsLib;
using Microsoft.Maui.Essentials;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;


// use SQL commands

namespace MAUIDatabaseLib
{
    public static class DatabaseSQLiteMobile
    {
        static SQLiteAsyncConnection _connection;
        static SQLiteConnection connection;

        public static async void Init()
        {
            if (_connection != null)
                return;

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "PasswordManagerLocal.db");

            File.Delete(dbPath);

            _connection = new SQLiteAsyncConnection(dbPath);


            await _connection.ExecuteAsync("PRAGMA foreign_keys = ON;");

            await _connection.ExecuteAsync(@"CREATE TABLE Users(
	                Id				INTEGER PRIMARY KEY,
	                Email			VARCHAR(320) NOT NULL UNIQUE,
	                PasswordHASH	TEXT NOT NULL
                );"
            );

            await _connection.ExecuteAsync(@"CREATE TABLE Passwords(
	                Id					INTEGER PRIMARY KEY,
	                PasswordName		TEXT NOT NULL,
	                UserName			TEXT NOT NULL,
	                PasswordText       	TEXT NOT NULL,
	                Description			TEXT,
                    UserId              INTEGER REFERENCES Users(Id)
                );"
            );

            await _connection.ExecuteAsync(@"CREATE TABLE Settings(
	                Id					INTEGER PRIMARY KEY,
	                SavePassword		INT NOT NULL DEFAULT 1,
                    UserId              INTEGER REFERENCES Users(Id)
                );"
            );

            //await _connection.ExecuteAsync("ALTER TABLE Passwords ADD COLUMN UserId INTEGER REFERENCES Users(Id);");
            //await _connection.ExecuteAsync("ALTER TABLE Settings ADD COLUMN UserId INTEGER REFERENCES Users(Id);");
        }

        public static async Task<bool> RemoveUser(int id)
        {
            if (await _connection.DeleteAsync<User>(id) == 1)
                return true;
            else
                return false;
        }

        public static async Task<int> AddUser(User user)
        {
            if (user == null)
                return -1;

            await _connection.InsertAsync(user);
            return await _connection.ExecuteScalarAsync<int>("select seq from sqlite_sequence where name=\"Users\"");
            //return await _connection.ExecuteScalarAsync<int>("select last_inset_rowid()");
        }

        public static async Task<int> Add(Settings settings)
        {
            return await _connection.InsertAsync(settings);
        }

        public static async Task<int> Add(Password password)
        {
            return await _connection.InsertAsync(password);
        }

        public static async Task<User?> GetUser(string email, string hashedPassword)
        {
            var queryResult = await _connection.QueryAsync<User>("SELECT * FROM Users WHERE Email ='?' AND Password ='?'", email, hashedPassword);

            if (queryResult.Count() == 1)
            {
                return queryResult[0];
            }
            else
            {
                return await Task.FromResult<User?>(null);
            }
        }

        public static async Task<User?> GetUser(string email)
        {
            //if(email != "")
            //{
            //    return await _connection.Table<User>().Where(i => i.Email == email).FirstOrDefaultAsync();
            //}


            var queryResult = await _connection.QueryAsync<User>("SELECT * FROM Users WHERE Email LIKE ?", email);

            if (queryResult.Count() == 1)
            {
                return queryResult[0];
            }
            else
            {
                return await Task.FromResult<User?>(null);
            }
        }

        public static async Task<Settings?> GetSettings(int userId)
        {
            var queryResult = await _connection.QueryAsync<Settings>("SELECT * FROM Settings WHERE UserId =?", userId);

            if (queryResult.Count() == 1)
            {
                return queryResult[0];
            }
            else
            {
                return await Task.FromResult<Settings?>(null);
            }
        }

        public static async Task<List<Settings>> GetSettings2(int userId)
        {
            return await _connection.QueryAsync<Settings>("SELECT * FROM Settings WHERE UserId ='?'", userId);
            //var queryResult = await _connection.QueryAsync<Settings>("SELECT * FROM Settings WHERE UserId ='?'", userId);
        }

        public static async Task<List<User>> GetPeopleAsync()
        {
            return await _connection.Table<User>().ToListAsync();
        }

        public static async Task<List<Settings>> GetSettingsTable()
        {
            return await _connection.Table<Settings>().ToListAsync();
        }

        public static async Task<List<Password>> GetPasswords()
        {
            return await _connection.Table<Password>().ToListAsync();
        }

        public static void UpdateWithChildren(User user)
        {
            connection.UpdateWithChildren(user);
        }

        public static User GetWithChildren(int userId)
        {
            return connection.GetWithChildren<User>(userId);
        }


        //public static async Task<IEnumerable<User>> GetPasswords(User user)
        //{
        //    await Init();
        //    var users = await _connection.Table<User>().ToListAsync();
        //    return users;
        //}

    }
}
