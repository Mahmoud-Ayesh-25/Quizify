using System.Diagnostics;

namespace Quizify_DB_DataLayer
{
    public class clsSettings
    {
        public static string ConnectionString = "Server=.;Database=Quizify_DB;User Id=sa;Password=123456;TrustServerCertificate=True;";

        public static void CreateErrorEventLog(string ex)
        {
            string sourceName = "Quizify";

            EventLog.WriteEntry(sourceName, ex, EventLogEntryType.Error);
        }
    }
}
