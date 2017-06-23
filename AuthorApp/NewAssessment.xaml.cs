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
using Microsoft.Win32;

namespace AuthorApp
{
    /// <summary>
    /// Interaction logic for NewAssessment.xaml
    /// </summary>
    public partial class NewAssessment : UserControl
    {
        public bool IsUpdating = false;
        public int Id = 0;

        public NewAssessment()
        {
            InitializeComponent();

            txtAddTopics.Visibility = tabLower.Visibility = chkShowLower.Visibility = Visibility.Hidden;
        }

        private void chkShowLower_Click(object sender, RoutedEventArgs e)
        {
            tabLower.Visibility = chkShowLower.IsChecked.HasValue && chkShowLower.IsChecked.Value
                                      ? Visibility.Visible
                                      : Visibility.Hidden;
        }

        private void bttnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            var mainMenu = new AssessmentList();

            MainWindow.Instance.SetControlAsCurrent(mainMenu);
        }

        private void bttnAddAssessment_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                if (IsUpdating)
                {
                    UpdateAssessment();
                }
                else
                {
                    AddAssessment();
                }
            }
        }

        private void UpdateAssessment()
        {
            using (var service = new AssessmentService())
            {
                var item = service.GetAssessment(this.Id);

                var formattedHtml = FormatHtml(editor.ContentHtml);

                editor.ContentHtml = formattedHtml;

                string html = @"<table width='950px' border='0' cellpadding='1' cellspacing='0'> <tr><td>" + formattedHtml + @"</td></tr></table>";

                byte[] htmlImage = new Html2ImageBinary(html).GetImage();

                if (item != null)
                {
                    item.Name = txtAssessmentName.Text;
                    item.InstructionText = formattedHtml;
                    item.InstructionImage = htmlImage;
                    item.Timed = chkTimed.IsChecked.Value;
                    item.Duration = chkTimed.IsChecked.Value ? Int32.Parse(txtDuration.Text) : 0;
                    item.RandomizeQuestions = chkRandomizeQuestions.IsChecked.Value;
                    item.RandomizeOptions = chkRandomizeOptions.IsChecked.Value;

                    item.DateLastUpdated = DateTime.Today;


                    var app = service.Update(item);

                    if (app.IsDone)
                    {
                        ShowStatus(app.Message, false);
                    }
                    else
                    {
                        ShowStatus(app.Message);
                    }
                }
            }
        }

        private string FormatHtml(string html)
        {
            var writer = new StringWriter();
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            foreach (var img in doc.DocumentNode.Descendants("img"))
            {
                var fileName = img.Attributes["src"].Value;

                fileName = fileName.Replace(@"file:///", string.Empty);

                fileName = Uri.UnescapeDataString(fileName);


                if (fileName.StartsWith("data:image/")) continue;

                var bytes = File.ReadAllBytes(fileName);

                var base64Str = Convert.ToBase64String(bytes);

                img.Attributes["src"].Value = "data:image/jpeg;base64," + base64Str;
            }


            doc.Save(writer);

            return writer.ToString();
        }

        private void AddAssessment()
        {
            using (var service = new AssessmentService())
            {
                var formattedHtml = FormatHtml(editor.ContentHtml);

                editor.ContentHtml = formattedHtml;

                string html = @"<table width='950px' border='0' cellpadding='1' cellspacing='0'> <tr><td>" + formattedHtml + @"</td></tr></table>";

                byte[] htmlImage = new Html2ImageBinary(html).GetImage();

                var item = new Assessment
                    {
                        Name = txtAssessmentName.Text,
                        Timed = chkTimed.IsChecked.Value,
                        Duration = chkTimed.IsChecked.Value ? Int32.Parse(txtDuration.Text) : 0,
                        InstructionText =formattedHtml,
                        InstructionImage = htmlImage,
                        AssessmentType = AssessmentType.MCQ,
                        RandomizeQuestions = chkRandomizeQuestions.IsChecked.Value,
                        RandomizeOptions = chkRandomizeOptions.IsChecked.Value,
                        AdvancedOutputOptions = false,
                        QuestionsPerTest = 0,
                        DateAdded = DateTime.Today,
                        DateLastUpdated = DateTime.Today
                    };

                var app = service.Add(item);

                if (app.IsDone)
                {
                    ShowStatus(app.Message, false);

                    int id = (int) app.Data;
                    LoadAssessment(id);
                }
                else
                {
                    ShowStatus(app.Message);
                }
            }
        }


        public void LoadAssessment(int id)
        {
            using (var service = new AssessmentService())
            {
                var item = service.GetAssessment(id);

                if (item != null)
                {
                    txtAssessmentName.Text = item.Name;
                    editor.ContentHtml = item.InstructionText;
                    chkTimed.IsChecked = item.Timed;
                    txtDuration.Text = chkTimed.IsChecked.Value ? item.Duration.ToString() : string.Empty;

                    lblDuration.Visibility =
                        txtDuration.Visibility = chkTimed.IsChecked.Value ? Visibility.Visible : Visibility.Hidden;


                    chkRandomizeQuestions.IsChecked = item.RandomizeQuestions;
                    chkRandomizeOptions.IsChecked = item.RandomizeOptions;

                    chkShowLower.Visibility = txtAddTopics.Visibility = Visibility.Visible;

                    this.Id = id;

                    bttnAddAssessment.Content = "Update Assessment";

                    this.IsUpdating = true;

                    bttnQuestions.IsEnabled = true;

                    var topicList = service.Context.AssessmentTopics.Where(x => x.AssessmentId == id).ToList();
                    var levelList = service.Context.QuestionDifficultyLevels.Where(x => x.AssessmentId == id).ToList();

                    TopicGrid.ItemsSource = topicList;
                    LevelGrid.ItemsSource = levelList;

                    if (levelList.Count > 0 || topicList.Count > 0)
                    {
                        chkShowLower.IsChecked = true;
                        tabLower.Visibility = Visibility.Visible;
                    }

                    bttnExportAssessment.IsEnabled = true;
                }
            }
        }


        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(txtAssessmentName.Text))
            {
                ShowStatus("Assessment name is required!", true);
                return false;
            }

            if (string.IsNullOrWhiteSpace(editor.ContentHtml))
            {
                ShowStatus("Assessment instructions is required!", true);
                return false;
            }

            if (chkTimed.IsChecked.Value && string.IsNullOrWhiteSpace(txtDuration.Text))
            {
                ShowStatus("Duration is required if you've chosen that the assessment is timed.", true);
                return false;
            }

            int num = 0;
            if (chkTimed.IsChecked.Value && !Int32.TryParse(txtDuration.Text, out num))
            {
                ShowStatus("Duration should be a valid numeric value.", true);
                return false;
            }

            return true;
        }

        private void bttnQuestions_Click(object sender, RoutedEventArgs e)
        {
            var question = new QuestionControl();
            question.LoadContent(this.Id);

            MainWindow.Instance.SetControlAsCurrent(question);
        }

        private void chkTimed_Click(object sender, RoutedEventArgs e)
        {
            lblDuration.Visibility =
                txtDuration.Visibility = chkTimed.IsChecked.Value ? Visibility.Visible : Visibility.Hidden;
        }


        public void ShowStatus(string message, bool IsErrorType = true)
        {
            lblStatus.Foreground = IsErrorType
                                       ? new SolidColorBrush((Color) ColorConverter.ConvertFromString("red"))
                                       : new SolidColorBrush((Color) ColorConverter.ConvertFromString("#003C60"));


            lblStatus.Text = message;
        }


        public void LoadTopics()
        {
            using (var service = new AssessmentTopicService())
            {
                var list = service.GetTopics(this.Id);

                TopicGrid.ItemsSource = list;
            }
        }

        private void bttnAddTopic_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddOrEditTopic();
            window.Owner = MainWindow.Instance;
            window.CallerInstance = this;

            window.ShowDialog();

            
        }

        private void TopicGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowTopicUpdateDialog();
        }

        private void ShowTopicUpdateDialog()
        {
            if (TopicGrid.SelectedItem != null)
            {
                var item = (AssessmentTopic) TopicGrid.SelectedItem;

                var window = new AddOrEditTopic();
                window.Owner = MainWindow.Instance;
                window.CallerInstance = this;
                window.LoadTopic(item.TopicId);

                window.ShowDialog();
            }
        }

     

        private void bttnEditTopic_Click(object sender, RoutedEventArgs e)
        {
            ShowTopicUpdateDialog();
        }

        private void bttnDeleteTopic_Click(object sender, RoutedEventArgs e)
        {
            DeleteTopic();
        }

        private void DeleteTopic()
        {
            if (TopicGrid.SelectedItem != null)
            {
                MessageBox.BoxResult ret = MessageBox.DisplayMessage("Confirm",
                                                                     "Delete selected topic?",
                                                                     MainWindow.Instance,
                                                                     MessageBox.BoxType.YesNo);
                if (ret == MessageBox.BoxResult.Yes)
                {
                    var topic = (AssessmentTopic) TopicGrid.SelectedItem;

                    using (var service = new AssessmentTopicService())
                    {
                        service.Delete(topic.TopicId);
                    }

                    LoadTopics();
                }
            }
        }

        private void bttnAddLevel_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddOrEditLevel();
            window.Owner = MainWindow.Instance;
            window.CallerInstance = this;

            window.ShowDialog();
        }

        public void LoadLevels()
        {
            using (var service = new QuestionDifficultyLevelService())
            {
                var list = service.GetLevels(this.Id);

                LevelGrid.ItemsSource = list;
            }
        }

        private void ShowLevelUpdateDialog()
        {
            if (LevelGrid.SelectedItem != null)
            {
                var item = (QuestionDifficultyLevel) LevelGrid.SelectedItem;

                var window = new AddOrEditLevel();
                window.Owner = MainWindow.Instance;
                window.CallerInstance = this;
                window.LoadLevel(item.LevelId);

                window.ShowDialog();
            }
        }

     

        private void LevelGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowLevelUpdateDialog();
        }

        private void bttnEditLevel_Click(object sender, RoutedEventArgs e)
        {
            ShowLevelUpdateDialog();
        }

        private void DeleteLevel()
        {
            if (LevelGrid.SelectedItem != null)
            {
                MessageBox.BoxResult ret = MessageBox.DisplayMessage("Confirm",
                                                                     "Delete selected difficulty level?",
                                                                     MainWindow.Instance,
                                                                     MessageBox.BoxType.YesNo);
                if (ret == MessageBox.BoxResult.Yes)
                {
                    var level = (QuestionDifficultyLevel) LevelGrid.SelectedItem;

                    using (var service = new QuestionDifficultyLevelService())
                    {
                        service.Delete(level.LevelId);
                    }

                    LoadLevels();
                }
            }
        }

        private void bttnDeleteLevel_Click(object sender, RoutedEventArgs e)
        {
            DeleteLevel();
        }

        private void bttnExportAssessment_Click(object sender, RoutedEventArgs e)
        {
            ExportAssessment();
        }

        private void ExportAssessment()
        {
            using (var service = new DTOService())
            {
                int count = service.Context.AssessmentQuestions.Count(x => x.AssessmentId == this.Id);

                if (count == 0)
                {
                    ShowStatus("There are no questions in this assessment.");
                    return;
                }


                byte[] fileBytes = service.AssessmentBytes(this.Id);

                var dialog = new SaveFileDialog();

                dialog.FileName = "Assessment_" + this.Id + "_" + DateTime.Today.ToString("dd-MMM-yyyy") + ".fot";


                if (dialog.ShowDialog().Value)
                {
                    var fileName = dialog.FileName;

                    File.WriteAllBytes(fileName, fileBytes);

                    ShowStatus("Assessment exported successfully.", false);
                }
            }
        }

        private void TopicGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            bttnDeleteTopic.IsEnabled = bttnEditTopic.IsEnabled = TopicGrid.SelectedItem != null;
        }

        private void LevelGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            bttnEditLevel.IsEnabled = bttnDeleteLevel.IsEnabled = LevelGrid.SelectedItem != null;
        }
    }
}