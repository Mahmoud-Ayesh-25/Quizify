namespace Quizify_Project.Classes
{
    public class clsQuestionPageEventsHelper
    {
        public delegate void EditComplete(int id, string head, string answer);
        public static event EditComplete OnEditComplete;

        public delegate void RemoveComplete(int id);
        public static event RemoveComplete OnRemoveComplete;

        public static void InvokeEditComplete(int id, string head, string answer)
        {
            OnEditComplete?.Invoke(id, head, answer);
        }

        public static void InvokeRemoveComplete(int id)
        {
            OnRemoveComplete?.Invoke(id);
        }
    }
}
