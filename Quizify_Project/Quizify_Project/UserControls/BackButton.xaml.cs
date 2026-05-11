using System.Windows.Controls;
using System.Windows.Media;

namespace Quizify_Project.UserControls
{
    /// <summary>
    /// Interaction logic for BackButton.xaml
    /// </summary>
    public partial class BackButton : UserControl
    {
        public BackButton()
        {
            InitializeComponent();

            this.CacheMode = new BitmapCache()
            {
                RenderAtScale = 1
            };
        }
    }
}
