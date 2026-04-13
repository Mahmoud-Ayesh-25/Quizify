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
    /// Interaction logic for Switch.xaml
    /// </summary>
    public partial class Switch : UserControl
    {
        public delegate void SwitchStatusChanged(bool status);
        public event SwitchStatusChanged OnStatusChanged;

        public bool isSwitchOn {  get; set; }

        public Switch()
        {
            InitializeComponent();
        }
        private void SwitchBTN_Loaded(object sender, RoutedEventArgs e)
        {
            isSwitchOn = false;

            SwitchTransform.X = -(SwitchBall.ActualWidth / 2);
        }

        void SwitchOn()
        {
            Storyboard sb = (Storyboard)MainBorder.FindResource("SwitchOnAnimation");
            sb.Begin();

            DoubleAnimation moveRight = new DoubleAnimation(-(SwitchBall.ActualWidth / 2), (SwitchBall.ActualWidth / 2), TimeSpan.FromMilliseconds(150));

            SwitchTransform.BeginAnimation(TranslateTransform.XProperty, moveRight);

            isSwitchOn = true;

            OnStatusChanged?.Invoke(true);
        }

        void SwitchOff()
        {
            Storyboard sb = (Storyboard)MainBorder.FindResource("SwitchOffAnimation");
            sb.Begin();

            DoubleAnimation moveLeft = new DoubleAnimation((SwitchBall.ActualWidth / 2), -(SwitchBall.ActualWidth / 2), TimeSpan.FromMilliseconds(150));

            SwitchTransform.BeginAnimation(TranslateTransform.XProperty, moveLeft);

            isSwitchOn = false;

            OnStatusChanged?.Invoke(false);
        }

        public void MainBorder_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!isSwitchOn)
                SwitchOn();
            else
                SwitchOff();
        }

    }
}
