using Quizify_DB_BusinessLayer;
using Quizify_Project.Classes;
using Quizify_Project.UserControls;
using System.IO;
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
    public partial class AddEditCourse : Window
    {
        public delegate void SaveComplete(int id, string title, string color, string iconPath);
        public event SaveComplete OnSaveComplete;

        enum enMode { Add, Edit}
        enMode _Mode;

        clsCourse course;
        int courseID;

        Quizify_Project.UserControls.Button save;

        Dictionary<string, clsColors.stColor> colors;

        string selectedColor;
        string selectedIcon;

        public AddEditCourse(int id, Dictionary<string, clsColors.stColor> colors)
        {
            InitializeComponent();

            this.DataContext = this;

            courseID = id;

            _Mode = enMode.Edit;

            this.colors = colors;
        }

        public AddEditCourse(Dictionary<string, clsColors.stColor> colors)
        {
            InitializeComponent();

            this.DataContext = this;

            _Mode = enMode.Add;

            this.colors = colors;
        }

        void SetButtons()
        {
            Quizify_Project.UserControls.Button close = new Quizify_Project.UserControls.Button(new SolidColorBrush(Color.FromArgb(255, 24, 30, 60)), new SolidColorBrush(Color.FromArgb(255, 63, 68, 98)));
            save = new Quizify_Project.UserControls.Button(new SolidColorBrush(Color.FromArgb(255, 21, 40, 97)), new SolidColorBrush(Color.FromArgb(255, 65, 92, 164)));

            close.Width = 100;
            close.Margin = new Thickness(0, 10, 10, 12);

            save.Width = 100;
            save.Margin = new Thickness(0, 10, 10, 12);

            close.Text = "Close";
            save.Text = "Save";

            close.MouseUp += CloseBTN_MouseUp;
            save.MouseUp += Save_MouseUp;

            ButtonsSP.Children.Add(save);
            ButtonsSP.Children.Add(close);
        }

        private void CheckColorBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < ColorsPanal.Children.Count; i++)
            {
                ((ColorButton)ColorsPanal.Children[i]).SetUnChecked();
            }

            ((ColorButton)sender).SetChecked();
            selectedColor = ((ColorButton)sender).ColorName;

            DataChanged();
        }
        void SetColorButtons()
        {
            foreach(KeyValuePair<string, clsColors.stColor> color in colors)
            {
                ColorButton button = new ColorButton();

                button.Height = 45;
                button.Width = 45;

                button.Back_Ground = new SolidColorBrush(color.Value.MainBackgroundColorUp);
                button.Border_Brush = color.Value.BorderColorUp;

                button.ColorName = color.Key;

                button.ToolTip = button.ColorName;

                button.MouseUp += CheckColorBTN_MouseUp;

                if (ColorsPanal.Children.Count == 0)
                {
                    button.Margin = new Thickness(0, 0, 19, 0);

                    if (_Mode == enMode.Add)
                    {
                        button.SetChecked();
                        selectedColor = button.ColorName;
                    }
                }
                else if (ColorsPanal.Children.Count == 8)
                {
                    button.Margin = new Thickness(19, 0, 0, 0);
                }
                else
                {
                    button.Margin = new Thickness(19, 0, 19, 0);
                }

                ColorsPanal.Children.Add(button);
            }
        }

        private void CheckIconBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < IconsPanal.Children.Count; i++)
            {
                ((IconButton)IconsPanal.Children[i]).SetUnChecked();
            }

            ((IconButton)sender).SetChecked();
            selectedIcon = ((IconButton)sender).ImageSource.ToString();

            DataChanged();
        }
        async Task SetIconButtons()
        {
            string folderPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Images",
                "Icons"
            );

            string[] icons = await Task.Run(() =>
            {
                string[] icns = Directory.GetFiles(folderPath, "*.png");
                icns.Concat(Directory.GetFiles(folderPath, "*.jpg"))
                     .Concat(Directory.GetFiles(folderPath, "*.jpeg"))
                     .Concat(Directory.GetFiles(folderPath, "*.bmp"))
                     .Concat(Directory.GetFiles(folderPath, "*.tiff"))
                     .Concat(Directory.GetFiles(folderPath, "*.ico"))
                     .ToList();
                return icns;
            });

            foreach (string icon in icons)
            {
                IconButton button = new IconButton();

                button.ImageSource = new BitmapImage(new Uri(icon, UriKind.Absolute));
                button.Width = 60;
                button.Height = 60;
                button.Margin = new Thickness(10.1);
                button.MouseUp += CheckIconBTN_MouseUp;

                if (_Mode == enMode.Add)
                {
                    if (IconsPanal.Children.Count == 0)
                    {
                        button.SetChecked();
                        selectedIcon = button.ImageSource.ToString();
                    }
                }

                IconsPanal.Children.Add(button);

                await Task.Delay(10);
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.CacheMode = new BitmapCache();

            MainBorder.IsEnabled = false;

            if (_Mode == enMode.Add)
            {
                EdgeText.Text = "Add Course";
            }
            else
            {
                EdgeText.Text = "Edit Course";
            }

            SetColorButtons();
            await SetIconButtons();

            SetButtons();

            TitleRTLText.MouseUp += TitleSwitch.MainBorder_MouseUp;

            try
            {
                if (_Mode == enMode.Edit)
                {
                    course = await clsCourse.GetCourse(courseID);
                }
                else
                {
                    course = new clsCourse();
                }
            }
            catch 
            {
                if (CustomMessageBox.Show("An error occurred while fetching the question data from the database.", MessageBoxImage.Error, DBErrorFullMessage.FullMessage) == false)
                {
                    await CloseWindow();
                }
            }

            TitleText.Text = course.title;

            if (_Mode == enMode.Edit)
            {
                selectedColor = course.color;
                selectedIcon = course.iconPath;

                foreach (ColorButton btn in ColorsPanal.Children)
                {
                    if (btn.ColorName == selectedColor)
                        { btn.SetChecked(); }
                }

                foreach (IconButton btn in IconsPanal.Children)
                {
                    if (btn.ImageSource.ToString() == selectedIcon)
                        { btn.SetChecked(); }
                }
            }

            save.Opacity = 0.5;
            save.IsEnabled = false;

            MainBorder.IsEnabled = true;

            await Task.Delay(TimeSpan.FromMilliseconds(300));

            this.CacheMode = null;
        }

        async Task CloseWindow()
        {
            this.CacheMode = new BitmapCache();

            await CloseAnimation();

            base.Close();
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

        private async void CloseBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_Mode == enMode.Edit)
            {
                if (TitleText.Text == course.title && selectedColor == course.color && selectedIcon == course.iconPath)
                {
                    await CloseWindow();
                }
                else
                {
                    if (CustomMessageBox.Show("Are you sure you want to close this window? You will lose all the changes you have made.", MessageBoxImage.Question) == true)
                    {
                        await CloseWindow();
                    }
                }
            }
            else
            {
                if (TitleText.Text == string.Empty)
                {
                    await CloseWindow();
                }
                else
                {
                    if (CustomMessageBox.Show("Are you sure you want to close this window? You will lose all the changes you have made.", MessageBoxImage.Question) == true)
                    {
                        await CloseWindow();
                    }
                }
            }
        }

        private void DragBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void TitleSwitch_OnStatusChanged(bool status)
        {
            if (status)
                TitleText.FlowDirection = FlowDirection.RightToLeft;
            else
                TitleText.FlowDirection = FlowDirection.LeftToRight;
        }

        private void DataChanged()
        {
            if (TitleText.Text == string.Empty)
            {
                save.Opacity = 0.5;
                save.IsEnabled = false;
                return;
            }

            if (TitleText.Text == course.title && selectedColor == course.color && selectedIcon == course.iconPath)
            {
                save.Opacity = 0.5;
                save.IsEnabled = false;
            }
            else
            {
                save.Opacity = 1;
                save.IsEnabled = true;
            }
        }

        async Task Save()
        {
            if (_Mode == enMode.Add)
            {
                clsCourse course = new clsCourse();

                course.title = TitleText.Text;
                course.color = selectedColor;
                course.iconPath = selectedIcon;

                try
                {
                    await course.Save();

                    CustomMessageBox.Show("The course has been saved successfully.", MessageBoxImage.Information);

                    this.course = course;
                    courseID = course.courseID;

                    _Mode = enMode.Edit;

                    EdgeText.Text = "Edit Course";

                    OnSaveComplete?.Invoke(courseID, course.title, course.color, course.iconPath);

                    await CloseWindow();
                }
                catch { CustomMessageBox.Show("An error occurred while saving the course.", MessageBoxImage.Error, DBErrorFullMessage.FullMessage); }
            }
            else
            {
                course.title = TitleText.Text;
                course.color = selectedColor;
                course.iconPath = selectedIcon;

                try
                {
                    await course.Save();

                    CustomMessageBox.Show("The course has been saved successfully.", MessageBoxImage.Information);

                    OnSaveComplete?.Invoke(courseID, course.title, course.color, course.iconPath);

                    await CloseWindow();
                }
                catch { CustomMessageBox.Show("An error occurred while saving the course.", MessageBoxImage.Error, DBErrorFullMessage.FullMessage); }
            }
        }
        private async void Save_MouseUp(object sender, MouseButtonEventArgs e)
        {
            await Save();
        }

        private async void TitleText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (save.IsEnabled)
                {
                    await Save();
                }
            }
        }
    }
}
