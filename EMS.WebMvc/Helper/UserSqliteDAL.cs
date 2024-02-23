using System.Data.SQLite;

namespace EMS.WebMvc.Helper
{
    public class UserSqliteDAL
    {
        private DatabaseHelper dbHelper;

        public UserSqliteDAL(string dbFilePath)
        {
            dbHelper = new DatabaseHelper(dbFilePath);
        }

        public void CreateUser(string username, string email)
        {
            using (var connection = dbHelper.GetConnection())
            {
                connection.Open();
                string sql = "INSERT INTO users (username, email) VALUES (@Username, @Email)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Email", email);
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
