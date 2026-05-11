using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Quizify_Project.UserControls
{
    /// <summary>
    /// Interaction logic for MainButton.xaml
    /// </summary>
    public partial class QuestionButton : UserControl
    {
        public int ID { get; }
        public string Question 
        {
            get
            {
                return QuestionText.Text;
            }
            set
            {
                if (!(value.EndsWith("?") || value.EndsWith("؟")))
                    value = value + "?";

                QuestionText.Text = $"Q: {value}";
            } 
        }
        public string Answer
        {
            get
            {
                return AnswerText.Text;
            }
            set
            {
                value = value.Replace("\r\n", " ");

                if (!value.EndsWith("."))
                    value = value + ".";

                AnswerText.Text = $"A: {value}";
            }
        }

        public QuestionButton(int id, string question, string answer)
        {
            ID = id;

            this.DataContext = this;

            InitializeComponent();

            EditBTN.Tag = id;
            RemoveBTN.Tag = id;

            Question = question;
            Answer = answer;

            this.CacheMode = new BitmapCache()
            {
                RenderAtScale = 1.2
            };
        }

        private void MainBorderStopEvent_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void EditBTN_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickAnim1.To = 1.02;
            ClickAnim2.To = 1.02;

            MainBorder.MouseUp += MainBorderStopEvent_MouseDown;
        }

        private void EditBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ClickAnim1.To = 1.01;
            ClickAnim2.To = 1.01;

            MainBorder.MouseUp -= MainBorderStopEvent_MouseDown;
        }

        private void RemoveBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ClickAnim1.To = 1.01;
            ClickAnim2.To = 1.01;

            MainBorder.MouseUp -= MainBorderStopEvent_MouseDown;
        }
    }
}
