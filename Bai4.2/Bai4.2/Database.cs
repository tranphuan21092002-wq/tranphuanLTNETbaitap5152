
using MySql.Data.MySqlClient;

namespace Bai4._2
{
    public static class Database
    {
        private static readonly string connectionString =
            "Server=127.0.0.1;Port=3306;Database=QuanLyTaiKhoan;Uid=root;Pwd=;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public static bool TestConnection()
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                return conn.State == System.Data.ConnectionState.Open;
            }
        }

    }
}
