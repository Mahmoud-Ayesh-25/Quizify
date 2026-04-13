using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Quizify_DB_DataLayer
{
    public class clsMainMethods
    {
        public static async Task<DataTable> GetAllData(string tableName)
        {
            DataTable dt = new DataTable();

            string query = $@"SELECT * FROM {tableName};";

            using (SqlConnection connection = new SqlConnection(clsSettings.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await  command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                }
                catch (Exception ex) { clsSettings.CreateErrorEventLog(ex.ToString()); throw; }
            }

            return dt;
        }

        public static async Task<DataTable> GetAllDataByColumnID(string tableName, string columnName, int id)
        {
            DataTable dt = new DataTable();

            string query = $@"SELECT * FROM {tableName}
                            WHERE {columnName} = @ID;";

            using (SqlConnection connection = new SqlConnection(clsSettings.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", id);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                }
                catch (Exception ex) { clsSettings.CreateErrorEventLog(ex.ToString()); throw; }
            }

            return dt;
        }

        public static async Task<List<object>> GetData(string tableName, string idColumnName, int id)
        {
            List<object> dataList = new List<object>();

            string query = $@"SELECT * FROM {tableName}
                            WHERE {idColumnName} = @id;";

            using (SqlConnection connection = new SqlConnection(clsSettings.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    dataList.Add(reader.GetValue(i) ?? DBNull.Value);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { clsSettings.CreateErrorEventLog(ex.ToString()); throw; }
            }

            return dataList;
        }

        public static async Task<int> AddNewData(string tableName, Dictionary<string, object> values)
        {
            int newID = -1;

            string query = $@"INSERT INTO {tableName}
                            VALUES
                            ({string.Join(", ", values.Select(v => $"@{v.Key}"))})
                            SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsSettings.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        foreach (KeyValuePair<string, object> kvp in values)
                        {
                            command.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                        }

                        object result = await command.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int id))
                        {
                            newID = id;
                        }
                    }
                }
                catch (Exception ex) { clsSettings.CreateErrorEventLog(ex.ToString()); throw; }
            }

            return newID;
        }

        public static async Task<bool> UpdateData(string tableName, string idColumnName, int id, Dictionary<string, object> columns)
        {
            bool isUpdate = false;

            string query = $@"UPDATE {tableName}
                            SET {string.Join(", ", columns.Select(v => $"{v.Key} = @{v.Key}"))}
                            WHERE {idColumnName} = @id;";

            using (SqlConnection connection = new SqlConnection(clsSettings.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        foreach (KeyValuePair<string, object> pair in columns)
                        {
                            command.Parameters.AddWithValue($"@{pair.Key}", pair.Value ?? DBNull.Value);
                        }

                        int rowsEffected = await command.ExecuteNonQueryAsync();

                        isUpdate = (rowsEffected > 0);
                    }
                }
                catch (Exception ex) { clsSettings.CreateErrorEventLog(ex.ToString()); throw; }
            }

            return isUpdate;
        }

        public static async Task<bool> DeleteData(string tableName, string idColumnName, int id)
        {
            bool isDeleted = false;

            string query = $@"DELETE FROM {tableName} WHERE {idColumnName} = @id;";

            using (SqlConnection connection = new SqlConnection(clsSettings.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        int rowsEffected = await command.ExecuteNonQueryAsync();

                        isDeleted = (rowsEffected > 0);
                    }
                }
                catch (Exception ex) { clsSettings.CreateErrorEventLog(ex.ToString()); throw; }
            }

            return isDeleted;
        }

        public static async Task<int> GetDataCount(string tableName, string targetColumnName, object targetValue, SqlConnection connection)
        {
            int count = -1;

            string query = $@"SELECT COUNT(*) FROM {tableName} WHERE {targetColumnName} = @value;";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@value", targetValue);

                object result = await command.ExecuteScalarAsync();

                if (result != null && int.TryParse(result.ToString(), out int rows))
                {
                    count = rows;
                }
            }

            return count;
        }
    }
}
