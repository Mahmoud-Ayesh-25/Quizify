using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Quizify_DB_DataLayer
{
    public class clsLessons
    {
        static string tableName = "Lessons";
        static string lessonIDColumnName = "LessonID";

        public static async Task<DataTable> GetAllLessons()
        {
            return await clsMainMethods.GetAllData(tableName);
        }

        public static async Task<List<object>> GetLesson(int lessonID)
        {
            return await clsMainMethods.GetData(tableName, lessonIDColumnName, lessonID);
        }

        public static async Task<int> GetLessonsCountByCourseID(int courseID, SqlConnection connection)
        {
            return await clsMainMethods.GetDataCount(tableName, "CourseID", courseID, connection);
        }

        public static async Task<int> AddNewLesson(string title, int courseID)
        {
            Dictionary<string, object> columnsAndValues = new Dictionary<string, object>();

            columnsAndValues.Add("Title", title);
            columnsAndValues.Add("CourseID", courseID);

            return await clsMainMethods.AddNewData(tableName, columnsAndValues);
        }

        public static async Task<bool> UpdateLesson(int lessonID, string title, int courseID)
        {
            Dictionary<string, object> columnsAndValues = new Dictionary<string, object>();

            columnsAndValues.Add("Title", title);
            columnsAndValues.Add("CourseID", courseID);

            return await clsMainMethods.UpdateData(tableName, lessonIDColumnName, lessonID, columnsAndValues);
        }

        public static async Task<bool> DeleteLesson(int lessonID)
        {
            return await clsMainMethods.DeleteData(tableName, lessonIDColumnName, lessonID);
        }

        public static async Task<DataTable> GetAllLessonsByCourseID(int courseID)
        {
            return await clsMainMethods.GetAllDataByColumnID(tableName, "CourseID", courseID);
        }
    }
}
