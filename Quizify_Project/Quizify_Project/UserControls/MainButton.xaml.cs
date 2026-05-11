using Quizify_Project.Classes;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Quizify_Project.UserControls
{
    /// <summary>
    /// Interaction logic for MainButton.xaml
    /// </summary>
    public partial class MainButton : UserControl, INotifyPropertyChanged
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


        public Color MainBackgroundColorUp { get { return _mainBackgroundColorUp; } set { _mainBackgroundColorUp = value; OnPropertyChanged(nameof(MainBackgroundColorUp)); } }
        Color _mainBackgroundColorUp;
        public Color MainBackgroundColorDown { get { return _mainBackgroundColorDown; } set { _mainBackgroundColorDown = value; OnPropertyChanged(nameof(MainBackgroundColorDown)); } }
        Color _mainBackgroundColorDown;

        public Color BorderColorUp { get { return _borderColorUp; } set { _borderColorUp = value; OnPropertyChanged(nameof(BorderColorUp)); } }
        Color _borderColorUp;
        public Color BorderColorDown { get { return _borderColorDown; } set { _borderColorDown = value; OnPropertyChanged(nameof(BorderColorDown)); } }
        Color _borderColorDown;

        public Color LessonsCountAreaBackgroundColor { get { return _lessonsCountAreaBackgroundColor; } set { _lessonsCountAreaBackgroundColor = value; OnPropertyChanged(nameof(LessonsCountAreaBackgroundColor)); } }
        Color _lessonsCountAreaBackgroundColor;

        public Color MouseEnterBorderColor { get { return _mouseEnterBorderColor; } set { _mouseEnterBorderColor = value; OnPropertyChanged(nameof(MouseEnterBorderColor)); } }
        Color _mouseEnterBorderColor;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

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
