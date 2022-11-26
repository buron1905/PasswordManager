﻿using Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace MAUIDatabaseLib
{
	public static class DatabaseSQLiteMobile
	{
		static SQLiteAsyncConnection _connection;


		public static async void Init()
		{
			if (_connection != null)
				return;

			string dbPath = Path.Combine(FileSystem.AppDataDirectory, "PasswordManagerLocal.db");

			//File.Delete(dbPath);

			_connection = new SQLiteAsyncConnection(dbPath);


			await _connection.ExecuteAsync("PRAGMA foreign_keys = ON;");

			await _connection.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS Users(
					Id				INTEGER PRIMARY KEY AUTOINCREMENT,
					Email			VARCHAR(320) NOT NULL UNIQUE,
					PasswordHASH	TEXT NOT NULL
				);"
			);

			await _connection.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS Passwords(
					Id					INTEGER PRIMARY KEY AUTOINCREMENT,
					PasswordName		TEXT NOT NULL,
					UserName			TEXT NOT NULL,
					PasswordText       	TEXT NOT NULL,
					Description			TEXT,
					UserId              INTEGER REFERENCES Users(Id) ON DELETE CASCADE
				);"
			);

			await _connection.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS Settings(
					UserId				INTEGER PRIMARY KEY REFERENCES Users(Id) ON DELETE CASCADE,
					SavePassword		INT NOT NULL DEFAULT 1
				);"
			);
		}


		public static async Task<int> AddUser(User user)
		{
			if (user == null)
				return -1;
			return await _connection.InsertAsync(user);
		}

		public static async Task<int> AddSettings(Settings settings)
		{
			return await _connection.InsertAsync(settings);
		}

		public static async Task<int> AddPassword(Password password)
		{
			return await _connection.InsertAsync(password);
		}


		public static async Task<int> UpdateUser(User user)
		{
			return await _connection.UpdateAsync(user);
		}

		public static async Task<int> UpdateSettings(Settings settings)
		{
			return await _connection.UpdateAsync(settings);
		}

		public static async Task<int> UpdatePassword(Password password)
		{
			return await _connection.UpdateAsync(password);
		}


		public static async Task<bool> RemoveUser(int id)
		{
			if (await _connection.DeleteAsync<User>(id) == 1)
				return true;
			else
				return false;
		}

		public static async Task<bool> RemoveSettings(int id)
		{
			if (await _connection.DeleteAsync<Settings>(id) == 1)
				return true;
			else
				return false;
		}

		public static async Task<bool> RemovePassword(Guid id)
		{
			if (await _connection.DeleteAsync<Password>(id) == 1)
				return true;
			else
				return false;
		}


		public static async Task<List<User>> GetUsersTableAsync()
		{
			return await _connection.Table<User>().ToListAsync();
		}

		public static async Task<List<Settings>> GetSettingsTableAsync()
		{
			return await _connection.Table<Settings>().ToListAsync();
		}

		public static async Task<List<Password>> GetPasswordsTable()
		{
			return await _connection.Table<Password>().ToListAsync();
		}


		public static async Task<User?> GetUser(int id)
		{
			return await _connection.FindAsync<User>(id);
		}

		public static async Task<Settings?> GetSettings(Guid userId)
		{
			return await _connection.FindAsync<Settings>(userId);
		}

		public static async Task<Password?> GetPassword(int id)
		{
			return await _connection.FindAsync<Password>(id);
		}

		public static async Task<User?> GetUser(string email)
		{
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

		public static async Task<List<Password>> GetUserPasswords(Guid userId)
		{
			return await _connection.QueryAsync<Password>("SELECT * FROM Passwords WHERE UserId = ?", userId);
		}

	}
}