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
    /// Interaction logic for CustomComboBox.xaml
    /// </summary>
    public partial class CustomComboBox : UserControl
    {
        public delegate void SelectedItemChanged(string newSelectedItem);
        public event SelectedItemChanged OnSelectedItemChanged;

        public string name
        {
            get
            {
                return SelectedText.Text;
            }
            set
            {
                SelectedText.Text = value;
                _Name = value;
            }
        }

        string _Name;

        public void AddItem(string text)
        {
            ComboBoxItem item = new ComboBoxItem();
            item.text = text;
            item.MouseUp += Item_MouseUp;

            Items.Children.Add(item);
        }

        public void SetSelectedItem(int index)
        {
            for (int i = 0; i < Items.Children.Count; i++)
            {
                ((ComboBoxItem)Items.Children[i]).SetNotSelected();
            }

            ComboBoxItem item = ((ComboBoxItem)Items.Children[index]);

            item.SetSelected();
            SelectedText.Text = $"{_Name}  {item.text}";
        }

        public CustomComboBox()
        {
            InitializeComponent();
        }

        private void MainBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MyPopup.IsOpen = true;
        }

        private void Item_MouseUp(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < Items.Children.Count; i++)
            {
                ((ComboBoxItem)Items.Children[i]).SetNotSelected();
            }

            var item = sender as ComboBoxItem;
            item.SetSelected();
            SelectedText.Text = $"{_Name}  {item.text}";

            OnSelectedItemChanged?.Invoke(item.text);

            MyPopup.IsOpen = false;
        }
    }
}
