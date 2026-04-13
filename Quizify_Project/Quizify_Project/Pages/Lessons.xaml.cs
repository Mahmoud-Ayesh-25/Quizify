using Microsoft.Data.SqlClient;
using Quizify_DB_BusinessLayer;
using Quizify_DB_DataLayer;
using Quizify_Project.Classes;
using Quizify_Project.UserControls;
using Quizify_Project.Windows;
using Quizify_Project.Windows.Questions;
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
    public partial class Lessons : Page
    {
        DataTable lessons = new DataTable();
        DataTable dt = new DataTable();

        enum enSortBy { Default, Name};
        enSortBy _sortBy;

        public Lessons()
        {
            _sortBy = enSortBy.Default;

            InitializeComponent();
        }

        void PlayAnimation()
        {
            switch(clsPagesSettings.lessonsAnimationMode)
            {
                case clsPagesSettings.enAnimationMode.OutBack:
                    OutBackAnimation(); break;
                case clsPagesSettings.enAnimationMode.InBack:
                    InBackAnimation(); break;
                case clsPagesSettings.enAnimationMode.In:
                    InAnimation(); break;
                case clsPagesSettings.enAnimationMode.Out:
                    OutAnimation(); break;
                default: break;
            }
        }

        void InBackAnimation()
        {
            DoubleAnimation scaleX = new DoubleAnimation(0.7, 1, TimeSpan.FromMilliseconds(200))
            {
                EasingFunction = new QuarticEase()
                {
                    EasingMode = EasingMode.EaseOut,
                }
            };
            DoubleAnimation scaleY = new DoubleAnimation(0.7, 1, TimeSpan.FromMilliseconds(200))
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

        void OutBackAnimation()
        {
            DoubleAnimation translateX = new DoubleAnimation(0, ((Clmn.ActualWidth) * -1) - 10, TimeSpan.FromMilliseconds(200))
            {
                EasingFunction = new QuarticEase()
                {
                    EasingMode = EasingMode.EaseOut,
                }
            };

            DoubleAnimation backBTN_Opacity = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(200))
            {
                EasingFunction = new QuarticEase()
                {
                    EasingMode = EasingMode.EaseOut,
                }
            };

            txt_TT.BeginAnimation(TranslateTransform.XProperty, translateX);

            BackBTN.BeginAnimation(OpacityProperty, backBTN_Opacity);

            DoubleAnimation scaleX = new DoubleAnimation(1, 0.7, TimeSpan.FromMilliseconds(200))
            {
                EasingFunction = new QuarticEase()
                {
                    EasingMode = EasingMode.EaseOut,
                }
            };
            DoubleAnimation scaleY = new DoubleAnimation(1, 0.7, TimeSpan.FromMilliseconds(200))
            {
                EasingFunction = new QuarticEase()
                {
                    EasingMode = EasingMode.EaseOut,
                }
            };

            DoubleAnimation opacity = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(100));

            SVScaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleX);
            SVScaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleY);

            SViewr.BeginAnimation(OpacityProperty, opacity);
        }

        async void OutAnimation()
        {
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

            DoubleAnimation txtOpacity = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(50));

            await Task.Delay(TimeSpan.FromMilliseconds(50));

            text.BeginAnimation(OpacityProperty, txtOpacity);
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

        void AddNewButtonItem(DataRow row, int questionCount)
        {

            LessonButton button = new LessonButton((int)row[0], row[1].ToString(), questionCount);

            button.MouseUp += Item_MouseUp;
            button.EditBTN.MouseUp += EditBTN_MouseUp;
            button.RemoveBTN.MouseUp += Remove_MouseUp;

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
                            int questionCount = await clsQuestion.GetQuestionsCountByLessonID((int)row[0], connection);

                            AddNewButtonItem(row, questionCount);
                        }
                    }
                    catch { CustomMessageBox.ShowWithBackShadow("An error occurred while fetching data from the database.", MessageBoxImage.Error, DBErrorFullMessage.FullMessage); }
                    ;
                }
            }

            AddNew addNewButton = new AddNew();

            addNewButton.MouseUp += AddNewBTN_MouseUp;

            wpButtonsContainer.Children.Add(addNewButton);
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            text.Text = clsPagesSettings.selectedCourseTitle;

            SetComboBox();

            try
            {
                lessons = await clsLesson.GetAllLessonsByCourseID(clsPagesSettings.selectedCourseID);

                if (lessons.Rows.Count == 0)
                {
                    lessons = new DataTable();

                    lessons.Columns.Add("LessonID", typeof(int));
                    lessons.Columns.Add("Title", typeof(string));
                    lessons.Columns.Add("CourseID", typeof(int));
                }

                dt = lessons.Copy();
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
            dt = lessons.Copy();

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
            BackBTN.IsEnabled = false;

            clsPagesSettings.selectedLessonTitle = ((LessonButton)sender).Title;
            clsPagesSettings.selectedlessonID = ((LessonButton)sender).ID;

            OutAnimation();

            await Task.Delay(TimeSpan.FromMilliseconds(200));

            this.NavigationService.Source = new Uri("/Pages/Question.xaml", UriKind.Relative);
        }

        private async void BackBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            wpButtonsContainer.IsEnabled = false;
            BackBTN.IsEnabled = false;

            OutBackAnimation();

            await Task.Delay(TimeSpan.FromMilliseconds(200));

            clsPagesSettings.coursesAnimationMode = clsPagesSettings.enAnimationMode.In;

            this.NavigationService.Source = new Uri($"/Pages/Courses.xaml", UriKind.Relative);
        }

        void AddLessonComplete(int id, string title)
        {
            wpButtonsContainer.Children.RemoveAt(wpButtonsContainer.Children.Count - 1);

            LessonButton button = new LessonButton(id, title, 0);
            button.MouseUp += Item_MouseUp;
            button.EditBTN.MouseUp += EditBTN_MouseUp;
            button.RemoveBTN.MouseUp += Remove_MouseUp;
            wpButtonsContainer.Children.Add(button);

            AddNew addNew = new AddNew();
            addNew.MouseUp += AddNewBTN_MouseUp;
            wpButtonsContainer.Children.Add(addNew);

            DataRow newRow = lessons.NewRow();

            newRow[0] = id;
            newRow[1] = title;
            newRow[2] = -1;

            lessons.Rows.Add(newRow);

            dt = lessons.Copy();
        }

        void EditLessonComplete(int id, string title)
        {
            for (int i = 0; i < wpButtonsContainer.Children.Count - 1; i++)
            {
                if (((LessonButton)wpButtonsContainer.Children[i]).ID == id)
                {
                    ((LessonButton)wpButtonsContainer.Children[i]).Title = title;

                    lessons.Rows[i][1] = title;

                    dt = lessons.Copy();

                    return;
                }
            }
        }

        private void AddNewBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AddEditLesson addEditLesson = new AddEditLesson();

            addEditLesson.OnSaveComplete += AddLessonComplete;

            clsBackShadowSettings.BackShadowOn();
            addEditLesson.ShowDialog();
            clsBackShadowSettings.BackShadowOff();
        }

        private void EditBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int lessonId = int.Parse(((Border)sender).Tag.ToString());

            AddEditLesson addEditLesson = new AddEditLesson(lessonId);

            addEditLesson.OnSaveComplete += EditLessonComplete;

            clsBackShadowSettings.BackShadowOn();
            addEditLesson.ShowDialog();
            clsBackShadowSettings.BackShadowOff();
        }

        private async void Remove_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int lessonID = int.Parse(((Border)sender).Tag.ToString());

            clsBackShadowSettings.BackShadowOn();
            if (CustomMessageBox.Show("Are you sure you want to remove this question?", MessageBoxImage.Question) == true)
            {
                try
                {
                    await clsQuestion.DeleteAllQuestionByLessonID(lessonID);
                    await clsLesson.DeleteLesson(lessonID);

                    RemoveItemComplete(lessonID);
                }
                catch { CustomMessageBox.Show("An error occurred while removing the question.", MessageBoxImage.Error, DBErrorFullMessage.FullMessage); }
            }
            clsBackShadowSettings.BackShadowOff();
        }

        void RemoveItemComplete(int id)
        {
            for (int i = 0; i < wpButtonsContainer.Children.Count - 1; i++)
            {
                if (((LessonButton)wpButtonsContainer.Children[i]).ID == id)
                {
                    wpButtonsContainer.Children.RemoveAt(i);

                    lessons.Rows.RemoveAt(i);

                    dt = lessons.Copy();

                    return;
                }
            }
        }
    }
}
