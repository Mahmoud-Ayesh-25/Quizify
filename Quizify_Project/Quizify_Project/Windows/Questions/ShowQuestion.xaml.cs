using Microsoft.Win32;
using Quizify_DB_BusinessLayer;
using Quizify_Project.Classes;
using Quizify_Project.UserControls;
using System.Media;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Quizify_Project.Windows.Questions
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class ShowQuestion : Window
    {
        clsQuestion question;
        int questionID;

        double questionRowHeight;
        double answerRowHeight;

        public ShowQuestion(int id)
        {
            InitializeComponent();

            this.DataContext = this;

            questionID = id;
        }

        double TotalHeight()
        {
            return 130 + questionRowHeight + answerRowHeight;
        }

        void SetButtons()
        {
            Quizify_Project.UserControls.Button close = new Quizify_Project.UserControls.Button(new SolidColorBrush(Color.FromArgb(255, 24, 30, 60)), new SolidColorBrush(Color.FromArgb(255, 63, 68, 98)));
            Quizify_Project.UserControls.Button edit = new Quizify_Project.UserControls.Button(new SolidColorBrush(Color.FromArgb(255, 21, 40, 97)), new SolidColorBrush(Color.FromArgb(255, 65, 92, 164)));
            Quizify_Project.UserControls.Button remove = new Quizify_Project.UserControls.Button(new SolidColorBrush(Color.FromArgb(255, 72, 31, 57)), new SolidColorBrush(Color.FromArgb(255, 166, 71, 104)));

            close.Width = 100;
            close.Margin = new Thickness(0, 10, 10, 12);

            edit.Width = 100;
            edit.Margin = new Thickness(0, 10, 10, 12);

            remove.Width = 100;
            remove.Margin = new Thickness(0, 10, 10, 12);

            close.Text = "Close";
            edit.Text = "Edit";
            remove.Text = "Remove";

            close.MouseUp += CloseBTN_MouseUp;
            edit.MouseUp += Edit_MouseDown;
            remove.MouseUp += Remove_MouseDown;

            ButtonsSP.Children.Add(remove);
            ButtonsSP.Children.Add(edit);
            ButtonsSP.Children.Add(close);
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetButtons();

            AnswerRTLText.MouseUp += AnswerSwitch.MainBorder_MouseUp;
            QuesitonRTLText.MouseUp += QuestionSwitch.MainBorder_MouseUp;

            try
            {
                question = await clsQuestion.GetQuestion(questionID);
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

            if (!question.head.EndsWith("?"))
                QuestionText.Text += "?";

            if (!question.answer.EndsWith("."))
                AnswerText.Text += ".";

            QuestionText.Height = QuestionText.LineCount * 28;
            AnswerText.Height = AnswerText.LineCount * 28;

            questionRowHeight = QuestionText.Height + 75;
            answerRowHeight = AnswerText.Height + 75;

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
            await CloseWindow();
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

        private void Edit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddEditQuestion addEdit = new AddEditQuestion(questionID);

            addEdit.OnSaveComplete += clsQuestionPageEventsHelper.InvokeEditComplete;

            addEdit.ShowDialog();
        }

        private async void Remove_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CustomMessageBox.Show("Are you sure you want to remove this question?", MessageBoxImage.Question) == true)
            {
                try
                {
                    await clsQuestion.DeleteQuestion(questionID);

                    clsQuestionPageEventsHelper.InvokeRemoveComplete(questionID);

                    await CloseWindow();
                }
                catch { CustomMessageBox.Show("An error occurred while removing the question.", MessageBoxImage.Error, DBErrorFullMessage.FullMessage); }
            }
        }
    }
}
