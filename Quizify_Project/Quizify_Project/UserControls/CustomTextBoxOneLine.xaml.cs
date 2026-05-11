using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Quizify_Project.UserControls
{
    /// <summary>
    /// Interaction logic for SearchBar.xaml
    /// </summary>
    public partial class CustomTextBoxOneLine : UserControl
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

        public int MaxLength
        {
            get
            {
                return txtBx.MaxLength;
            }
            set
            {
                txtBx.MaxLength = value;
            }
        }

        public int LineCount
        {
            get
            {
                return txtBx.LineCount;
            }
        }

        public string BackText
        {
            set
            {
                TextBXBackText.Text = value;
            }
        }

        public TextWrapping TextWrapping
        {
            set
            {
                txtBx.TextWrapping = value;
            }
        }

        public CustomTextBoxOneLine()
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
            if (txtBx.Text != string.Empty)
            {
                ImageAndSearchPanal.Opacity = 0;
            }

            OnTextChanged?.Invoke();
        }
    }
}
