using Quizify_DB_BusinessLayer;
using Quizify_Project.Classes;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Quizify_Project.Windows
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CreateQuiz : Window
    {
        public delegate void Finished();
        public event Finished OnFinished;

        DataTable questions = new DataTable();
        int questionsCount = 0;
        int selectedQuestionCount = 0;
        int selectedQuestionTime = 0;

        public CreateQuiz()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        void SetButtons()
        {
            Quizify_Project.UserControls.Button close = new Quizify_Project.UserControls.Button(new SolidColorBrush(Color.FromArgb(255, 24, 30, 60)), new SolidColorBrush(Color.FromArgb(255, 63, 68, 98)));
            Quizify_Project.UserControls.Button startQuiz = new Quizify_Project.UserControls.Button(new SolidColorBrush(Color.FromArgb(255, 21, 92, 40)), new SolidColorBrush(Color.FromArgb(255, 65, 164, 92)));

            close.Width = 100;
            close.Margin = new Thickness(0, 10, 10, 12);

            startQuiz.Width = 140;
            startQuiz.Margin = new Thickness(0, 10, 10, 12);

            close.Text = "Close";
            startQuiz.Text = "Start Quiz";

            close.MouseUp += CloseBTN_MouseUp;
            startQuiz.MouseUp += StartQuiz_MouseUp;

            ButtonsSP.Children.Add(startQuiz);
            ButtonsSP.Children.Add(close);
        }

        string SetPathText()
        {
            string pathText;

            if (clsPagesSettings.selectedCourseTitle != string.Empty)
            {
                pathText = clsPagesSettings.selectedCourseTitle;
                if (clsPagesSettings.selectedLessonTitle != string.Empty)
                {
                    pathText += "/" + clsPagesSettings.selectedLessonTitle;
                }
            }
            else
            {
                pathText = "All Courses";
            }

            return pathText;
        }

        async Task<DataTable> GetQuestions()
        {
            DataTable questions = new DataTable();

            if (clsPagesSettings.selectedCourseID == -1)
            {
                questions = await clsQuestion.GetAllQuestions();

                questionsCount = questions.Rows.Count;
                
                return questions;
            }

            if (clsPagesSettings.selectedlessonID == -1)
            {
                DataTable lessons = await clsLesson.GetAllLessonsByCourseID(clsPagesSettings.selectedCourseID);

                for (int i = 0; i < lessons.Rows.Count; i++)
                {
                    DataTable lessonQuestions = await clsQuestion.GetAllQuestionsByLessonID(int.Parse(lessons.Rows[i][0].ToString()));

                    questionsCount = questions.Rows.Count;

                    questions.Merge(lessonQuestions);
                }

                return questions;
            }

            questions = await clsQuestion.GetAllQuestionsByLessonID(clsPagesSettings.selectedlessonID);

            return questions;
        }

        void SetQuestionTimeComboBox()
        {
            QuesitonTimeCB.name = "Question Time (Minutes)";

            QuesitonTimeCB.AddItem("Unlimited");
            QuesitonTimeCB.AddItem("1");
            QuesitonTimeCB.AddItem("2");
            QuesitonTimeCB.AddItem("3");
            QuesitonTimeCB.AddItem("4");
            QuesitonTimeCB.AddItem("5");

            QuesitonTimeCB.SetSelectedItem(0);
        }

        void SetQuestionCountComboBex()
        {
            QuestionCountCB.name = "Questions Count";

            if (questionsCount >= 5)
                QuestionCountCB.AddItem("5");

            if (questionsCount >= 10)
                QuestionCountCB.AddItem("10");

            if (questionsCount >= 15)
                QuestionCountCB.AddItem("15");

            if (questionsCount >= 20)
                QuestionCountCB.AddItem("20");

            if (questionsCount >= 25)
                QuestionCountCB.AddItem("25");

            if (questionsCount >= 30)
                QuestionCountCB.AddItem("30");

            QuestionCountCB.AddItem("All");

            QuestionCountCB.SetSelectedItem(0);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.CacheMode = new BitmapCache();

            try
            {
                questions = await GetQuestions();
                questionsCount = questions.Rows.Count;
            }
            catch { CustomMessageBox.Show("An error occurred while fetching the questions.", MessageBoxImage.Error, DBErrorFullMessage.FullMessage); }

            if (questions.Rows.Count == 0)
            {
                CustomMessageBox.Show("There are no questions.", MessageBoxImage.Warning);
                await CloseWindow();
            }

            SetButtons();
            PathText.Text = SetPathText();

            SetQuestionCountComboBex();
            SetQuestionTimeComboBox();

            await Task.Delay(TimeSpan.FromMilliseconds(300));

            this.CacheMode = null;
        }

        async Task CloseWindow()
        {
            await CloseAnimation();

            base.Close();
        }

        async Task CloseAnimation()
        {
            this.CacheMode = new BitmapCache();

            DoubleAnimation fadeOut = new DoubleAnimation(0, TimeSpan.FromMilliseconds(200));

            var scaleX = new DoubleAnimation(1, 0.7, TimeSpan.FromMilliseconds(200));
            var scaleY = new DoubleAnimation(1, 0.7, TimeSpan.FromMilliseconds(200));

            WindowScale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleX);
            WindowScale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleY);

            this.BeginAnimation(Window.OpacityProperty, fadeOut);

            await Task.Delay(TimeSpan.FromMilliseconds(200));
        }

        private async void CloseBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            await CloseWindow();
        }

        private void DragBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        DataTable OrganizeQuizQuestions()
        {
            DataTable quizQuestions = questions.Clone();
            quizQuestions.Rows.Clear();

            Random rand = new Random();

            while (quizQuestions.Rows.Count < selectedQuestionCount)
            {
                int num = rand.Next(0, questions.Rows.Count);

                DataRow row = questions.Rows[num];

                quizQuestions.ImportRow(row);
                questions.Rows.RemoveAt(num);
            }

            return quizQuestions;
        }

        private async void StartQuiz_MouseUp(object sender, MouseButtonEventArgs e)
        {
            clsQuizSettings.questions = OrganizeQuizQuestions();
            clsQuizSettings.questionTime = selectedQuestionTime * 60;
            OnFinished?.Invoke();
            await CloseWindow();
        }

        private void QuestionCountCB_OnSelectedItemChanged(string newSelectedItem)
        {
            if (newSelectedItem == "All")
            {
                selectedQuestionCount = questionsCount;

                QuestionCountCB.SelectedText.Text = $"Questions Count  {selectedQuestionCount}";

                return;
            }

            selectedQuestionCount = int.Parse(newSelectedItem);
        }

        private void QuesitonTimeCB_OnSelectedItemChanged(string newSelectedItem)
        {
            if (newSelectedItem == "Unlimited")
            {
                selectedQuestionTime = 0;
                return;
            }

            selectedQuestionTime = int.Parse(newSelectedItem);
        }
    }
}
