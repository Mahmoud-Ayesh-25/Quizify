using Quizify_DB_BusinessLayer;
using Quizify_Project.Classes;
using Quizify_Project.UserControls;
using Quizify_Project.Windows.Questions;
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
    public partial class Question : Page
    {
        DataTable questions = new DataTable();
        DataTable dt = new DataTable();

        enum enSortBy { Default, Name};
        enSortBy _sortBy;

        bool firstLoad = true;

        public Question()
        {
            _sortBy = enSortBy.Default;

            InitializeComponent();

            clsQuestionPageEventsHelper.OnEditComplete += EditQuestionComplete;
            clsQuestionPageEventsHelper.OnRemoveComplete += RemoveItemComplete;
        }

        async void InBackAnimation()
        {
            spButtonsContainer.IsEnabled = false;
            this.CacheMode = new BitmapCache();

            SVScaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, null);
            SVScaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, null);
            SViewr.BeginAnimation(OpacityProperty, null);

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

            this.CacheMode = null;

            await Task.Delay(TimeSpan.FromMilliseconds(200));
            spButtonsContainer.IsEnabled = true;
        }

        async void OutBackAnimation()
        {
            spButtonsContainer.IsEnabled = false;
            this.CacheMode = new BitmapCache();

            SVScaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, null);
            SVScaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, null);
            SViewr.BeginAnimation(OpacityProperty, null);

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

            this.CacheMode = null;

            await Task.Delay(TimeSpan.FromMilliseconds(200));
            spButtonsContainer.IsEnabled = true;
        }

        void SetComboBox()
        {
            SortedByCBx.name = "Sort By";

            SortedByCBx.AddItem("Default");
            SortedByCBx.AddItem("Question Head");

            SortedByCBx.SetSelectedItem(0);
        }

        void AddNewButtonItem(DataRow row)
        {
            QuestionButton button = new QuestionButton((int)row[0], row[1].ToString(), row[2].ToString());

            button.MouseUp += Item_MouseUp;
            button.EditBTN.MouseUp += Edit_MouseUp;
            button.RemoveBTN.MouseUp += Remove_MouseUp;

            spButtonsContainer.Children.Add(button);
        }
        void LoadButtonsItems(DataTable items)
        {
            spButtonsContainer.Children.Clear();

            if (items.Rows.Count != 0)
            {
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

            AddNewQuestion addNewButton = new AddNewQuestion();

            addNewButton.MouseUp += AddNew_MouseUp;

            spButtonsContainer.Children.Add(addNewButton);
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SViewr.Opacity = 0;

            text.Text = $"{clsPagesSettings.selectedCourseTitle}/{clsPagesSettings.selectedLessonTitle}"
            ;

            SetComboBox();

            try
            {
                questions = await clsQuestion.GetAllQuestionsByLessonID(clsPagesSettings.selectedlessonID);

                if (questions.Rows.Count == 0)
                {
                    questions = new DataTable();

                    questions.Columns.Add("QuestionID", typeof(int));
                    questions.Columns.Add("Head", typeof(string));
                    questions.Columns.Add("Answer", typeof(string));
                    questions.Columns.Add("LessonID", typeof(int));
                }

                dt = questions.Copy();
            }
            catch { CustomMessageBox.ShowWithBackShadow("An error occurred while fetching data from the database.", MessageBoxImage.Error, DBErrorFullMessage.FullMessage); }

            LoadButtonsItems(dt);
            SViewr.Opacity = 1;

            InBackAnimation();
        }

        private async void SortedByCBx_OnSelectedItemChanged(string newSelectedItem)
        {
            if (firstLoad) { firstLoad = false; return; }

            DoubleAnimation animOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(100));
            DoubleAnimation animIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(100));

            spButtonsContainer.BeginAnimation(OpacityProperty, animOut);

            await Task.Delay(TimeSpan.FromMilliseconds(100));

            if (newSelectedItem == "Default")
                _sortBy = enSortBy.Default;
            else
                _sortBy = enSortBy.Name;

            LoadButtonsItems(dt);

            spButtonsContainer.BeginAnimation(OpacityProperty, animIn);
        }

        private void SearchBar_OnTextChanged()
        {
            if (firstLoad) { firstLoad = false; return; }

            dt = questions.Copy();

            DoubleAnimation animOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(50));
            DoubleAnimation animIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(50));

            spButtonsContainer.BeginAnimation(OpacityProperty, animOut);

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

            spButtonsContainer.BeginAnimation(OpacityProperty, animIn);
        }

        private void Item_MouseUp(object sender, MouseButtonEventArgs e)
        {
            QuestionButton sndr = ((QuestionButton)sender);

            ShowQuestion showQuestionWindow = new ShowQuestion(sndr.ID);


            clsBackShadowSettings.BackShadowOn();
            showQuestionWindow.ShowDialog();
            clsBackShadowSettings.BackShadowOff();
        }

        private async void BackBTN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            spButtonsContainer.IsEnabled = false;
            BackBTN.IsEnabled = false;

            OutBackAnimation();

            await Task.Delay(TimeSpan.FromMilliseconds(200));

            clsPagesSettings.lessonsAnimationMode = clsPagesSettings.enAnimationMode.In;

            clsPagesSettings.selectedLessonTitle = string.Empty;
            clsPagesSettings.selectedlessonID = -1;

            this.NavigationService.Source = new Uri($"/Pages/Lessons.xaml", UriKind.Relative);
        }


        void AddQuestionComplete(int id, string head, string answer)
        {
            spButtonsContainer.Children.RemoveAt(spButtonsContainer.Children.Count - 1);

            QuestionButton button = new QuestionButton(id, head, answer);
            button.MouseUp += Item_MouseUp;
            button.EditBTN.MouseUp += Edit_MouseUp;
            button.RemoveBTN.MouseUp += Remove_MouseUp;
            spButtonsContainer.Children.Add(button);

            AddNewQuestion addNew = new AddNewQuestion();
            addNew.MouseUp += AddNew_MouseUp;
            spButtonsContainer.Children.Add(addNew);

            DataRow newRow = questions.NewRow();

            newRow[0] = id;
            newRow[1] = head;
            newRow[2] = answer;
            newRow[3] = -1;

            questions.Rows.Add(newRow);

            dt = questions.Copy();
        }

        void EditQuestionComplete(int id, string head, string answer)
        {
            for (int i = 0;  i < spButtonsContainer.Children.Count - 1; i++)
            {
                if (((QuestionButton)spButtonsContainer.Children[i]).ID == id)
                {
                    ((QuestionButton)spButtonsContainer.Children[i]).Question = head;
                    ((QuestionButton)spButtonsContainer.Children[i]).Answer = answer;

                    questions.Rows[i][1] = head;
                    questions.Rows[i][2] = answer;

                    dt = questions.Copy();

                    return;
                }
            }
        }
        private void AddNew_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AddEditQuestion addEdit = new AddEditQuestion();
            addEdit.OnSaveComplete += AddQuestionComplete;

            clsBackShadowSettings.BackShadowOn();
            addEdit.ShowDialog();
            clsBackShadowSettings.BackShadowOff();
        }

        private void Edit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int quesitonID = int.Parse(((Border)sender).Tag.ToString());

            AddEditQuestion addEdit = new AddEditQuestion(quesitonID);
            addEdit.OnSaveComplete += EditQuestionComplete;

            clsBackShadowSettings.BackShadowOn();
            addEdit.ShowDialog();
            clsBackShadowSettings.BackShadowOff();
        }

        private async void Remove_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int quesitonID = int.Parse(((Border)sender).Tag.ToString());

            clsBackShadowSettings.BackShadowOn();
            if (CustomMessageBox.Show("Are you sure you want to remove this question?", MessageBoxImage.Question) == true)
            {
                try
                {
                    await clsQuestion.DeleteQuestion(quesitonID);

                    RemoveItemComplete(quesitonID);
                }
                catch { CustomMessageBox.Show("An error occurred while removing the question.", MessageBoxImage.Error, DBErrorFullMessage.FullMessage); }
            }
            clsBackShadowSettings.BackShadowOff();
        }

        void RemoveItemComplete(int id)
        {
            for (int i = 0; i < spButtonsContainer.Children.Count - 1; i++)
            {
                if (((QuestionButton)spButtonsContainer.Children[i]).ID == id)
                {
                    spButtonsContainer.Children.RemoveAt(i);

                    questions.Rows.RemoveAt(i);

                    dt = questions.Copy();

                    return;
                }
            }
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
