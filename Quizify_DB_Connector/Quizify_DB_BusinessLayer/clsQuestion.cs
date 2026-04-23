using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Quizify_DB_BusinessLayer
{
    public class clsQuestion
    {
        enum enMode { AddNew, Update}
        enMode _Mode;

        public int questionID { get; set; }
        public string head { get; set; }
        public string answer { get; set; }
        public int lessonID { get; set; }

        public clsQuestion()
        {
            _Mode = enMode.AddNew;

            questionID = -1;
            head = string.Empty;
            answer = string.Empty;
            lessonID = -1;
        }

        clsQuestion(int questionID, string head, string answer, int lessonID)
        {
            _Mode = enMode.Update;

            this.questionID = questionID;
            this.head = head;
            this.answer = answer;
            this.lessonID = lessonID;
        }

        public static async Task<DataTable> GetAllQuestions()
        {
            return await Quizify_DB_DataLayer.clsQuestions.GetAllQuestions();
        }

        public static async Task<DataTable> GetAllQuestionsByLessonID(int lessonID)
        {
            return await Quizify_DB_DataLayer.clsQuestions.GetAllQuestionsByLessonID(lessonID);
        }

        public static async Task<clsQuestion> GetQuestion(int questionID)
        {
            List<object> data = await Quizify_DB_DataLayer.clsQuestions.GetQuestion(questionID);

            string head = (string)data[1];
            string answer = (string)data[2];
            int lessonID = (int)data[3];
            
            return new clsQuestion(questionID, head, answer, lessonID);
        }

        async Task<bool> _Add()
        {
            questionID = await Quizify_DB_DataLayer.clsQuestions.AddNewQuestion(head, answer, lessonID);
            return questionID != -1;
        }

        async Task<bool> _Update()
        {
            return await Quizify_DB_DataLayer.clsQuestions.UpdateQuestion(questionID, head, answer, lessonID);
        }

        public async Task<bool> Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    { if (await _Add()) { _Mode = enMode.Update; return true; } else { return false; } }
                case enMode.Update:
                    { return await _Update(); }
                default: return false;
            }
        }

        public static async Task<bool> DeleteQuestion(int questionID)
        {
            return await Quizify_DB_DataLayer.clsQuestions.DeleteQuestion(questionID);
        }

        public static async Task<bool> DeleteAllQuestionByLessonID(int lessonID)
        {
            return await Quizify_DB_DataLayer.clsQuestions.DeleteAllQuestionByLessonID(lessonID);
        }

        public static async Task<int> GetQuestionsCountByLessonID(int lessonID, SqlConnection connection)
        {
            return await Quizify_DB_DataLayer.clsQuestions.GetQuestionCountByLessonID(lessonID, connection);
        }
    }
}
