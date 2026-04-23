using Quizify_Project.Classes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Quizify_Project.Pages
{
    /// <summary>
    /// Interaction logic for FinalResult.xaml
    /// </summary>
    public partial class FinalResult : Page
    {
        float persantage;

        public FinalResult()
        {
            InitializeComponent();
        }

        void SetLevel()
        {
            if (persantage < 50)
            {
                Level.Text = "BAD";
                Level.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (persantage < 70)
            {
                Level.Text = "ACCEPTABLE";
                Level.Foreground = new SolidColorBrush(Colors.Orange);
            }
            else if (persantage < 90)
            {
                Level.Text = "GOOD";
                Level.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                Level.Text = "EXCELLENT";
                Level.Foreground = new SolidColorBrush(Colors.LightBlue);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            persantage = (clsQuizSettings.score / clsQuizSettings.questionCount) * 100;

            AnsweredQuestions.Text = $"Questions  {clsQuizSettings.questionsAnswered} / {clsQuizSettings.questionCount}";
            Mark.Text = $"Mark  {clsQuizSettings.score} / {clsQuizSettings.questionCount}";
            Persentage.Text = $"{Math.Round(persantage)}%";

            SetLevel();

            UserControls.Button submit = new UserControls.Button(new SolidColorBrush(Color.FromArgb(255, 24, 30, 60)), new SolidColorBrush(Color.FromArgb(255, 63, 68, 98)));

            submit.Width = 200;
            submit.Height = 50;

            submit.Text = "Main Menu";

            submit.VerticalAlignment = VerticalAlignment.Bottom;

            submit.MouseUp += MainMenuBTN_MouseUp;

            ButtonContainer.Children.Add(submit);
        }

        async void MainMenuBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.IsEnabled = false;

            clsPagesSettings.selectedCourseTitle = string.Empty;
            clsPagesSettings.selectedLessonTitle = string.Empty;
            clsPagesSettings.selectedCourseID = -1;
            clsPagesSettings.selectedlessonID = -1;
            clsPagesSettings.coursesAnimationMode = clsPagesSettings.enAnimationMode.Load;

            DoubleAnimation opacity = new DoubleAnimation(0, TimeSpan.FromMilliseconds(200));
            this.BeginAnimation(OpacityProperty, opacity);

            await Task.Delay(TimeSpan.FromMilliseconds(200));

            this.NavigationService.Source = new Uri("Pages/Courses.xaml", UriKind.Relative);
        }
    }
}
