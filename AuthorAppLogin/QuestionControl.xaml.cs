using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AuthorApp.Infrastructure;
using AuthorApp.Models;
using AuthorApp.Services;

namespace AuthorApp
{
    /// <summary>
    /// Interaction logic for QuestionControl.xaml
    /// </summary>
    public partial class QuestionControl : UserControl
    {
        public bool IsUpdating = false;
        public int Id = 0;
        public AssessmentQuestion currentQuestion;
        public int assessmentId;

        public QuestionControl()
        {
            InitializeComponent();
        }


        public void LoadContent(int assessmentId)
        {
            using (var service = new AssessmentService())
            {
                var item = service.GetAssessmentAndAttributes(assessmentId);

                if (item != null)
                {
                    lblHeading.Text = "QUESTIONS - " + item.Name;

                    listOptionsType.SelectedIndex = 0;
                    listOptionsLayout.SelectedIndex = 0;


                    var cb = new ComboObj {Text = "None", Id = 0};

                    var topicList =
                        item.AssessmentTopics.Select(x => new ComboObj {Text = x.Topic, Id = x.TopicId}).ToList();
                    topicList.Add(cb);

                    listTopics.ItemsSource = topicList;

                    listTopics.SelectedValue = cb.Id;


                    var levelList =
                        item.QuestionDifficultyLevels.Select(x => new ComboObj {Text = x.LevelName, Id = x.LevelId}).
                            ToList();

                    levelList.Add(cb);

                    listLevels.ItemsSource = levelList;

                    listLevels.SelectedValue = cb.Id;


                    var groupList =
                        item.QuestionGroups.Select(x => new ComboObj {Text = x.GroupName, Id = x.GroupId}).ToList();

                    groupList.Add(cb);

                    listGroups.ItemsSource = groupList;

                    listGroups.SelectedValue = cb.Id;


                    var questionList =
                        item.AssessmentQuestions.Select(
                            (t, i) => new ComboObj {Text = "Question " + (i + 1), Id = t.QuestionId, Approved = t.Approved}).ToList();


                    listQuestionList.ItemsSource = questionList;

                    ShowOptionArea(false);

                    this.assessmentId = assessmentId;
                }
            }
        }


        public void LoadQuestions()
        {
            using (var service = new AssessmentQuestionService())
            {
                var list = service.GetQuestions(this.assessmentId);

                var questionList =
                    list.Select((t, i) => new ComboObj {Text = "Question " + (i + 1), Id = t.QuestionId, Approved = t.Approved}).ToList();


                listQuestionList.ItemsSource = questionList;
            }
        }


        public void LoadQuestion(int id)
        {
            using (var service = new AssessmentQuestionService())
            {
                var item = service.GetQuestion(id);

                if (item != null)
                {
                    editor.ContentHtml = item.QuestionText;
                    txtAdditionalText.Text = item.AdditionalText;

                    listTopics.SelectedValue = item.TopicId.HasValue ? item.TopicId.Value.ToString() : "0";

                    listLevels.SelectedValue = item.DifficultyLevel.HasValue
                                                   ? item.DifficultyLevel.Value.ToString()
                                                   : "0";

                    listGroups.SelectedValue = item.GroupId.HasValue ? item.GroupId.Value.ToString() : "0";

                    listOptionsType.SelectedValue = item.AnswerType;

                    listOptionsLayout.SelectedIndex = item.OptionsLayoutIsVertical ? 0 : 1;


                    bttnNewQuestion.IsEnabled = true;

                    bttnAddQuestion.Content = "Update Question";

                    IsUpdating = true;

                    ShowOptionArea(true);

                    this.Id = id;

                    this.currentQuestion = item;

                    lblStatus.Text = string.Empty;

                    LoadOptions();
                }
            }
        }

        private void bttnBackToAssessment_Click(object sender, RoutedEventArgs e)
        {
            var assessment = new NewAssessment();
            assessment.LoadAssessment(this.assessmentId);

            MainWindow.Instance.SetControlAsCurrent(assessment);
        }


