using Quizify_DB_BusinessLayer;
using Quizify_Project.Classes;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Quizify_Project.Windows.Questions
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class AddEditLesson : Window
    {
        public delegate void SaveComplete(int id, string title);
        public event SaveComplete OnSaveComplete;

        enum enMode { Add, Edit}
        enMode _Mode;

        clsLesson lesson;
        int lessonID;

        double tbxRowHeight;

        Quizify_Project.UserControls.Button save;

        public AddEditLesson(int id)
        {
            InitializeComponent();

            this.DataContext = this;

            lessonID = id;

            _Mode = enMode.Edit;
        }

        public AddEditLesson()
        {
            InitializeComponent();

            this.DataContext = this;

            _Mode = enMode.Add;
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


        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_Mode == enMode.Add)
            {
                EdgeText.Text = "Add Lesson";
            }
            else
            {
                EdgeText.Text = "Edit Lesson";
            }

            SetButtons();

            save.Opacity = 0.5;
            save.IsEnabled = false;

            TitleRTLText.MouseUp += TitleSwitch.MainBorder_MouseUp;

            try
            {
                if (_Mode == enMode.Edit)
                {
                    lesson = await clsLesson.GetLesson(lessonID);
                }
                else
                {
                    lesson = new clsLesson();
                }
            }
            catch 
            {
                if (CustomMessageBox.Show("An error occurred while fetching the question data from the database.", MessageBoxImage.Error, DBErrorFullMessage.FullMessage) == false)
                {
                    await CloseWindow();
                }
            }

            TitleText.Text = lesson.title;
        }

        async Task CloseWindow()
        {
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
            if (TitleText.Text == string.Empty)
            {
                await CloseWindow();
            }
            else
            {
                if (_Mode == enMode.Edit)
                {
                    if (TitleText.Text == lesson.title)
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

        private void TitleText_TextChanged()
        {
            if (TitleText.Text == string.Empty)
            {
                save.Opacity = 0.5;
                save.IsEnabled = false;
            }
            else
            {
                if (_Mode == enMode.Edit)
                {
                    if (TitleText.Text == lesson.title)
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
                else
                {
                    save.Opacity = 1;
                    save.IsEnabled = true;
                }
            }
        }

        private async void Save_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_Mode == enMode.Add)
            {
                clsLesson lesson = new clsLesson();

                lesson.title = TitleText.Text;
                lesson.courseID = clsPagesSettings.selectedCourseID;

                try
                {
                    await lesson.Save();

                    CustomMessageBox.Show("The lesson has been saved successfully.", MessageBoxImage.Information);

                    this.lesson = lesson;
                    lessonID = lesson.lessonID;

                    _Mode = enMode.Edit;

                    EdgeText.Text = "Edit Lesson";

                    OnSaveComplete?.Invoke(lessonID, lesson.title);

                    await CloseWindow();
                }
                catch { CustomMessageBox.Show("An error occurred while saving the lesson.", MessageBoxImage.Error, DBErrorFullMessage.FullMessage); }
            }
            else
            {
                lesson.title = TitleText.Text;

                try
                {
                    await lesson.Save();

                    CustomMessageBox.Show("The lesson has been saved successfully.", MessageBoxImage.Information);

                    OnSaveComplete?.Invoke(lessonID, lesson.title);

                    await CloseWindow();
                }
                catch { CustomMessageBox.Show("An error occurred while saving the lesson.", MessageBoxImage.Error, DBErrorFullMessage.FullMessage); }
            }
        }
    }
}
