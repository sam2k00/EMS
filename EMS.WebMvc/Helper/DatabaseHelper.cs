using System.Data.SQLite;

namespace EMS.WebMvc.Helper
{
    public class DatabaseHelper
    {
        private string connectionString;

        public DatabaseHelper(string dbFilePath)
        {
            connectionString = $"Data Source={dbFilePath};Version=3;";
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }
    }
}
