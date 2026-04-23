using Quizify_Project.Classes;
using Quizify_Project.UserControls;
using Quizify_Project.Windows;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Quizify_Project.Pages
{
    /// <summary>
    /// Interaction logic for pgQuiz.xaml
    /// </summary>
    public partial class pgQuiz : Page
    {
        int time = clsQuizSettings.questionTime * clsQuizSettings.questions.Rows.Count;
        DispatcherTimer dt = new DispatcherTimer();

        bool isThereUnAnsweredQuestions = false;

        public pgQuiz()
        {
            InitializeComponent();
        }

        async Task SetElements()
        {
            Progress.Value = 0;
            float pregressIncrease = 100 / clsQuizSettings.questions.Rows.Count;

            int i = 0;

            foreach(DataRow row in clsQuizSettings.questions.Rows)
            {
                i++;

                ucQuestion question = new ucQuestion(row[1].ToString(), Color.FromRgb(255, 255, 255));

                if (ElementsContainer.Children.Count == clsQuizSettings.questions.Rows.Count - 1)
                {
                    question = new ucQuestion(row[1].ToString(), Color.FromArgb(0, 255, 255, 255));
                }

                question.QuestionNumberText.Text = $"{i}/{clsQuizSettings.questions.Rows.Count}";

                ElementsContainer.Children.Add(question);

                await Task.Delay(50);

                Progress.Value += pregressIncrease;
            }
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

        void SetTimer()
        {
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += Timer_Tick;
            dt.Start();
        }

        void SetButtons()
        {
            UserControls.Button quit = new UserControls.Button(new SolidColorBrush(Color.FromArgb(255, 24, 30, 60)), new SolidColorBrush(Color.FromArgb(255, 63, 68, 98)));
            UserControls.Button submit = new UserControls.Button(new SolidColorBrush(Color.FromArgb(255, 21, 92, 40)), new SolidColorBrush(Color.FromArgb(255, 65, 164, 92)));

            quit.Width = 100;
            quit.Height = 40;
            submit.Width = 200;
            submit.Height = 50;

            quit.Text = "Exit";
            submit.Text = "Submit Answers";

            quit.Margin = new Thickness(0, 0, 25, 0);

            quit.VerticalAlignment = VerticalAlignment.Bottom;
            submit.VerticalAlignment = VerticalAlignment.Bottom;

            quit.MouseUp += ExitBTN_MouseUp;
            submit.MouseUp += Submit_MouseUp;

            SubmitPanal.Children.Add(submit);
            QuitPanal.Children.Add(quit);
        }

        void ShowContent()
        {
            ProgressStackPanal.Visibility = Visibility.Hidden;
            ProgressStackPanal.IsEnabled = false;

            ContentGrid.IsEnabled = true;

            DoubleAnimation anim = new DoubleAnimation(1, TimeSpan.FromMilliseconds(100));

            ContentGrid.BeginAnimation(OpacityProperty, anim);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ContentGrid.IsEnabled = false;
            ContentGrid.Opacity = 0;

            PathText.Text = SetPath();
            SetButtons();
            await SetElements();
            ShowContent();

            if (time > 0)
            {
                SetTimer();
            }
        }

        async void Timer_Tick(object sender, EventArgs e)
        {
            time--;

            TimeSpan ts = TimeSpan.FromSeconds(time);

            TimeText.Text = ts.ToString(@"hh\:mm\:ss");

            if (time <= 30)
            {
                TimeText.Foreground = new SolidColorBrush(Colors.Red);
            }

            if (time == 0)
            {
                dt.Stop();

                if (CustomMessageBox.ShowWithBackShadow("Time is up!", MessageBoxImage.Information) == false)
                {
                    await Submit(true);
                }
            }
        }

        async void ExitBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (time > 0)
            {
                dt.Stop();
            }

            if (CustomMessageBox.ShowWithBackShadow("Are you sure you want to exit the quiz?", MessageBoxImage.Question))
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
            else
            {
                this.IsEnabled = true;

                if  (time > 0)
                {
                    dt.Start();
                }
            }
        }

        async Task Submit(bool timeIsUp = false)
        {
            this.IsEnabled = false;

            clsQuizSettings.questionsForReview = new DataTable();

            clsQuizSettings.questionsForReview.Columns.Add("Question", typeof(string));
            clsQuizSettings.questionsForReview.Columns.Add("UserAnswer", typeof(string));
            clsQuizSettings.questionsForReview.Columns.Add("CorrectAnswer", typeof(string));
            clsQuizSettings.questionsForReview.Columns.Add("QuestionRTL", typeof(int));
            clsQuizSettings.questionsForReview.Columns.Add("AnswerRTL" , typeof(int));

            for (int i = 0; i < ElementsContainer.Children.Count; i++)
            {
                ucQuestion question = (ucQuestion)ElementsContainer.Children[i];

                if (question.AnswerTextBX.Text.Length == 0)
                    isThereUnAnsweredQuestions = true;

                int questionRTL = (question.QuestionText.FlowDirection == FlowDirection.RightToLeft) ? 1 : 0;
                int answerRTL = (question.AnswerTextBX.FlowDirection == FlowDirection.RightToLeft) ? 1 : 0;

                clsQuizSettings.questionsForReview.Rows.Add(question.QuestionText.Text,
                    clsQuizSettings.questions.Rows[i][2].ToString(), question.AnswerTextBX.Text, questionRTL, answerRTL);
            }

            dt.Stop();

            if (!timeIsUp)
            {
                if (isThereUnAnsweredQuestions)
                {
                    if (!CustomMessageBox.ShowWithBackShadow("There are unanswered questions. Would you like to ignore them?", MessageBoxImage.Question))
                    {
                        dt.Start();
                        this.IsEnabled = true;
                        return;
                    }
                }
            }

            DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromMilliseconds(200));
            this.BeginAnimation(OpacityProperty, animation);

            await Task.Delay(TimeSpan.FromMilliseconds(200));

            this.NavigationService.Source = new Uri("Pages/pgReviewAnswers.xaml", UriKind.Relative);
        }

        async void Submit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            await Submit();
        }
    }
}
