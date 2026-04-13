using Quizify_Project.Classes;
using Quizify_Project.UserControls;
using Quizify_Project.Windows;
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

namespace Quizify_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            isMin = false;

            clsBackShadowSettings.backShadowBorder = BackShadowBorder;
        }

        private async void CloseBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DoubleAnimation fadeOut = new DoubleAnimation(0, TimeSpan.FromMilliseconds(200));

            var scaleX = new DoubleAnimation(1, 0.7, TimeSpan.FromMilliseconds(200));
            var scaleY = new DoubleAnimation(1, 0.7, TimeSpan.FromMilliseconds(200));

            WindowScale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleX);
            WindowScale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleY);

            this.BeginAnimation(Window.OpacityProperty, fadeOut);

            await Task.Delay(TimeSpan.FromMilliseconds(200));

            base.Close();
        }

        double top;

        private async void MaxBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                var scaleX = new DoubleAnimation(1, 0.7, TimeSpan.FromMilliseconds(150));
                var scaleY = new DoubleAnimation(1, 0.7, TimeSpan.FromMilliseconds(150));

                WindowScale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleX);
                WindowScale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleY);

                await Task.Delay(TimeSpan.FromMilliseconds(150));

                WindowScale.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                WindowScale.BeginAnimation(ScaleTransform.ScaleYProperty, null);

                isMin = false;
                this.WindowState = WindowState.Normal;
            }
            else if (this.WindowState == WindowState.Normal)
            {
                var scaleX = new DoubleAnimation(0.7, 1, TimeSpan.FromMilliseconds(300))
                {
                    EasingFunction = new QuarticEase
                    {
                        EasingMode = EasingMode.EaseOut,
                    }
                };

                var scaleY = new DoubleAnimation(0.7, 1, TimeSpan.FromMilliseconds(300))
                {
                    EasingFunction = new QuarticEase
                    {
                        EasingMode = EasingMode.EaseOut,
                    }
                };

                WindowScale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleX);
                WindowScale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleY);

                isMin = false;
                this.WindowState = WindowState.Maximized;
            }
        }

        bool isMin;
        private async void MinBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            top = this.Top;

            var move = new DoubleAnimation(top, top + 300, TimeSpan.FromMilliseconds(300))
            {
                EasingFunction = new QuarticEase
                {
                    EasingMode = EasingMode.EaseOut,
                }
            };

            var fade = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(300))
            {
                EasingFunction = new QuarticEase
                {
                    EasingMode = EasingMode.EaseOut,
                }
            };


            var scaleX = new DoubleAnimation(1, 0.7, TimeSpan.FromMilliseconds(300))
            {
                EasingFunction = new QuarticEase
                {
                    EasingMode = EasingMode.EaseOut,
                }
            };

            var scaleY = new DoubleAnimation(1, 0.7, TimeSpan.FromMilliseconds(300))
            {
                EasingFunction = new QuarticEase
                {
                    EasingMode = EasingMode.EaseOut,
                }
            };

            WindowScale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleX);
            WindowScale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleY);

            this.BeginAnimation(Window.TopProperty, move);
            this.BeginAnimation(Window.OpacityProperty, fade);

            await Task.Delay(300);

            this.BeginAnimation(Window.TopProperty, null);
            this.BeginAnimation(Window.OpacityProperty, null);

            WindowScale.BeginAnimation(ScaleTransform.ScaleXProperty, null);
            WindowScale.BeginAnimation(ScaleTransform.ScaleYProperty, null);

            this.Top = top;
            this.Opacity = 1;
            this.WindowScale.ScaleX = 1;
            this.WindowScale.ScaleY = 1;

            this.WindowState = WindowState.Minimized;

            isMin = true;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private bool isResizing = false;
        private string resizeDirection = "";

        private void RightResize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border != null)
            {
                isResizing = true;
                resizeDirection = border.Name;
                border.CaptureMouse(); 
            }
        }

        private void RightResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isResizing) return;

            Point pos = e.GetPosition(this);

            if (resizeDirection == "RightResize")
                this.Width = Math.Max(200, pos.X); 
            else if (resizeDirection == "BottomResize")
                this.Height = Math.Max(200, pos.Y);
        }

        private void RightResize_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isResizing = false;
            var border = sender as Border;
            border?.ReleaseMouseCapture();
        }

        private async void MyWindow_StateChanged(object sender, EventArgs e)
        {
            if (!isMin) return;

            if (WindowState == WindowState.Normal || WindowState == WindowState.Maximized)
            {
                top = this.Top;

                var move = new DoubleAnimation(top + 300, top, TimeSpan.FromMilliseconds(300))
                {
                    EasingFunction = new QuarticEase
                    {
                        EasingMode = EasingMode.EaseOut,
                    }
                };

                var fade = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300))
                {
                    EasingFunction = new QuarticEase
                    {
                        EasingMode = EasingMode.EaseOut,
                    }
                };


                var scaleX = new DoubleAnimation(0.7, 1, TimeSpan.FromMilliseconds(300))
                {
                    EasingFunction = new QuarticEase
                    {
                        EasingMode = EasingMode.EaseOut,
                    }
                };

                var scaleY = new DoubleAnimation(0.7, 1, TimeSpan.FromMilliseconds(300))
                {
                    EasingFunction = new QuarticEase
                    {
                        EasingMode = EasingMode.EaseOut,
                    }
                };

                WindowScale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleX);
                WindowScale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleY);

                this.BeginAnimation(Window.TopProperty, move);
                this.BeginAnimation(Window.OpacityProperty, fade);

                await Task.Delay(300);

                this.BeginAnimation(Window.TopProperty, null);
                this.BeginAnimation(Window.OpacityProperty, null);

                WindowScale.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                WindowScale.BeginAnimation(ScaleTransform.ScaleYProperty, null);

                isMin = false;
            }
        }
    }
}