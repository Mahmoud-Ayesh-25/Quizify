using Quizify_Project.Classes;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Quizify_Project.UserControls
{
    /// <summary>
    /// Interaction logic for MainButton.xaml
    /// </summary>
    public partial class MainButton : UserControl
    {
        public int ID { get; }

        public string Title 
        {
            get { return TitleText.Text; }
            set { TitleText.Text = value; } 
        }

        public string LessonsCount { get; }

        public string IconPath
        {
            set { MainImage.Source = new BitmapImage(new Uri(value, UriKind.Absolute)); }
        }



        public Color MainBackgroundColorUp 
        { 
            get { return BackgroundUp.Color; }
            set { BackgroundUp.Color = value; }
        }
        public Color MainBackgroundColorDown
        {
            get { return BackgroundDown.Color; }
            set { BackgroundDown.Color = value; }
        }

        public Color BorderColorUp
        {
            get { return OutLine1.Color; }
            set { OutLine1.Color = value; }
        }
        public Color BorderColorDown
        {
            get { return OutLine2.Color; }
            set { OutLine2.Color = value; }
        }

        public Color LessonsCountAreaBackgroundColor
        {
            get { return ButtomAreaColor.Color; }
            set { ButtomAreaColor.Color = value; }
        }

        public Color MouseEnterBorderColor { get; set; }

        public MainButton(int id, string title, int lessonCount, string iconPath, clsColors.stColor colors)
        {
            ID = id;
            LessonsCount = $"{lessonCount} Lessons";

            InitializeComponent();

            Title = title;
            IconPath = iconPath;

            EditBTN.Tag = id;
            RemoveBTN.Tag = id;

            MainBackgroundColorUp = colors.MainBackgroundColorUp;
            MainBackgroundColorDown = colors.MainBackgroundColorDown;

            BorderColorUp = colors.BorderColorUp;
            BorderColorDown = colors.BorderColorDown;

            LessonsCountAreaBackgroundColor = colors.LessonsCountAreaBackgroundColor;

            MouseEnterBorderColor = colors.MouseEnterBorderColor;

            this.DataContext = this;
        }

        private void MainBorderStopEvent_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void EditBTN_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickAnim1.To = 1.08;
            ClickAnim2.To = 1.08;

            MainBorder.MouseUp += MainBorderStopEvent_MouseDown;
        }

        private void EditBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ClickAnim1.To = 1.03;
            ClickAnim2.To = 1.03;

            MainBorder.MouseUp -= MainBorderStopEvent_MouseDown;
        }

        private void RemoveBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ClickAnim1.To = 1.03;
            ClickAnim2.To = 1.03;

            MainBorder.MouseUp -= MainBorderStopEvent_MouseDown;
        }

        public void ChangeButtonColor(clsColors.stColor colors)
        {
            MainBackgroundColorUp = colors.MainBackgroundColorUp;
            MainBackgroundColorDown = colors.MainBackgroundColorDown;

            BorderColorUp = colors.BorderColorUp;
            BorderColorDown = colors.BorderColorDown;

            LessonsCountAreaBackgroundColor = colors.LessonsCountAreaBackgroundColor;

            MouseEnterBorderColor = colors.MouseEnterBorderColor;
        }
    }
}
