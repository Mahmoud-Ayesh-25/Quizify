using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Quizify_Project.Classes
{
    public class clsBackShadowSettings
    {
        public static Border backShadowBorder {  get; set; }

        public static void BackShadowOn()
        {
            backShadowBorder.Visibility = Visibility.Visible;

            DoubleAnimation opacity = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(200));

            backShadowBorder.BeginAnimation(Border.OpacityProperty, opacity);
        }

        public static async void BackShadowOff()
        {
            DoubleAnimation opacity = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(200));

            backShadowBorder.BeginAnimation(Border.OpacityProperty, opacity);

            await Task.Delay(TimeSpan.FromMilliseconds(200));

            backShadowBorder.Visibility = Visibility.Hidden;
        }
    }
}
