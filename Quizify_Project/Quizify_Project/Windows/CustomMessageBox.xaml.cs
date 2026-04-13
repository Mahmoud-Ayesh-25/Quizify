using Microsoft.Win32;
using Quizify_Project.Classes;
using Quizify_Project.UserControls;
using System.Media;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Quizify_Project.Windows
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        bool isFullMessageOpen = false;

        public bool result = false;

        public bool isFullMessageVisible
        {
            get
            {
                return FullMessageBTN.IsVisible;
            }
            set
            {
                if (value)
                    FullMessageBTN.Visibility = Visibility.Visible;
                else
                    FullMessageBTN.Visibility = Visibility.Hidden;
            }
        }

        public CustomMessageBox(string smallMessage, MessageBoxImage image, string fullMessage = "")
        {
            InitializeComponent();

            this.DataContext = this;
        }

        static string SetEdgeText(MessageBoxImage image)
        {
            switch(image)
            {
                case MessageBoxImage.Information: return "Information"; 
                case MessageBoxImage.Warning: return "Warning"; 
                case MessageBoxImage.Error: return "Error";
                case MessageBoxImage.Question: return "Question";
                default: return "Warning";
            }
        }

        static Uri GetImageSource(MessageBoxImage image)
        {
            switch (image)
            {
                case MessageBoxImage.Information: return new Uri("/Images/MessageBoxIcons/info.png", UriKind.Relative);
                case MessageBoxImage.Warning: return new Uri("/Images/MessageBoxIcons/warning.png", UriKind.Relative);
                case MessageBoxImage.Error: return new Uri("/Images/MessageBoxIcons/error.png", UriKind.Relative);
                case MessageBoxImage.Question: return new Uri("/Images/MessageBoxIcons/question.png", UriKind.Relative);
                default: return new Uri("/Images/MessageBoxIcons/warning.png");
            }
        }

        static void SetButtons(CustomMessageBox cmb, MessageBoxImage image)
        {
            if (image == MessageBoxImage.Question)
            {
                Button yes = new Button();
                yes.Text = "Yes";

                yes.Width = 100;
                yes.Margin = new Thickness(0, 10, 10, 0);
                yes.MouseUp += cmb.YesBTN_MouseUp;

                Button no = new Button();
                no.Text = "No";

                no.Width = 100;
                no.Margin = new Thickness(0, 10, 10, 0);
                no.MouseUp += cmb.CloseBTN_MouseUp;

                cmb.ButtonsSP.Children.Add(no);
                cmb.ButtonsSP.Children.Add(yes);
            }
            else if (image == MessageBoxImage.Error)
            {
                Button close = new Button();
                close.Text = "Close";

                close.Width = 100;
                close.Margin = new Thickness(0, 10, 10, 0);
                close.MouseUp += cmb.CloseBTN_MouseUp;

                cmb.ButtonsSP.Children.Add(close);
            }
            else
            {
                Button ok = new Button();
                ok.Text = "Ok";

                ok.Width = 100;
                ok.Margin = new Thickness(0, 10, 10, 0);
                ok.MouseUp += cmb.CloseBTN_MouseUp;

                cmb.ButtonsSP.Children.Add(ok);
            }
        }
        public static bool Show(string smallMessage, MessageBoxImage image, string fullMessage = "")
        {
            if (image == MessageBoxImage.Error ||  image == MessageBoxImage.Warning)
                SystemSounds.Beep.Play();

            CustomMessageBox cmb = new CustomMessageBox(smallMessage, image, fullMessage);

            if (fullMessage == string.Empty)
                cmb.isFullMessageVisible = false;

            cmb.SmallMessage.Text = smallMessage;
            cmb.FullMessageText.Text = fullMessage;
            cmb.EdgeText.Text = SetEdgeText(image).ToUpper();
            cmb.MidImage.Source = new BitmapImage(GetImageSource(image));
            SetButtons(cmb, image);

            cmb.ShowDialog();

            return cmb.result;
        }

        public static bool ShowWithBackShadow(string smallMessage, MessageBoxImage image, string fullMessage = "")
        {
            if (image == MessageBoxImage.Error || image == MessageBoxImage.Warning)
                SystemSounds.Beep.Play();

            CustomMessageBox cmb = new CustomMessageBox(smallMessage, image, fullMessage);

            if (fullMessage == string.Empty)
                cmb.isFullMessageVisible = false;

            cmb.SmallMessage.Text = smallMessage;
            cmb.FullMessageText.Text = fullMessage;
            cmb.EdgeText.Text = SetEdgeText(image).ToUpper();
            cmb.MidImage.Source = new BitmapImage(GetImageSource(image));
            SetButtons(cmb, image);

            clsBackShadowSettings.BackShadowOn();

            cmb.ShowDialog();

            clsBackShadowSettings.BackShadowOff();

            return cmb.result;
        }

        async Task CloseAnimation()
        {
            DoubleAnimation fadeOut = new DoubleAnimation(0, TimeSpan.FromMilliseconds(200));

            var scaleX = new DoubleAnimation(1, 0.7, TimeSpan.FromMilliseconds(200));
            var scaleY = new DoubleAnimation(1, 0.7, TimeSpan.FromMilliseconds(200));

            WindowScale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleX);
            WindowScale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleY);

            this.BeginAnimation(Window.OpacityProperty, fadeOut);

            await Task.Delay(TimeSpan.FromMilliseconds(200));
        }

        private async void YesBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            await CloseAnimation();

            result = true;

            base.Close();
        }

        private async void CloseBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            await CloseAnimation();

            base.Close();
        }

        private void DragBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        private async void FullMessageBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!isFullMessageOpen)
            {
                CopyBTN.IsEnabled = true;

                DoubleAnimation copyBTNOpacity = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(200));
                CopyBTN.BeginAnimation(OpacityProperty, copyBTNOpacity);

                DoubleAnimation rotateArrow = new DoubleAnimation(0, 180, TimeSpan.FromMilliseconds(200));
                FullMessageBTNArrowRotation.BeginAnimation(RotateTransform.AngleProperty, rotateArrow);

                this.MaxHeight = 500;

                DoubleAnimation sizeChange = new DoubleAnimation(250, 500, TimeSpan.FromMilliseconds(200));

                this.BeginAnimation(HeightProperty, sizeChange);

                isFullMessageOpen = true;
            }
            else
            {
                DoubleAnimation copyBTNOpacity = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(200));
                CopyBTN.BeginAnimation(OpacityProperty, copyBTNOpacity);

                DoubleAnimation rotateArrow = new DoubleAnimation(180, 0, TimeSpan.FromMilliseconds(200));
                FullMessageBTNArrowRotation.BeginAnimation(RotateTransform.AngleProperty, rotateArrow);

                DoubleAnimation sizeChange = new DoubleAnimation(500, 250, TimeSpan.FromMilliseconds(200));

                this.BeginAnimation(HeightProperty, sizeChange);

                await Task.Delay(TimeSpan.FromMilliseconds(200));

                this.MaxHeight = 250;

                isFullMessageOpen = false;

                CopyBTN.IsEnabled = false;

                
            }
        }

        private void CopyBTN_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Clipboard.Clear();

            Clipboard.SetText(FullMessageText.Text);
        }
    }
}
