using Quizify_Project.Classes;
using Quizify_Project.UserControls;
using Quizify_Project.Windows;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Quizify_Project.Pages
{
    /// <summary>
    /// Interaction logic for pgReviewAnswers.xaml
    /// </summary>
    public partial class pgReviewAnswers : Page
    {
        int questionsReviewed = 0;
        int questionsCount = 0;

        public pgReviewAnswers()
        {
            InitializeComponent();
        }

        async Task SetElements()
        {
            foreach(DataRow row in clsQuizSettings.questionsForReview.Rows)
            {
                ucAnswerReview answerReview = new ucAnswerReview(row[0].ToString(),
                    row[1].ToString(), row[2].ToString(), int.Parse(row[3].ToString()), int.Parse(row[4].ToString()));

                answerReview.Margin = new Thickness(10);
                answerReview.OnScoreSelected += AnswerReview_OnScoreSelected;

                if (row[2].ToString().Length != 0)
                    questionsCount++;

                answerReview.QuestionNumber = $"{questionsCount}/{clsQuizSettings.questionsForReview.Rows.Count}";

                ElementsContainer.Children.Add(answerReview);

                await Task.Delay(TimeSpan.FromMilliseconds(100));
            }
        }

        private void AnswerReview_OnScoreSelected()
        {
            questionsReviewed++;
        }

        void SetButtons()
        {
            UserControls.Button exit = new UserControls.Button(new SolidColorBrush(Color.FromArgb(255, 24, 30, 60)), new SolidColorBrush(Color.FromArgb(255, 63, 68, 98)));
            UserControls.Button showResult = new UserControls.Button(new SolidColorBrush(Color.FromArgb(255, 21, 92, 40)), new SolidColorBrush(Color.FromArgb(255, 65, 164, 92)));

            exit.Width = 100;
            exit.Height = 40;
            showResult.Width = 200;
            showResult.Height = 50;

            exit.Text = "Exit";
            showResult.Text = "Show Result";

            exit.Margin = new Thickness(0, 0, 25, 0);

            exit.VerticalAlignment = VerticalAlignment.Bottom;
            showResult.VerticalAlignment = VerticalAlignment.Bottom;

            exit.MouseUp += ExitBTN_MouseUp;
            showResult.MouseUp += ShowResultBTN_MouseUp;

            ShowResultSP.Children.Add(showResult);
            ExitSP.Children.Add(exit);
        }

        string SetPath()
        {
            string path;

            if (clsPagesSettings.selectedCourseID == -1)
            {
                return "All Courses";
            }
            else
            {
                path = clsPagesSettings.selectedCourseTitle;

                if (clsPagesSettings.selectedlessonID != -1)
                {
                    path += $"/{clsPagesSettings.selectedLessonTitle}";
                }
            }

            return path;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PathText.Text = SetPath();
            await SetElements();
            SetButtons();
        }

        async void ExitBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (CustomMessageBox.ShowWithBackShadow("Are you sure you want to return to the main menu?", MessageBoxImage.Question))
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

        async void ShowResultBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.IsEnabled = false;

            if (questionsReviewed < questionsCount)
            {
                if (!CustomMessageBox.ShowWithBackShadow("Would you like to consider the remaining questions as incorrect?", MessageBoxImage.Question))
                {
                    this.IsEnabled = true;
                    return;
                }
            }

            clsQuizSettings.questionCount = clsQuizSettings.questionsForReview.Rows.Count;
            clsQuizSettings.questionsAnswered = questionsReviewed;

            float score = 0;

            foreach (ucAnswerReview answerReview in ElementsContainer.Children)
            {
                if (answerReview.score != -1)
                    score += answerReview.score;
            }

            clsQuizSettings.score = score;

            DoubleAnimation opacity = new DoubleAnimation(0, TimeSpan.FromMilliseconds(200));
            this.BeginAnimation(OpacityProperty, opacity);

            await Task.Delay(TimeSpan.FromMilliseconds(200));

            this.NavigationService.Source = new Uri(@"Pages/FinalResult.xaml", UriKind.Relative); 
        }
    }
}
