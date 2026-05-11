using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Quizify_Project.UserControls
{
    /// <summary>
    /// Interaction logic for MainButton.xaml
    /// </summary>
    public partial class LessonButton : UserControl
    {
        public int ID { get; }

        public string Title 
        {   get
            {
                return TitleText.Text;
            } 
            set 
            {
                TitleText.Text = value;
            } 
        }

        public string QuestionsCount { get; }

        public LessonButton(int id, string title, int questionsCount)
        {
            ID = id;
            QuestionsCount = $"{questionsCount} Questions";

            this.DataContext = this;

            InitializeComponent();

            EditBTN.Tag = id;
            RemoveBTN.Tag = id;

            Title = title;

            this.CacheMode = new BitmapCache()
            {
                RenderAtScale = 1.4
            };
        }

        private void MainBorderStopEvent_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void EditBTN_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickAnim1.To = 1.04;
            ClickAnim2.To = 1.04;

            MainBorder.MouseUp += MainBorderStopEvent_MouseDown;
        }

        private void EditBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ClickAnim1.To = 1.02;
            ClickAnim2.To = 1.02;

            MainBorder.MouseUp -= MainBorderStopEvent_MouseDown;
        }

        private void RemoveBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ClickAnim1.To = 1.02;
            ClickAnim2.To = 1.02;

            MainBorder.MouseUp -= MainBorderStopEvent_MouseDown;
        }
    }
}
