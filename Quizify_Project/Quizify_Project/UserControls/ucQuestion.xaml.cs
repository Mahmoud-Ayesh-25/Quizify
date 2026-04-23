using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Quizify_Project.UserControls
{
    /// <summary>
    /// Interaction logic for ucQuestion.xaml
    /// </summary>
    public partial class ucQuestion : UserControl
    {
        double questionRowHeight;
        double answerRowHeight;

        string questionHead;

        public string Answer
        {
            get { return AnswerTextBX.Text; }
        }

        public ucQuestion(string questionHead, Color underLineColor)
        {
            InitializeComponent();

            if (!questionHead.EndsWith("?"))
            {
                questionHead += "?";
            }

            this.questionHead = questionHead;

            UnderLineColor1.Color = underLineColor;
            UnderLineColor2.Color = underLineColor;
            UnderLineColor3.Color = underLineColor;

            QuestionRTLText.MouseUp += QuestionRTLSwitch.MainBorder_MouseUp;
            AnswerRTLText.MouseUp += AnswerRTLSwitch.MainBorder_MouseUp;
        }

        private void QuestionUC_Loaded(object sender, RoutedEventArgs e)
        {
            QuestionText.Text = questionHead;

            SetHeight();
        }

        double TotalHeight()
        {
            return 120 + questionRowHeight + answerRowHeight;
        }

        void SetHeight()
        {
            QuestionText.Height = QuestionText.LineCount * 28;
            AnswerTextBX.Height = (AnswerTextBX.LineCount * 20) + 25;

            questionRowHeight = QuestionText.Height + 15;
            answerRowHeight = AnswerTextBX.Height + 25;

            QuestionRow.Height = new GridLength(questionRowHeight);
            AnswerRow.Height = new GridLength(answerRowHeight);

            this.Height = TotalHeight();
            this.MinHeight = TotalHeight();
            this.MaxHeight = TotalHeight();
        }

        private void CustomTextBox_OnTextChanged()
        {
            SetHeight();
        }

        private void QuestionRTLSwitch_OnStatusChanged(bool status)
        {
            if (status)
                QuestionText.FlowDirection = FlowDirection.RightToLeft;
            else
                QuestionText.FlowDirection = FlowDirection.LeftToRight;
        }

        private void AnswerRTLSwitch_OnStatusChanged(bool status)
        {
            if (status)
                AnswerTextBX.FlowDirection = FlowDirection.RightToLeft;
            else
                AnswerTextBX.FlowDirection = FlowDirection.LeftToRight;
        }
    }
}
