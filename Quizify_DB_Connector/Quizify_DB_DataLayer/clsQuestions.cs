using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Quizify_DB_DataLayer
{
    public class clsQuestions
    {
        static string tableName = "Questions";
        static string questionIDColumnName = "QuestionID";

        public static async Task<DataTable> GetAllQuestions()
        {
            return await clsMainMethods.GetAllData(tableName);
        }

        public static async Task<List<object>> GetQuestion(int questionID)
        {
            return await clsMainMethods.GetData(tableName, questionIDColumnName, questionID);
        }

        public static async Task<int> GetQuestionCountByLessonID(int lessonID, SqlConnection connection)
        {
            return await clsMainMethods.GetDataCount(tableName, "LessonID", lessonID, connection);
        }

        public static async Task<int> AddNewQuestion(string head, string answer, int lessonID)
        {
            Dictionary<string, object> columnsAndValues = new Dictionary<string, object>();

            columnsAndValues.Add("Head", head);
            columnsAndValues.Add("Answer", answer);
            columnsAndValues.Add("LessonID", lessonID);

            return await clsMainMethods.AddNewData(tableName, columnsAndValues);
        }

        public static async Task<bool> UpdateQuestion(int questionID, string head, string answer, int lessonID)
        {
            Dictionary<string, object> columnsAndValues = new Dictionary<string, object>();

            columnsAndValues.Add("Head", head);
            columnsAndValues.Add("Answer", answer);
            columnsAndValues.Add("LessonID", lessonID);

            return await clsMainMethods.UpdateData(tableName, questionIDColumnName, questionID, columnsAndValues);
        }

        public static async Task<bool> DeleteQuestion(int questionID)
        {
            return await clsMainMethods.DeleteData(tableName, questionIDColumnName, questionID);
        }
        public static async Task<DataTable> GetAllQuestionsByLessonID(int lessonID)
        {
            return await clsMainMethods.GetAllDataByColumnID(tableName, "LessonID", lessonID);
        }

        public static async Task<bool> DeleteAllQuestionByLessonID(int lessonID)
        {
            return await clsMainMethods.DeleteData(tableName, "LessonID", lessonID);
        }
    }
}
