using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Quizify_DB_DataLayer
{
    public class clsCourses
    {
        static string tableName = "Courses";
        static string courseIDColumnName = "CourseID";

        public static async Task<DataTable> GetAllCourses()
        {
            return await clsMainMethods.GetAllData(tableName);
        }

        public static async Task<DataTable> GetAllCoursesWithLessonsCount()
        {
            DataTable dt = new DataTable();

            string query = $@"SELECT Courses.*, Count(Lessons.CourseID) AS LessonsCount FROM
                            Courses LEFT JOIN Lessons ON Lessons.CourseID = Courses.CourseID
                            GROUP BY Courses.CourseID, Courses.Title, Courses.Color, Courses.IconPath";

            using (SqlConnection connection = new SqlConnection(clsSettings.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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

        public static async Task<List<object>> GetCourse(int courseID)
        {
            return await clsMainMethods.GetData(tableName, courseIDColumnName, courseID);
        }

        public static async Task<int> AddNewCourse(string title, string color, string iconPath)
        {
            Dictionary<string, object> columnsAndValues = new Dictionary<string, object>();

            columnsAndValues.Add("Title", title);
            columnsAndValues.Add("Color", color);
            columnsAndValues.Add("IconPath", iconPath);

            return await clsMainMethods.AddNewData(tableName, columnsAndValues);
        }

        public static async Task<bool> UpdateCourse(int courseID, string title, string color, string iconPath)
        {
            Dictionary<string, object> columnsAndValues = new Dictionary<string, object>();

            columnsAndValues.Add("Title", title);
            columnsAndValues.Add("Color", color);
            columnsAndValues.Add("IconPath", iconPath);

            return await clsMainMethods.UpdateData(tableName, courseIDColumnName, courseID, columnsAndValues);
        }

        public static async Task<bool> DeleteCourse(int courseID)
        {
            return await clsMainMethods.DeleteData(tableName, courseIDColumnName, courseID);
        }
    }
}
