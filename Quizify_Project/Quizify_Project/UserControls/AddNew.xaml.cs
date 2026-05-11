using System.Windows.Controls;
using System.Windows.Media;

namespace Quizify_Project.UserControls
{
    /// <summary>
    /// Interaction logic for MainButton.xaml
    /// </summary>
    public partial class AddNew : UserControl
    {
        public AddNew()
        {
            InitializeComponent();

            this.CacheMode = new BitmapCache()
            {
                RenderAtScale = 1.4
            };
        }
    }
}
