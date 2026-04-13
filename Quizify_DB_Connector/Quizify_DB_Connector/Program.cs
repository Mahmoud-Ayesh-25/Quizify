using Quizify_DB_BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify_DB_Connector
{
    internal class Program
    {
        static async Task TestGetAll()
        {
            DataTable dt = await Quizify_DB_BusinessLayer.clsCourse.GetAllCourses();

            foreach (DataRow dr in dt.Rows)
            {
                Console.WriteLine($"{dr[0]} - {dr[1]} - {dr[2]} - {dr[3]}");
            }
        }

        static async Task TestAddAndUpdate()
        {
            clsCourse course = await clsCourse.GetCourse(1002);

            course.title = "Database Level 2";
            course.color = "Orange";
            course.iconPath = "C:\\Users\\MYMY2\\source\\repos\\Quizify_Project\\Quizify_Project\\Images\\Icons\\3D_Chart.png";

            Console.WriteLine(course.lessonsCount);
        }

        static async Task TestDelete()
        {
            if (await clsCourse.DeleteCourse(1003))
            {
                Console.WriteLine("Deleted");
            }
        }

        static async Task Main(string[] args)
        {
            await TestAddAndUpdate();
        }
    }
}
