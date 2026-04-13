using Microsoft.Data.SqlClient;
using Quizify_DB_DataLayer;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Quizify_DB_BusinessLayer
{
    public class clsLesson
    {
        enum enMode { AddNew, Update}
        enMode _Mode;

        public int lessonID { get; set; }
        public string title { get; set; }
        public int courseID { get; set; }
        public int questionsCount { get; set; }

        public clsLesson()
        {
            _Mode = enMode.AddNew;

            lessonID = -1;
            title = string.Empty;
            courseID = -1;
        }

        clsLesson(int lessonID, string title, int courseID, int questionsCount)
        {
            _Mode = enMode.Update;

            this.lessonID = lessonID;
            this.title = title;
            this.courseID = courseID;
            this.questionsCount = questionsCount;
        }

        public static async Task<DataTable> GetAllLessons()
        {
            return await Quizify_DB_DataLayer.clsLessons.GetAllLessons();
        }

        public static async Task<DataTable> GetAllLessonsByCourseID(int courseID)
        {
            return await Quizify_DB_DataLayer.clsLessons.GetAllLessonsByCourseID(courseID);
        }

        public static async Task<clsLesson> GetLesson(int lessonID)
        {
            List<object> data = await Quizify_DB_DataLayer.clsLessons.GetLesson(lessonID);

            string title = (string)data[1];
            int courseID = (int)data[2];
            int questionsCount;

            using (SqlConnection connection = new SqlConnection(Quizify_DB_DataLayer.clsSettings.ConnectionString))
            {
                await connection.OpenAsync();

                questionsCount = await Quizify_DB_DataLayer.clsQuestions.GetQuestionCountByLessonID(lessonID, connection);
            }

            return new clsLesson(lessonID, title, courseID, questionsCount);
        }

        async Task<bool> _Add()
        {
            lessonID = await Quizify_DB_DataLayer.clsLessons.AddNewLesson(title, courseID);

            return lessonID != -1;
        }

        async Task<bool> _Update()
        {
            return await Quizify_DB_DataLayer.clsLessons.UpdateLesson(lessonID, title, courseID);
        }

        public async Task<bool> Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                    { if (await _Add()) { _Mode = enMode.Update; return true; } else { return false; } }
                case enMode.Update:
                    { return await _Update(); }
                default: return false;
            }
        }

        public static async Task<bool> DeleteLesson(int lessonID)
        {
            return await Quizify_DB_DataLayer.clsLessons.DeleteLesson(lessonID);
        }

        public static async Task<int> GetLessonsCountByCourseID(int courseID, SqlConnection connection)
        {
            return await clsLessons.GetLessonsCountByCourseID(courseID, connection);
        }
    }
}
