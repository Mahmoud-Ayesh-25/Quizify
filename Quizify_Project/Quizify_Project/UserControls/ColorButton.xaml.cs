using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Quizify_Project.UserControls
{
    /// <summary>
    /// Interaction logic for ColorButton.xaml
    /// </summary>
    public partial class ColorButton : UserControl
    {
        public Brush Back_Ground
        {
            get{ return MainBorder.Background; }
            set{ MainBorder.Background = value; }
        }

        public Color Border_Brush
        {
            get { return BorderBrushColor.Color; }
            set { BorderBrushColor.Color = value; _border_color = value; }
        }

        public string Text
        {
            get { return ButtonText.Text; }
            set { ButtonText.Text = value; }
        }
        Color _border_color;

        public string ColorName { get; set; }

        public ColorButton()
        {
            InitializeComponent();

            this.CacheMode = new BitmapCache()
            {
                RenderAtScale = 1.2
            };
        }

        public void SetChecked()
        {
            ColorAnimation borderAnim = new ColorAnimation(Colors.White, TimeSpan.FromMilliseconds(200));

            BorderBrushColor.BeginAnimation(SolidColorBrush.ColorProperty, borderAnim);
        }

        public void SetUnChecked()
        {
            ColorAnimation borderAnim = new ColorAnimation(_border_color, TimeSpan.FromMilliseconds(200));

            BorderBrushColor.BeginAnimation(SolidColorBrush.ColorProperty, borderAnim);
        }
    }
}
