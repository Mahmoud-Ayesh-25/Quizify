using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Quizify_Project.UserControls
{
    /// <summary>
    /// Interaction logic for IconButton.xaml
    /// </summary>
    public partial class IconButton : UserControl
    {
        public ImageSource ImageSource
        {
            get { return MainImage.Source; }
            set { MainImage.Source = value; }
        }

        Color _border_color;

        public IconButton()
        {
            InitializeComponent();

            _border_color = BorderColor.Color;
        }

        public void SetChecked()
        {
            ColorAnimation borderAnim = new ColorAnimation(Colors.White, TimeSpan.FromMilliseconds(200));

            BorderColor.BeginAnimation(SolidColorBrush.ColorProperty, borderAnim);
        }

        public void SetUnChecked()
        {
            ColorAnimation borderAnim = new ColorAnimation(_border_color, TimeSpan.FromMilliseconds(200));

            BorderColor.BeginAnimation(SolidColorBrush.ColorProperty, borderAnim);
        }
    }
}
