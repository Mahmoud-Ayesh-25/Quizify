using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Quizify_DB_BusinessLayer
{
    public class clsCourse
    {
        enum enMode { AddNew, Update};
        enMode _Mode;

        public int courseID { get; set; }
        public string title { get; set; }
        public string color { get; set; }
        public string iconPath { get; set; }
        public int lessonsCount { get; set; }

        public clsCourse()
        {
            _Mode = enMode.AddNew;

            courseID = -1;
            title = string.Empty;
            color = string.Empty;
            iconPath = string.Empty;
        }

        clsCourse(int courseID, string title, string color, string iconPath, int lessonsCount)
        {
            _Mode = enMode.Update;

            this.courseID = courseID;
            this.title = title;
            this.color = color;
            this.iconPath = iconPath;
            this.lessonsCount = lessonsCount;
        }

        public static async Task<DataTable> GetAllCourses()
        {
                return await Quizify_DB_DataLayer.clsCourses.GetAllCourses();
        }

        public static async Task<DataTable> GetAllCoursesWithLessonsCount()
        {
            return await Quizify_DB_DataLayer.clsCourses.GetAllCoursesWithLessonsCount();
        }

        public static async Task<clsCourse> GetCourse(int courseID)
        {
            List<object> data = await Quizify_DB_DataLayer.clsCourses.GetCourse(courseID);

            string title = (string)data[1];
            string color = (string)data[2];
            string iconPath = (string)data[3];
            int lessonsCount;

            using (SqlConnection connection = new SqlConnection(Quizify_DB_DataLayer.clsSettings.ConnectionString))
            {
                await connection.OpenAsync();

                lessonsCount = await Quizify_DB_DataLayer.clsLessons.GetLessonsCountByCourseID(courseID, connection);
            }

            return new clsCourse(courseID, title, color, iconPath, lessonsCount);
        }

        async Task<bool> _Add()
        {
            courseID = await Quizify_DB_DataLayer.clsCourses.AddNewCourse(title, color, iconPath);

            return courseID != -1;
        }

        async Task<bool> _Update()
        {
            return await Quizify_DB_DataLayer.clsCourses.UpdateCourse(courseID, title, color, iconPath);
        }

        public async Task<bool> Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    {
                        if (await _Add()) { _Mode = enMode.Update; return true; }
                        else { return false; }
                    }
                case enMode.Update: return await _Update();
                default: return false;
            }
        }

        public static async Task<bool> DeleteCourse(int courseID)
        {
            return await Quizify_DB_DataLayer.clsCourses.DeleteCourse(courseID);
        }
    }
}
