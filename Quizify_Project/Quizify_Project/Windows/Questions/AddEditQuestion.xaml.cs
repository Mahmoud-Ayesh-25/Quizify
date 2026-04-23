using Quizify_DB_BusinessLayer;
using Quizify_Project.Classes;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Quizify_Project.Windows.Questions
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class AddEditQuestion : Window
    {
        public delegate void SaveComplete(int id, string head, string answer);
        public event SaveComplete OnSaveComplete;

        enum enMode { Add, Edit}
        enMode _Mode;

        clsQuestion question;
        int questionID;

        double questionRowHeight;
        double answerRowHeight;

        Quizify_Project.UserControls.Button save;

        public AddEditQuestion(int id)
        {
            InitializeComponent();

            this.DataContext = this;

            questionID = id;

            _Mode = enMode.Edit;
        }

        public AddEditQuestion()
        {
            InitializeComponent();

            this.DataContext = this;

            _Mode = enMode.Add;
        }

        double TotalHeight()
        {
            return 120 + questionRowHeight + answerRowHeight;
        }

        void SetButtons()
        {
            Quizify_Project.UserControls.Button close = new Quizify_Project.UserControls.Button(new SolidColorBrush(Color.FromArgb(255, 24, 30, 60)), new SolidColorBrush(Color.FromArgb(255, 63, 68, 98)));
            save = new Quizify_Project.UserControls.Button(new SolidColorBrush(Color.FromArgb(255, 21, 40, 97)), new SolidColorBrush(Color.FromArgb(255, 65, 92, 164)));

            close.Width = 100;
            close.Margin = new Thickness(0, 10, 10, 12);

            save.Width = 100;
            save.Margin = new Thickness(0, 10, 10, 12);

            close.Text = "Close";
            save.Text = "Save";

            close.MouseUp += CloseBTN_MouseUp;
            save.MouseUp += Save_MouseUp;

            ButtonsSP.Children.Add(save);
            ButtonsSP.Children.Add(close);
        }
        void SetHeight()
        {
            QuestionText.Height = (QuestionText.LineCount * 20) + 25;
            AnswerText.Height = (AnswerText.LineCount * 20) + 25;

            questionRowHeight = QuestionText.Height + 65;
            answerRowHeight = AnswerText.Height + 65;

            if (questionRowHeight + answerRowHeight <= 420)
            {
                QuestionRow.Height = new GridLength(questionRowHeight);
                AnswerRow.Height = new GridLength(answerRowHeight);
            }

            if (TotalHeight() < 540)
            {
                this.Height = TotalHeight();
                this.MinHeight = TotalHeight();
                this.MaxHeight = TotalHeight();
            }
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_Mode == enMode.Add)
            {
                EdgeText.Text = "Add Question";
            }
            else
            {
                EdgeText.Text = "Edit Question";
            }

            SetButtons();

            save.Opacity = 0.5;
            save.IsEnabled = false;

            AnswerRTLText.MouseUp += AnswerSwitch.MainBorder_MouseUp;
            QuesitonRTLText.MouseUp += QuestionSwitch.MainBorder_MouseUp;

            try
            {
                if (_Mode == enMode.Edit)
                {
                    question = await clsQuestion.GetQuestion(questionID);
                }
                else
                {
                    question = new clsQuestion();
                }
            }
            catch 
            {
                if (CustomMessageBox.Show("An error occurred while fetching the question data from the database.", MessageBoxImage.Error, DBErrorFullMessage.FullMessage) == false)
                {
                    await CloseWindow();
                }
            }

            QuestionText.Text = question.head;
            AnswerText.Text = question.answer;

            SetHeight();
        }

        async Task CloseWindow()
        {
            await CloseAnimation();

            base.Close();
        }

        async Task CloseAnimation()
        {
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
            if (QuestionText.Text == string.Empty && AnswerText.Text == string.Empty)
            {
                await CloseWindow();
            }
            else
            {
                if (_Mode == enMode.Edit)
                {
                    if (QuestionText.Text == question.head && AnswerText.Text == question.answer)
                    {
                        await CloseWindow();
                    }
                    else
                    {
                        if (CustomMessageBox.Show("Are you sure you want to close this window? You will lose all the changes you have made.", MessageBoxImage.Question) == true)
                        {
                            await CloseWindow();
                        }
                    }
                }
                else
                {
                    if (CustomMessageBox.Show("Are you sure you want to close this window? You will lose all the changes you have made.", MessageBoxImage.Question) == true)
                    {
                        await CloseWindow();
                    }
                }
            }
        }

        private void DragBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void QuestionSwitch_OnStatusChanged(bool status)
        {
            if (status)
                QuestionText.FlowDirection = FlowDirection.RightToLeft;
            else
                QuestionText.FlowDirection = FlowDirection.LeftToRight;
        }

        private void AnswerSwitch_OnStatusChanged(bool status)
        {
            if (status)
                AnswerText.FlowDirection = FlowDirection.RightToLeft;
            else
                AnswerText.FlowDirection = FlowDirection.LeftToRight;
        }

        private void QuestionText_TextChanged()
        {
            SetHeight();

            if (QuestionText.Text == string.Empty || AnswerText.Text == string.Empty)
            {
                save.Opacity = 0.5;
                save.IsEnabled = false;
            }
            else
            {
                if (_Mode == enMode.Edit)
                {
                    if (QuestionText.Text == question.head && AnswerText.Text == question.answer)
                    {
                        save.Opacity = 0.5;
                        save.IsEnabled = false;
                    }
                    else
                    {
                        save.Opacity = 1;
                        save.IsEnabled = true;
                    }
                }
                else
                {
                    save.Opacity = 1;
                    save.IsEnabled = true;
                }
            }
        }

        private async void Save_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_Mode == enMode.Add)
            {
                clsQuestion question = new clsQuestion();

                question.head = QuestionText.Text;
                question.answer = AnswerText.Text;
                question.lessonID = clsPagesSettings.selectedlessonID;

                try
                {
                    await question.Save();

                    CustomMessageBox.Show("The question has been saved successfully.", MessageBoxImage.Information);

                    this.question = question;
                    questionID = question.questionID;

                    _Mode = enMode.Edit;

                    EdgeText.Text = "Edit Question";

                    OnSaveComplete?.Invoke(questionID, question.head, question.answer);

                    await CloseWindow();
                }
                catch { CustomMessageBox.Show("An error occurred while saving the question.", MessageBoxImage.Error, DBErrorFullMessage.FullMessage); }
            }
            else
            {
                question.head = QuestionText.Text;
                question.answer = AnswerText.Text;

                try
                {
                    await question.Save();

                    CustomMessageBox.Show("The question has been saved successfully.", MessageBoxImage.Information);

                    OnSaveComplete?.Invoke(questionID, question.head, question.answer);

                    await CloseWindow();
                }
                catch { CustomMessageBox.Show("An error occurred while saving the question.", MessageBoxImage.Error, DBErrorFullMessage.FullMessage); }
            }
        }
    }
}
