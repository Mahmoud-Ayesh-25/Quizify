namespace Quizify_Project.Classes
{
    public class clsPagesSettings
    {
        public enum enAnimationMode { Load, In, Out, InBack, OutBack}

        public static enAnimationMode coursesAnimationMode = enAnimationMode.Load;
        public static enAnimationMode lessonsAnimationMode = enAnimationMode.In;

        public static int selectedCourseID = -1;
        public static int selectedlessonID = -1;
        public static string selectedCourseTitle = "";
        public static string selectedLessonTitle = "";
    }
}
