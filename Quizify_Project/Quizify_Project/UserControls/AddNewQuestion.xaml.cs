using System.Windows.Controls;
using System.Windows.Input;

namespace Quizify_Project.UserControls
{
    /// <summary>
    /// Interaction logic for MainButton.xaml
    /// </summary>
    public partial class AddNewQuestion : UserControl
    {

        public AddNewQuestion()
        {
            InitializeComponent();
        }

        private void MainBorderStopEvent_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void EditBTN_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickAnim1.To = 1.08;
            ClickAnim2.To = 1.08;
        }

        private void EditBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ClickAnim1.To = 1.03;
            ClickAnim2.To = 1.03;
        }

        private void RemoveBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ClickAnim1.To = 1.03;
            ClickAnim2.To = 1.03;
        }
    }
}
