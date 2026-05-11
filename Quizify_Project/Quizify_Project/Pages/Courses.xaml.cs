using Microsoft.Data.SqlClient;
using Quizify_DB_BusinessLayer;
using Quizify_DB_DataLayer;
using Quizify_Project.Classes;
using Quizify_Project.UserControls;
using Quizify_Project.Windows;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Quizify_Project.Pages
{
    /// <summary>
    /// Interaction logic for Courses.xaml
    /// </summary>
    public partial class Courses : Page
    {
        DataTable courses = new DataTable();
        DataTable dt = new DataTable();
        clsColors colors = new clsColors();

        enum enSortBy { Default, Name};
        enSortBy _sortBy;

        public double width { get; set; }

        bool firstLoad = true;

        public Courses()
        {
            _sortBy = enSortBy.Default;

            InitializeComponent();
        }

        void PlayAnimation()
        {
            switch(clsPagesSettings.coursesAnimationMode)
            {
                case clsPagesSettings.enAnimationMode.Load:
                    LoadAnimation(); break;
                case clsPagesSettings.enAnimationMode.In:
                    InAnimation(); break;
                case clsPagesSettings.enAnimationMode.Out:
                    OutAnimation(); break;
                default: break;
            }
        }

        async void LoadAnimation()
        {
            wpButtonsContainer.IsEnabled = false;
            this.CacheMode = new BitmapCache();

            SVScaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, null);
            SVScaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, null);
            SViewr.BeginAnimation(OpacityProperty, null);

            DoubleAnimation scaleX = new DoubleAnimation(1.7, 1, TimeSpan.FromMilliseconds(700))
            {
                EasingFunction = new QuarticEase()
                {
                    EasingMode = EasingMode.EaseOut,
                }
            };
            DoubleAnimation scaleY = new DoubleAnimation(1.7, 1, TimeSpan.FromMilliseconds(700))
            {
                EasingFunction = new QuarticEase()
                {
                    EasingMode = EasingMode.EaseOut,
                }
            };

            DoubleAnimation opacity = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(400));

            SVScaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleX);
            SVScaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleY);

            SViewr.BeginAnimation(OpacityProperty, opacity);

            this.CacheMode = null;

            await Task.Delay(TimeSpan.FromMilliseconds(700));
            wpButtonsContainer.IsEnabled = true;
        }

        async void OutAnimation()
        {
            wpButtonsContainer.IsEnabled = false;
            this.CacheMode = new BitmapCache();

            txt_TT.BeginAnimation(TranslateTransform.XProperty, null);
            BackBTN.BeginAnimation(OpacityProperty, null);
            SVScaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, null);
            SVScaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, null);
            SViewr.BeginAnimation(OpacityProperty, null);

            DoubleAnimation translateX = new DoubleAnimation(0, Clmn.ActualWidth + 10, TimeSpan.FromMilliseconds(200))
            {
                EasingFunction = new QuarticEase()
                {
                    EasingMode = EasingMode.EaseOut,
                }
            };

            DoubleAnimation backBTN_Opacity = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(200))
            {
                EasingFunction = new QuarticEase()
                {
                    EasingMode = EasingMode.EaseOut,
                }
            };

            txt_TT.BeginAnimation(TranslateTransform.XProperty, translateX);

            BackBTN.BeginAnimation(OpacityProperty, backBTN_Opacity);

            DoubleAnimation scaleX = new DoubleAnimation(1, 1.7, TimeSpan.FromMilliseconds(200))
            {
                EasingFunction = new QuarticEase()
                {
                    EasingMode = EasingMode.EaseIn,
                }
            };

            DoubleAnimation scaleY = new DoubleAnimation(1, 1.7, TimeSpan.FromMilliseconds(200))
            {
                EasingFunction = new QuarticEase()
                {
                    EasingMode = EasingMode.EaseIn,
                }
            };

            DoubleAnimation opacity = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(200));

            SVScaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleX);
            SVScaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleY);

            SViewr.BeginAnimation(OpacityProperty, opacity);

            this.CacheMode = null;

            await Task.Delay(TimeSpan.FromMilliseconds(200));
            wpButtonsContainer.IsEnabled = true;
        }

        async void InAnimation()
        {
            wpButtonsContainer.IsEnabled = false;
            this.CacheMode = new BitmapCache();

            SVScaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, null);
            SVScaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, null);
            SViewr.BeginAnimation(OpacityProperty, null);

            DoubleAnimation scaleX = new DoubleAnimation(1.7, 1, TimeSpan.FromMilliseconds(200))
            {
                EasingFunction = new QuarticEase()
                {
                    EasingMode = EasingMode.EaseOut,
                }
            };
            DoubleAnimation scaleY = new DoubleAnimation(1.7, 1, TimeSpan.FromMilliseconds(200))
            {
                EasingFunction = new QuarticEase()
                {
                    EasingMode = EasingMode.EaseOut,
                }
            };

            DoubleAnimation opacity = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(100));

            SVScaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleX);
            SVScaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleY);

            SViewr.BeginAnimation(OpacityProperty, opacity);

            this.CacheMode = null;

            await Task.Delay(TimeSpan.FromMilliseconds(200));
            wpButtonsContainer.IsEnabled = true;
        }

        void SetComboBox()
        {
            SortedByCBx.name = "Sort By";

            SortedByCBx.AddItem("Default");
            SortedByCBx.AddItem("Title");

            SortedByCBx.SetSelectedItem(0);
        }

        void AddNewButtonItem(DataRow row)
        {
            clsColors.stColor color = new clsColors.stColor();

            foreach(KeyValuePair<string, clsColors.stColor> kvp in colors.buttonColors)
            {
                if (kvp.Key.ToString() == row[2].ToString()) color = kvp.Value;
            }

            MainButton button = new MainButton((int)row[0], row[1].ToString(), (int)row[4], row[3].ToString(), color);

            if (wpButtonsContainer.Children.Contains(button)) return;

            button.MouseUp += Item_MouseUp;
            button.EditBTN.MouseUp += EditBTN_MouseUp;
            button.RemoveBTN.MouseUp += RemoveBTN_MouseUp;

            wpButtonsContainer.Children.Add(button);
        }

        void LoadButtonsItems(DataTable items)
        {
            wpButtonsContainer.Children.Clear();

            if (items.Rows.Count != 0)
            {
                Dictionary<DataRow, int> buttons = new Dictionary<DataRow, int>();

                if (_sortBy == enSortBy.Name)
                {
                    var sortedItems = items.AsEnumerable().OrderBy(i => i.Field<string>(items.Columns[1].ColumnName));

                    items = sortedItems.CopyToDataTable();
                }

                foreach (DataRow row in items.Rows)
                {
                    AddNewButtonItem(row);
                }
            }

            AddNew addNewButton = new AddNew();
            addNewButton.MouseUp += AddNewBTN_MouseUp;

            wpButtonsContainer.Children.Add(addNewButton);
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SViewr.Opacity = 0;

            SetComboBox();

            try
            {
                courses = await clsCourse.GetAllCoursesWithLessonsCount();

                if (courses.Rows.Count == 0)
                {
                    courses = new DataTable();

                    courses.Columns.Add("CourseID", typeof(int));
                    courses.Columns.Add("Title", typeof(string));
                    courses.Columns.Add("Color", typeof(string));
                    courses.Columns.Add("IconPath", typeof(string));
                }

                dt = courses.Copy();
            }
            catch { CustomMessageBox.ShowWithBackShadow("An error occurred while fetching data from the database.", MessageBoxImage.Error, DBErrorFullMessage.FullMessage); }

            LoadButtonsItems(dt);

            PlayAnimation();

            SViewr.Opacity = 1;
        }

        private async void SortedByCBx_OnSelectedItemChanged(string newSelectedItem)
        {
            if (firstLoad) { firstLoad = false;  return; }

            DoubleAnimation animOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(100));
            DoubleAnimation animIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(100));

            wpButtonsContainer.BeginAnimation(OpacityProperty, animOut);

            await Task.Delay(TimeSpan.FromMilliseconds(100));

            if (newSelectedItem == "Default")
                _sortBy = enSortBy.Default;
            else
                _sortBy = enSortBy.Name;

            LoadButtonsItems(dt);

            wpButtonsContainer.BeginAnimation(OpacityProperty, animIn);
        }

        private void SearchBar_OnTextChanged()
        {
            if (firstLoad) { firstLoad = false; return; }

            dt = courses.Copy();

            DoubleAnimation animOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(50));
            DoubleAnimation animIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(50));

            wpButtonsContainer.BeginAnimation(OpacityProperty, animOut);

            if (SearchBar.Text != string.Empty)
            {
                if (dt.Rows.Count != 0)
                {
                    var items = dt.AsEnumerable().Where(row => (row.Field<string>(dt.Columns[1].ColumnName) ?? "").Contains(SearchBar.Text, StringComparison.OrdinalIgnoreCase));

                    if (items.Any())
                    {
                        dt = items.CopyToDataTable();
                    }
                    else
                    {
                        dt.Clear();
                    }
                }
            }

            LoadButtonsItems(dt);

            wpButtonsContainer.BeginAnimation(OpacityProperty, animIn);
        }

        private async void Item_MouseUp(object sender, MouseButtonEventArgs e)
        {
            wpButtonsContainer.IsEnabled = false;

            clsPagesSettings.selectedCourseID = ((MainButton)sender).ID;

            clsPagesSettings.lessonsAnimationMode = clsPagesSettings.enAnimationMode.InBack;

            clsPagesSettings.selectedCourseTitle = ((MainButton)sender).Title;

            OutAnimation();

            await Task.Delay(TimeSpan.FromMilliseconds(200));

            this.NavigationService.Source = new Uri("/Pages/Lessons.xaml", UriKind.Relative);
        }

        void AddCourseComplete(int id, string title, string color, string iconPath)
        {
            wpButtonsContainer.Children.RemoveAt(wpButtonsContainer.Children.Count - 1);

            clsColors.stColor buttonColor;

            foreach(KeyValuePair<string, clsColors.stColor> clr in colors.buttonColors)
            {
                if (clr.Key == color)
                    buttonColor = clr.Value;
            }

            MainButton button = new MainButton(id, title, 0, iconPath, buttonColor);
            button.MouseUp += Item_MouseUp;
            button.EditBTN.MouseUp += EditBTN_MouseUp;
            button.RemoveBTN.MouseUp += RemoveBTN_MouseUp;
            wpButtonsContainer.Children.Add(button);

            AddNew addNew = new AddNew();
            addNew.MouseUp += AddNewBTN_MouseUp;
            wpButtonsContainer.Children.Add(addNew);

            DataRow newRow = courses.NewRow();

            newRow[0] = id;
            newRow[1] = title;
            newRow[2] = color;
            newRow[3] = iconPath;

            courses.Rows.Add(newRow);

            dt = courses.Copy();
        }

        void EditLessonComplete(int id, string title, string color, string iconPath)
        {
            for (int i = 0; i < wpButtonsContainer.Children.Count - 1; i++)
            {
                if (((MainButton)wpButtonsContainer.Children[i]).ID == id)
                {
                    ((MainButton)wpButtonsContainer.Children[i]).Title = title;
                    ((MainButton)wpButtonsContainer.Children[i]).IconPath = iconPath;

                    clsColors.stColor buttonColor;
                    foreach(KeyValuePair<string, clsColors.stColor> clr in colors.buttonColors)
                    {
                        if (clr.Key == color)
                            buttonColor = clr.Value;
                    }

                    ((MainButton)wpButtonsContainer.Children[i]).ChangeButtonColor(buttonColor);

                    courses.Rows[i][1] = title;
                    courses.Rows[i][2] = color;
                    courses.Rows[i][3] = iconPath;

                    dt = courses.Copy();

                    return;
                }
            }
        }

        private void EditBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int id = int.Parse(((Border)sender).Tag.ToString());

            AddEditCourse addEditCourse = new AddEditCourse(id, colors.buttonColors);
            addEditCourse.OnSaveComplete += EditLessonComplete;

            clsBackShadowSettings.BackShadowOn();
            addEditCourse.ShowDialog();
            clsBackShadowSettings.BackShadowOff();
        }

        private void AddNewBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AddEditCourse addEditCourse = new AddEditCourse(colors.buttonColors);
            addEditCourse.OnSaveComplete += AddCourseComplete;

            clsBackShadowSettings.BackShadowOn();
            addEditCourse.ShowDialog();
            clsBackShadowSettings.BackShadowOff();
        }

        private async void RemoveBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            clsBackShadowSettings.BackShadowOn();
            if (CustomMessageBox.Show("Are you sure you want to remove this course?", MessageBoxImage.Question))
            {
                int id = int.Parse(((Border)sender).Tag.ToString());

                try
                {
                    DataTable lessons = await clsLesson.GetAllLessonsByCourseID(id);

                    foreach (DataRow lesson in lessons.Rows)
                    {
                        await clsQuestion.DeleteAllQuestionByLessonID(int.Parse(lesson[0].ToString()));

                        await clsLesson.DeleteLesson(int.Parse(lesson[0].ToString()));
                    }

                    await clsCourse.DeleteCourse(id);

                    for (int i = 0; i < courses.Rows.Count; i++)
                    {
                        if (int.Parse(courses.Rows[i][0].ToString()) == id)
                        {
                            courses.Rows.RemoveAt(i);
                            dt = courses.Copy();
                            wpButtonsContainer.Children.RemoveAt(i);
                        }
                    }
                }
                catch { CustomMessageBox.Show("An error occurred while removing the course.", MessageBoxImage.Error, DBErrorFullMessage.FullMessage); }
            }
            clsBackShadowSettings.BackShadowOff();
        }

        void CreateQuiz()
        {
            DoubleAnimation opacity = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(500));

            this.BeginAnimation(OpacityProperty, opacity);

            this.NavigationService.Source = new Uri("Pages/pgQuiz.xaml", UriKind.Relative);
        }

        private void CreateQuizButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CreateQuiz createQuiz = new CreateQuiz();

            clsBackShadowSettings.BackShadowOn();
            createQuiz.OnFinished += CreateQuiz;
            createQuiz.ShowDialog();
            clsBackShadowSettings.BackShadowOff();
        }
    }
}
