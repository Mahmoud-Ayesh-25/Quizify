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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Quizify_Project.UserControls
{
    /// <summary>
    /// Interaction logic for SearchBar.xaml
    /// </summary>
    public partial class SearchBar : UserControl
    {
        public delegate void TextChanged();
        public event TextChanged OnTextChanged;

        public string Text
        {
            get
            {
                return txtBx.Text;
            }
            set
            {
                txtBx.Text = value;
            }
        }

        public SearchBar()
        {
            InitializeComponent();
        }

        private void txtBx_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtBx.Text == "")
            {
                DoubleAnimation da = new DoubleAnimation(1, TimeSpan.FromMilliseconds(200));

                ImageAndSearchPanal.BeginAnimation(OpacityProperty, da);
            }
        }

        private void txtBx_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnTextChanged?.Invoke();
        }
    }
}
