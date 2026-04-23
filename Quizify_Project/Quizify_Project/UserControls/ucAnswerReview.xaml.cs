using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Quizify_Project.UserControls
{
    /// <summary>
    /// Interaction logic for ucAnswerReview.xaml
    /// </summary>
    public partial class ucAnswerReview : UserControl
    {
        public delegate void ScoreSelected();
        public event ScoreSelected OnScoreSelected;

        public float score {  get; set; }

        double questionRowHeight;
        double answerRowHeight;
        double yourAnswerRowHeight;

        string question;
        string answer;
        string yourAnswer;

        int questionRTL;
        int answerRTL;

        public string QuestionNumber
        {
            set { QuestionNumberText.Text = value; }
        }

        public ucAnswerReview(string question, string answer, string yourAnswer, int questionRTL, int answerRTL)
        {
            score = -1;

            this.question = question;
            this.answer = answer;
            this.yourAnswer = yourAnswer;

            this.questionRTL = questionRTL;
            this.answerRTL = answerRTL;

            InitializeComponent();
        }

        void SetScoreButtons()
        {
            ColorButton full = new ColorButton();
            full.Back_Ground = new SolidColorBrush(Color.FromArgb(255, 20, 150, 50));
            full.Border_Brush = Color.FromArgb(255, 50, 255, 50);
            full.Text = "1";
            full.Width = 50;
            full.Height = 50;
            full.MouseUp += Score_MouseUp;
            full.Margin = new Thickness(5);

            ColorButton threeFour = new ColorButton();
            threeFour.Back_Ground = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
            threeFour.Border_Brush = Color.FromArgb(100, 255, 255, 255);
            threeFour.Text = "3/4";
            threeFour.Width = 50;
            threeFour.Height = 50;
            threeFour.MouseUp += Score_MouseUp;
            threeFour.Margin = new Thickness(5);

            ColorButton half = new ColorButton();
            half.Back_Ground = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
            half.Border_Brush = Color.FromArgb(100, 255, 255, 255);
            half.Text = "1/2";
            half.Width = 50;
            half.Height = 50;
            half.MouseUp += Score_MouseUp;
            half.Margin = new Thickness(5);

            ColorButton quarter = new ColorButton();
            quarter.Back_Ground = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
            quarter.Border_Brush = Color.FromArgb(100, 255, 255, 255);
            quarter.Text = "1/4";
            quarter.Width = 50;
            quarter.Height = 50;
            quarter.MouseUp += Score_MouseUp;
            quarter.Margin = new Thickness(5);

            ColorButton zero = new ColorButton();
            zero.Back_Ground = new SolidColorBrush(Color.FromArgb(255, 150, 50, 50));
            zero.Border_Brush = Color.FromArgb(255, 255, 50, 50);
            zero.Text = "0";
            zero.Width = 50;
            zero.Height = 50;
            zero.MouseUp += Score_MouseUp;
            zero.Margin = new Thickness(5);

            ScoreContainer.Children.Add(full);
            ScoreContainer.Children.Add(threeFour);
            ScoreContainer.Children.Add(half);
            ScoreContainer.Children.Add(quarter);
            ScoreContainer.Children.Add(zero);
        }

        double TotalHeight()
        {
            return 75 + questionRowHeight + answerRowHeight + yourAnswerRowHeight;
        }

        public void SetNoAnswer()
        {
            UserAnswerText.Text = $"You did not answer this question.";
            UserAnswerText.Foreground = new SolidColorBrush(Colors.Red);
            this.IsEnabled = false;
            this.Opacity = 0.8;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (questionRTL == 1)
                QuestionText.FlowDirection = FlowDirection.RightToLeft;

            if (answerRTL == 1)
            {
                AnswerText.FlowDirection = FlowDirection.RightToLeft;
                UserAnswerText.FlowDirection= FlowDirection.RightToLeft;
            }

            if (!question.EndsWith("?"))
                question += "?";

            if (!answer.EndsWith("."))
                answer += ".";

            if (!yourAnswer.EndsWith("."))
                yourAnswer += ".";

            QuestionText.Text = question;
            AnswerText.Text = answer;
            UserAnswerText.Text = yourAnswer;

            if (yourAnswer == ".")
            {
                SetNoAnswer();
            }

            SetScoreButtons();

            QuestionText.Height = QuestionText.LineCount * 28;
            AnswerText.Height = AnswerText.LineCount * 28;
            UserAnswerText.Height = UserAnswerText.LineCount * 28;

            questionRowHeight = QuestionText.Height + 55;
            answerRowHeight = AnswerText.Height + 55;
            yourAnswerRowHeight = UserAnswerText.Height + 55;

            QuestionRow.Height = new GridLength(questionRowHeight);
            CurrectAnswer.Height = new GridLength(answerRowHeight);
            YourAnswer.Height = new GridLength(yourAnswerRowHeight);

            this.Height = TotalHeight();
            this.MinHeight = TotalHeight();
            this.MaxHeight = TotalHeight();
        }

        private void Score_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ColorButton sndr = (ColorButton)sender;

            for (int i = 0; i < ScoreContainer.Children.Count; i++)
            {
                ((ColorButton)ScoreContainer.Children[i]).SetUnChecked();
                ((ColorButton)sender).SetChecked();
            }

            if (score == -1)
                OnScoreSelected?.Invoke();

            if (sndr.Text == "3/4")
                score = 0.75f;
            else if (sndr.Text == "1/2")
                score = 0.5f;
            else if (sndr.Text == "1/4")
                score = 0.25f;
            else
                score = int.Parse(sndr.Text);
        }
    }
}
