using System.Data;

namespace Quizify_Project.Classes
{
    public class clsQuizSettings
    {
        public static DataTable questions { get; set; }
        public static int questionTime { get; set; }

        public static DataTable questionsForReview { get; set; }

        public static int questionCount { get; set; }
        public static int questionsAnswered {  get; set; }
        public static float score { get; set; }
    }
}
