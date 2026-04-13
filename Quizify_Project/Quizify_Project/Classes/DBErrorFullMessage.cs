using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify_Project.Classes
{
    public class DBErrorFullMessage
    {
        public static string FullMessage = @"The appearance of this message means that the connection to the database has failed. There are several possible reasons for this:

1- The database does not exist in its expected location:
The expected path is:
C:\Users\YourUserName\AppData\Local\Quizify

If you do not find the following two files in this path:
Quizify_DB.mdf
Quizify_DB_log.ldf

Then this means that you may have deleted them or moved them to another location.
If you know where the files are, move them back to the correct location. If you do not know, the easiest solution is to reinstall Quizify.

Warning: Reinstalling Quizify will delete all your previously created data such as courses, lessons, and questions.

2- LocalDB is not installed:
Quizify requires LocalDB 2022 in order to connect to the database. It is supposed to be installed automatically with the program, but if an error occurred during installation, you will need to install it manually or reinstall Quizify.

Don’t worry—if the database files mentioned above already exist, Quizify will not replace them during reinstallation, this means that you will keep your data on the program.
";
    }
}
