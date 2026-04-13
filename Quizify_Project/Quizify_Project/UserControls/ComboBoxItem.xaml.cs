using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