        public void ShowOptionArea(bool flag)
        {
            lblOptions.Visibility =
                listOptions.Visibility =
                bttnAddOption.Visibility =
                bttnDeleteOption.Visibility = bttnSetOption.Visibility = flag ? Visibility.Visible : Visibility.Hidden;
        }

        private void bttnAddQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (IsUpdating)
            {
                UpdateQuestion();
            }
            else
            {
                AddQuestion();
            }
        }

        private void UpdateQuestion()
        {
            using (var service = new AssessmentQuestionService())
            {
                var formattedHtml = Utilities.FormatHtml(editor.ContentHtml);

                editor.ContentHtml = formattedHtml;

                string html = @"<table width='950px' border='0' cellpadding='1' cellspacing='0'> <tr><td>" + formattedHtml + @"</td></tr></table>";

                byte[] htmlImage = new Html2ImageBinary(html).GetImage();

                var item = service.GetQuestion(this.Id);

                if (item != null)
                {
                    item.TopicId = ((ComboObj) listTopics.SelectedItem).Id == 0
                                       ? default(int?)
                                       : ((ComboObj) listTopics.SelectedItem).Id;
                    item.DifficultyLevel = ((ComboObj) listLevels.SelectedItem).Id == 0
                                               ? default(int?)
                                               : ((ComboObj) listLevels.SelectedItem).Id;

                    item.GroupId = ((ComboObj) listGroups.SelectedItem).Id == 0 ? default(int?) : ((ComboObj) listGroups.SelectedItem).Id;
                    item.QuestionText = formattedHtml;
                    item.QuestionImage = htmlImage;
                    item.AdditionalText = txtAdditionalText.Text;
                    item.AnswerType = listOptionsType.SelectedValue.ToString();
                    item.OptionsLayoutIsVertical = listOptionsLayout.SelectedIndex == 0;

                    item.Approved = false;

                    var app = service.Update(item);

                    if (app.IsDone)
                    {
                        ShowStatus("Updated question successfully.", false);
                        this.currentQuestion = item;

                        LoadQuestions();
                    }
                    else
                    {
                        ShowStatus("Could not update question.");
                    }
                }
            }
        }

        public void ShowStatus(string message, bool IsErrorType = true)
        {
            lblStatus.Foreground = IsErrorType
                                       ? new SolidColorBrush((Color) ColorConverter.ConvertFromString("red"))
                                       : new SolidColorBrush((Color) ColorConverter.ConvertFromString("#003C60"));


            lblStatus.Text = message;
        }


        private void AddQuestion()
        {
            using (var service = new AssessmentQuestionService())
            {
                var formattedHtml = Utilities.FormatHtml(editor.ContentHtml);

                editor.ContentHtml = formattedHtml;

                string html = @"<table width='950px' border='0' cellpadding='1' cellspacing='0'> <tr><td>" + formattedHtml + @"</td></tr></table>";

                byte[] htmlImage = new Html2ImageBinary(html).GetImage();


                var item = new AssessmentQuestion
                    {
                        AssessmentId = this.assessmentId,
                        TopicId =
                            ((ComboObj) listTopics.SelectedItem).Id == 0
                                ? default(int?)
                                : ((ComboObj) listTopics.SelectedItem).Id,
                        DifficultyLevel =
                            ((ComboObj) listLevels.SelectedItem).Id == 0
                                ? default(int?)
                                : ((ComboObj) listLevels.SelectedItem).Id,
                        GroupId = 
                           ((ComboObj)listGroups.SelectedItem).Id == 0 
                                ? default(int?) 
                                : ((ComboObj)listGroups.SelectedItem).Id,
                        QuestionText = formattedHtml,
                        QuestionImage = htmlImage,
                        AdditionalText = txtAdditionalText.Text,
                        AnswerType = listOptionsType.SelectedValue.ToString(),
                        OptionsLayoutIsVertical = listOptionsLayout.SelectedIndex == 0,
                        Approved = false
                        
                    };

                var app = service.Add(item);

                if (app.IsDone)
                {
                    int id = (int) app.Data;
                    LoadQuestion(id);
                    LoadQuestions();

                    ShowStatus("Added question successfully.", false);
                }
                else
                {
                    ShowStatus(app.Message);
                }
            }
        }

