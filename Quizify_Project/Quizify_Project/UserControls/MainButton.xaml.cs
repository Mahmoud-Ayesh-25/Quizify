using Quizify_Project.Classes;
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
    /// Interaction logic for MainButton.xaml
    /// </summary>
    public partial class MainButton : UserControl
    {
        public int ID { get; }

        public string Title { get; }

        public string LessonsCount { get; }

        public string IconPath { get; }



        public Color MainBackgroundColorUp { get; }
        public Color MainBackgroundColorDown { get; }

        public Color BorderColorUp { get; }
        public Color BorderColorDown { get; }

        public Color LessonsCountAreaBackgroundColor { get; }

        public Color MouseEnterBorderColor { get; }

        public MainButton(int id, string title, int lessonCount, string iconPath, clsColors.stColor colors)
        {
            ID = id;
            Title = title;
            LessonsCount = $"{lessonCount} Lessons";
            IconPath = iconPath;

            MainBackgroundColorUp = colors.MainBackgroundColorUp;
            MainBackgroundColorDown = colors.MainBackgroundColorDown;

            BorderColorUp = colors.BorderColorUp;
            BorderColorDown = colors.BorderColorDown;

            LessonsCountAreaBackgroundColor = colors.LessonsCountAreaBackgroundColor;

            MouseEnterBorderColor = colors.MouseEnterBorderColor;

            this.DataContext = this;

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

            MainBorder.MouseUp += MainBorderStopEvent_MouseDown;
        }

        private void EditBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ClickAnim1.To = 1.03;
            ClickAnim2.To = 1.03;

            MainBorder.MouseUp -= MainBorderStopEvent_MouseDown;
        }

        private void RemoveBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ClickAnim1.To = 1.03;
            ClickAnim2.To = 1.03;

            MainBorder.MouseUp -= MainBorderStopEvent_MouseDown;
        }
    }
}
