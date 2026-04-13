using Microsoft.Data.SqlClient;
using Quizify_DB_BusinessLayer;
using Quizify_DB_DataLayer;
using Quizify_Project.Classes;
using Quizify_Project.UserControls;
using Quizify_Project.Windows;
using System.Data;
using System.Threading.Tasks;
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

        void LoadAnimation()
        {
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
        }

        void OutAnimation()
        {
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
        }

        void InAnimation()
        {
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
        }

        void SetComboBox()
        {
            SortedByCBx.name = "Sort By";

            SortedByCBx.AddItem("Default");
            SortedByCBx.AddItem("Title");

            SortedByCBx.SetSelectedItem(0);
        }

        void AddNewButtonItem(DataRow row, int lessonsCount)
        {
            clsColors.stColor color = new clsColors.stColor();

            foreach(KeyValuePair<string, clsColors.stColor> kvp in colors.buttonColors)
            {
                if (kvp.Key.ToString() == row[2].ToString()) color = kvp.Value;
            }

            MainButton button = new MainButton((int)row[0], row[1].ToString(), lessonsCount, row[3].ToString(), color);

            button.MouseUp += Item_MouseUp;

            wpButtonsContainer.Children.Add(button);
        }
        async void LoadButtonsItems(DataTable items)
        {
            wpButtonsContainer.Children.Clear();

            if (items.Rows.Count != 0)
            {
                if (_sortBy == enSortBy.Name)
                {
                    var sortedItems = items.AsEnumerable().OrderBy(i => i.Field<string>(items.Columns[1].ColumnName));

                    items = sortedItems.CopyToDataTable();
                }

                using (SqlConnection connection = new SqlConnection(clsSettings.ConnectionString))
                {
                    try
                    {
                        await connection.OpenAsync();

                        foreach (DataRow row in items.Rows)
                        {
                            int lessonsCount = await clsLesson.GetLessonsCountByCourseID((int)row[0], connection);

                            AddNewButtonItem(row, lessonsCount);
                        }
                    }
                    catch { CustomMessageBox.ShowWithBackShadow("An error occurred while fetching data from the database.", MessageBoxImage.Error, DBErrorFullMessage.FullMessage); }
                }
            }

            AddNew addNewButton = new AddNew();

            wpButtonsContainer.Children.Add(addNewButton);
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetComboBox();

            try
            {
                courses = await clsCourse.GetAllCourses();

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

            PlayAnimation();

            LoadButtonsItems(dt);
        }

        private async void SortedByCBx_OnSelectedItemChanged(string newSelectedItem)
        {
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

        private async void SearchBar_OnTextChanged()
        {
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

            await Task.Delay(TimeSpan.FromMilliseconds(50));

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
    }
}
