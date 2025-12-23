using System;
using System.Data;
using System.Data.SQLite;

namespace NodeApp.Helpers
{
    public static class SQLiteHelper
    {
        private static string _connectionString;

        public static void Init(string dbPath = "Memos.db")
        {
            _connectionString = $"Data Source={dbPath};Version=3;";
            ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS Memos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FileName TEXT NOT NULL UNIQUE,
                    Content TEXT,
                    CreateTime DATETIME NOT NULL,
                    ModifyTime DATETIME NOT NULL
                );");
        }

        public static int ExecuteNonQuery(string sql, params SQLiteParameter[] parameters)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddRange(parameters);
                        return cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("SQL执行失败：" + ex.Message);
                }
            }
        }

        public static DataTable ExecuteQuery(string sql, params SQLiteParameter[] parameters)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddRange(parameters);
                        using (var adapter = new SQLiteDataAdapter(cmd))
                        {
                            var dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("SQL查询失败：" + ex.Message);
                }
            }
        }

        public static object ExecuteScalar(string sql, params SQLiteParameter[] parameters)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddRange(parameters);
                        return cmd.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("SQL查询失败：" + ex.Message);
                }
            }
        }
    }
}