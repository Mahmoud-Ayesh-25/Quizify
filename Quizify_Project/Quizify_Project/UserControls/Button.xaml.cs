using System.Windows.Controls;
using System.Windows.Media;

namespace Quizify_Project.UserControls
{
    /// <summary>
    /// Interaction logic for BackButton.xaml
    /// </summary>
    public partial class Button : UserControl
    {
        public string Text {  get; set; }

        public SolidColorBrush backGround {  get; set; }

        public SolidColorBrush border {  get; set; }

        public Button(SolidColorBrush backGround = null, SolidColorBrush border = null)
        {
            backGround ??= new SolidColorBrush(System.Windows.Media.Color.FromArgb(5, 255, 255, 255));
            border ??= new SolidColorBrush(System.Windows.Media.Color.FromArgb(60, 255, 255, 255));

            InitializeComponent();

            this.DataContext = this;

            this.backGround = backGround;
            this.border = border;

            this.CacheMode = new BitmapCache()
            {
                RenderAtScale = 1
            };
        }

        private void MyButton_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            MainBorder.Background = backGround;
            brdr.Color = border.Color;
            borderAnimLeave.To = border.Color;
        }
    }
}
