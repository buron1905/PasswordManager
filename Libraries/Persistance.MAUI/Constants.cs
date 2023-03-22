namespace Persistance.MAUI
{
    public static class Constants
    {
        public const string DatabaseFilename = "PasswordManagerSQLite.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache |
            // The file is encrypted and inaccessible while the device is locked
            SQLite.SQLiteOpenFlags.ProtectionComplete;

        public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }
}
