using System.Windows.Controls;
using System.Windows.Media;

namespace Quizify_Project.UserControls
{
    /// <summary>
    /// Interaction logic for CreateQuizButton.xaml
    /// </summary>
    public partial class CreateQuizButton : UserControl
    {
        public CreateQuizButton()
        {
            InitializeComponent();

            this.CacheMode = new BitmapCache()
            {
                RenderAtScale = 1
            };
        }
    }
}
