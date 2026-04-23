using System;
using System.Diagnostics;
using System.IO;

namespace Quizify_DB_DataLayer
{
    public class clsSettings
    {
        static string mdf_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Quizify", "Quizify_DB.mdf");

        public static string ConnectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={mdf_path};Integrated Security=True;Connect Timeout=30";

        public static void CreateErrorEventLog(string ex)
        {
            string sourceName = "Quizify";

            EventLog.WriteEntry(sourceName, ex, EventLogEntryType.Error);
        }
    }
}