        private void bttnNewQuestion_Click(object sender, RoutedEventArgs e)
        {
            DoNewQuestion();
        }

        private void DoNewQuestion()
        {
            IsUpdating = false;

            listQuestionList.SelectedIndex = -1;

            editor.ContentHtml = string.Empty;

            listTopics.SelectedValue = "0";
            listLevels.SelectedValue = "0";
            listGroups.SelectedValue = "0";

            txtAdditionalText.Text = string.Empty;

            bttnAddQuestion.Content = "Add Question";

            bttnNewQuestion.IsEnabled = bttnDeleteSelected.IsEnabled = bttnEditSelected.IsEnabled = false;

            lblStatus.Text = string.Empty;

            ShowOptionArea(false);
        }


        public void DoLoadQuestion()
        {
            if (listQuestionList.SelectedItem != null)
            {
                int id = ((ComboObj) listQuestionList.SelectedItem).Id;

                LoadQuestion(id);
            }
        }

        private void listQuestionList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DoLoadQuestion();
        }

        private void listQuestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bttnEditSelected.IsEnabled = bttnDeleteSelected.IsEnabled = listQuestionList.SelectedItem != null;
        }

        private void bttnEditSelected_Click(object sender, RoutedEventArgs e)
        {
            DoLoadQuestion();
        }

        private void bttnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            DeleteQuestion();
        }

        private void DeleteQuestion()
        {
            if (listQuestionList.SelectedItem != null)
            {
                MessageBox.BoxResult ret = MessageBox.DisplayMessage("Confirm",
                                                                     "Delete selected question?",
                                                                     MainWindow.Instance,
                                                                     MessageBox.BoxType.YesNo);
                if (ret == MessageBox.BoxResult.Yes)
                {
                    var item = (ComboObj) listQuestionList.SelectedItem;

                    using (var service = new AssessmentQuestionService())
                    {
                        service.Delete(item.Id);
                    }

                    LoadQuestions();
                    DoNewQuestion();
                }
            }
        }

        public void LoadOptions()
        {
            using (var service = new AssessmentAnswerService())
            {
                var list = service.GetAnswers(this.Id);

                listOptions.Items.Clear();

                foreach (var item in list)
                {
                    listOptions.Items.Add(item.IsImage
                                              ? GetImageOption(item.AnswerId, item.AnswerImage, item.IsCorrect)
                                              : GetTextOption(item.AnswerId, item.AnswerText, item.IsCorrect));
                }

                bttnDeleteOption.IsEnabled = bttnSetOption.IsEnabled = false;

            }



        }


        public ListBoxItem GetTextOption(int optionId, string optionText, bool IsSelected)
        {
            var listBoxItem = new ListBoxItem();
            listBoxItem.Margin = new Thickness(5, 5, 5, 5);

            var border = new Border();
            border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF0F0F0"));
            border.BorderThickness = new Thickness(1);

            var stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;

            var textBlock = new TextBlock();
            textBlock.Width = 825;
            textBlock.Height = 80;
            textBlock.Padding = new Thickness(10, 10, 10, 10);

            textBlock.Text = optionText;

            var chkBox = new CheckBox();

            chkBox.Margin = new Thickness(5, 30, 5, 0);

            chkBox.IsEnabled = false;
            chkBox.IsChecked = IsSelected;

            stack.Children.Add(textBlock);
            stack.Children.Add(chkBox);

            border.Child = stack;

            listBoxItem.Content = border;

            listBoxItem.Tag = optionId;

            return listBoxItem;
        }

        public ListBoxItem GetImageOption(int optionId, byte[] optionImage, bool IsSelected)
        {
            var listBoxItem = new ListBoxItem();
            listBoxItem.Margin = new Thickness(5, 5, 5, 5);

            var border = new Border();
            border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF0F0F0"));
            border.BorderThickness = new Thickness(1);

            var stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;

            var imgBorder = new Border();
            imgBorder.Width = 825;
            imgBorder.Height = 80;
            imgBorder.Padding = new Thickness(0);

            var img = new Image();
            var source = new BitmapImage();
            source.BeginInit();
            source.StreamSource = new MemoryStream(optionImage);
            source.EndInit();
            

            img.Source =  source;

            img.Height = 70;
            img.HorizontalAlignment = HorizontalAlignment.Left;

            imgBorder.Child = img;

            var chkBox = new CheckBox();

            chkBox.Margin = new Thickness(5, 30, 5, 0);

            chkBox.IsEnabled = false;
            chkBox.IsChecked = IsSelected;

            stack.Children.Add(imgBorder);
            stack.Children.Add(chkBox);

            border.Child = stack;

            listBoxItem.Content = border;

            listBoxItem.Tag = optionId;

            return listBoxItem;
        }

        private void bttnAddOption_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddOption();
            window.Owner = MainWindow.Instance;
            window.CallerInstance = this;

            window.ShowDialog();
        }

        private void listOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bttnDeleteOption.IsEnabled = bttnSetOption.IsEnabled = listOptions.SelectedItem != null;

            if (listOptions.SelectedItem != null)
            {
                int id = Int32.Parse(((ListBoxItem) listOptions.SelectedItem).Tag.ToString());

                ProcessSetButton(id);
            }
        }

        private void ProcessSetButton(int id)
        {
            using (var service = new AssessmentAnswerService())
            {
                var item = service.GetAnswer(id);

                if(item != null)
                {
                    bttnSetOption.Content = item.IsCorrect ? "Unset As Correct" : "Set As Correct";
                }

            }

        }

        private void bttnSetOption_Click(object sender, RoutedEventArgs e)
        {
            DoSetUnset();
        }

        private void DoSetUnset()
        {
            if(listOptions.SelectedItem != null)
            {
                int id = Int32.Parse(((ListBoxItem)listOptions.SelectedItem).Tag.ToString());

                using (var service = new AssessmentAnswerService())
                {
                    var item = service.GetAnswer(id);

                    if (item != null)
                    {
                        if(item.IsCorrect)
                        {
                            service.UnsetAsCorrectAnswer(item.AnswerId);
                        }
                        else
                        {
                            service.SetAsCorrectAnswer(item.AnswerId, item.QuestionId);
                        }

                        LoadOptions();
                            
                    }

                }
            }
           
        }

        private void bttnDeleteOption_Click(object sender, RoutedEventArgs e)
        {
            DoDelete();
        }

        private void DoDelete()
        {
            if (listOptions.SelectedItem != null)
            {
                int id = Int32.Parse(((ListBoxItem)listOptions.SelectedItem).Tag.ToString());

                using (var service = new AssessmentAnswerService())
                {
                   
                    service.Delete(id);

                    LoadOptions();

                    

                }
            }
        }

        private void bttnAddEditGroup_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddOrEditGroup(this.assessmentId);
            window.Owner = MainWindow.Instance;
            window.CallerInstance = this;

            window.ShowDialog();

            ReloadGroups();
        }

        private void ReloadGroups()
        {

            var item = listGroups.SelectedItem as ComboObj;


            var cb = new ComboObj { Text = "None", Id = 0 };

            var groupList =
                   new QuestionGroupService().GetGroups(this.assessmentId).Select(x => new ComboObj { Text = x.GroupName, Id = x.GroupId }).ToList();

            groupList.Add(cb);

            listGroups.ItemsSource = groupList;

            if (item != null && groupList.Contains(item))
            {
                listGroups.SelectedValue = item.Id;
            }
            else
            {
                listGroups.SelectedValue = cb.Id;
            }
        }

      
    }

    public class ComboObj
    {
        public string Text { get; set; }
        public int Id { get; set; }

        public bool Approved { get; set; }

        public string Color
        {
            get { return Approved ? "Green" : "Red"; }
        }
    }
}