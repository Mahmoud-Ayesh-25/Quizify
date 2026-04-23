using System.Windows.Controls;
using System.Windows.Media;

namespace Quizify_Project.UserControls
{
    /// <summary>
    /// Interaction logic for ComboBoxItem.xaml
    /// </summary>
    public partial class ComboBoxItem : UserControl
    {
        public bool isSelected;

        public string text
        {
            get
            {
                return txt.Text;
            }
            set
            {
                txt.Text = value;
            }
        }

        public void SetNotSelected()
        {
            isSelected = false;

            BorderColor.Color = Colors.Transparent;
        }

        public void SetSelected()
        {
            isSelected = true;

            BorderColor.Color = Colors.White;
        }

        public ComboBoxItem()
        {
            InitializeComponent();
        }
    }
}
