using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
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
using System.Xml.Linq;
using AuthorApp.Infrastructure;
using AuthorApp.Models;
using AuthorApp.Services;
using Microsoft.Win32;
using OfficeOpenXml;

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

            txtAddTopics.Visibility = tabLower.Visibility = chkShowLower.Visibility = txtFilePath.Visibility = btnBrowse.Visibility = btnImport.Visibility = Visibility.Hidden;
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
                        DateLastUpdated = DateTime.Today,
                        AuthorAdminId = MainWindow.CurrentUser.AdminId
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
                int count = service.Context.AssessmentQuestions.Count(x => x.AssessmentId == this.Id && x.Approved);

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


        private void SelectFile()
        {
            var dialog = new OpenFileDialog();

            dialog.Multiselect = false;
            dialog.DefaultExt = ".xlsx";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var ret = dialog.ShowDialog();

            if (ret.HasValue && ret.Value)
            {

                txtFilePath.Text = dialog.FileName;

                btnImport.IsEnabled = File.Exists(dialog.FileName);


            }


        }

        private void ImportExcel()
        {
            try
            {
                var fileBytes = File.ReadAllBytes(txtFilePath.Text);

                var ms = new MemoryStream(fileBytes);

                var excelEngine = new ExcelPackage(ms);


                var workBook = excelEngine.Workbook;
                var workSheet = workBook.Worksheets.First();


                var id = this.Id;

                var questions = new List<AssessmentQuestion>();

                for (int rowIndex = 2; rowIndex <= workSheet.Dimension.End.Row; rowIndex++)
                {

                    var questionText = workSheet.Cells[rowIndex, 1].Text.Trim();

                    var options = GetOptions(workSheet, rowIndex);

                    if (options.Count < 2) continue;

                    var tempQuestion = GetQuestion(questionText);

                    foreach (var option in options)
                    {
                        var answer = new AssessmentAnswer();
                        answer.AnswerText = option;
                        answer.IsCorrect = (option == options[0]);
                        answer.IsImage = false;

                        tempQuestion.AssessmentAnswers.Add(answer);

                    }
                    tempQuestion.AssessmentId = id;
                    questions.Add(tempQuestion);
                }

                if (questions.Any())
                {
                    var ctx = new ServiceBase().Context;

                    ctx.AssessmentQuestions.AddRange(questions);

                    ctx.SaveChanges();

                    var app = new AppMessage()
                    {
                        IsDone = true,
                        Message = $"{questions.Count} {(questions.Count == 1 ? "question" : "questions")} uploaded successfully.",
                        Status = MessageStatus.Success
                    };

                    // System.Windows.Forms.MessageBox.Show(app.Message);

                    ShowStatus(app.Message, IsErrorType: false);

                    txtFilePath.Text = string.Empty;
                    btnImport.IsEnabled = false;
                }
                else
                {

                    var app = new AppMessage()
                    {
                        IsDone = false,
                        Message = "No Questions were uploaded.",
                        Status = MessageStatus.Error
                    };

                    // System.Windows.Forms.MessageBox.Show(app.Message);

                    ShowStatus(app.Message, IsErrorType: true);

                }


            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("An error occured. Error: " + ex.Message);
            }


        }

        private void Import()
        {
            if (txtFilePath.Text.ToLower().EndsWith(".xlsx"))
            {
                ImportExcel();
            }
            else if (txtFilePath.Text.ToLower().EndsWith(".xml"))
            {
                ImportWordXml();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Import file should be either an Excel file (*.xlsx) or a Word 2003 XML file");
            }


        }

        private void ImportWordXml()
        {
            try
            {
                var data = File.ReadAllText(txtFilePath.Text);

                var xml = XElement.Parse(data);

                XNamespace ns = "http://schemas.microsoft.com/office/word/2003/wordml";
                XNamespace v = "urn:schemas-microsoft-com:vml";
                XNamespace wx = "http://schemas.microsoft.com/office/word/2003/auxHint";

                var body = xml.Element(ns + "body");

                var paragraphs = body.Element(wx + "sect").Descendants().Where(x => x.Name == ns + "p" || x.Name == ns + "tbl").ToList();

                paragraphs = paragraphs.Where(x => x.Ancestors(ns + "tbl").Any() == false).ToList();


                var list = new List<TempItem>();


                var entry = new TempItem();

                foreach (var item in paragraphs)
                {
                    var newPage = item.Descendants(ns + "br").Any();

                    if (newPage)
                    {
                        list.Add(entry);
                        entry = new TempItem();

                        var pageItems = item.Descendants(ns + "t").ToList();

                        var formattedPageItems = new List<string>();

                        foreach (var q in pageItems)
                        {
                            var temp = q.Value; if (string.IsNullOrWhiteSpace(temp.Trim())) continue;

                            if (q.Ancestors(ns + "r").Any())
                            {
                                var ans = q.Ancestors(ns + "r").First();

                                if (ans.Descendants(ns + "b").Any())
                                {
                                    temp = $"<strong>{temp}</strong>";
                                }

                                if (ans.Descendants(ns + "i").Any())
                                {
                                    temp = $"<i>{temp}</i>";
                                }

                                if (ans.Descendants(ns + "u").Any())
                                {
                                    temp = $"<u>{temp}</u>";
                                }
                            }

                            formattedPageItems.Add(temp);
                        }

                        var pageText = string.Join("", formattedPageItems.Select(x => x).ToList());

                        if (!string.IsNullOrWhiteSpace(pageText))
                            entry.Question = pageText;


                        continue;
                    }

                    //check if current paragraph is an option
                    bool isOption = item.Descendants(ns + "listPr").Any();

                    if (isOption)
                    {
                        var textItems = item.Descendants(ns + "t").ToList();

                        var str = string.Join("", textItems.Select(x => x.Value).ToList());

                        var option = new TempOption();

                        option.IsCorrect = str.Trim().EndsWith("*");
                        option.OptionText = option.IsCorrect ? str.Replace("*", string.Empty) : str;

                        entry.Options.Add(option);

                        continue;
                    }


                    bool isPicture = item.Descendants(ns + "pict").Any();

                    if (isPicture)
                    {
                        var imgStr = item.Descendants(ns + "binData").FirstOrDefault()?.Value;

                        if (!string.IsNullOrWhiteSpace(imgStr))
                        {
                            entry.ImageBytes = Convert.FromBase64String(imgStr);

                            entry.ImageStyle = item.Descendants(v + "shape").FirstOrDefault()?.Attribute("style").Value;

                            entry.FileName = item.Descendants(v + "imagedata").FirstOrDefault()?.Attribute("src").Value;
                        }

                        continue;
                    }

                    bool isTable = item.Name == ns + "tbl";

                    if (isTable)
                    {


                        continue;
                    }

                    //maybe regular text;

                    var questionItems = item.Descendants(ns + "t").ToList();

                    var formattedItems = new List<string>();

                    foreach (var q in questionItems)
                    {
                        var temp = q.Value; if (string.IsNullOrWhiteSpace(temp.Trim())) continue;

                        if (q.Ancestors(ns + "r").Any())
                        {
                            var ans = q.Ancestors(ns + "r").First();

                            if (ans.Descendants(ns + "b").Any())
                            {
                                temp = $"<strong>{temp}</strong>";
                            }

                            if (ans.Descendants(ns + "i").Any())
                            {
                                temp = $"<i>{temp}</i>";
                            }

                            if (ans.Descendants(ns + "u").Any())
                            {
                                temp = $"<u>{temp}</u>";
                            }
                        }

                        formattedItems.Add(temp);
                    }

                    var questionText = string.Join("", formattedItems.Select(x => x).ToList());

                    if (!string.IsNullOrWhiteSpace(questionText))
                        entry.Question = entry.Question + questionText;

                }

                list.Add(entry);

                var id = this.Id;

                var questions = new List<AssessmentQuestion>();

                foreach (var item in list)
                {

                    var questionText = item.Question;

                    var options = item.Options;

                    if (options.Count() < 2) continue;

                    var tempQuestion = item.ImageBytes == null ? GetQuestion(questionText) : GetQuestionWithImage(item);

                    foreach (var option in options)
                    {
                        var answer = new AssessmentAnswer();
                        answer.AnswerText = option.OptionText;
                        answer.IsCorrect = option.IsCorrect;
                        answer.IsImage = false;

                        tempQuestion.AssessmentAnswers.Add(answer);

                    }
                    tempQuestion.AssessmentId = id;
                    questions.Add(tempQuestion);
                }

                if (questions.Any())
                {
                    var ctx = new ServiceBase().Context;

                    ctx.AssessmentQuestions.AddRange(questions);

                    ctx.SaveChanges();

                    var app = new AppMessage()
                    {
                        IsDone = true,
                        Message = $"{questions.Count} {(questions.Count == 1 ? "question" : "questions")} uploaded successfully.",
                        Status = MessageStatus.Success
                    };

                    // System.Windows.Forms.MessageBox.Show(app.Message);

                    ShowStatus(app.Message, IsErrorType: false);

                    txtFilePath.Text = string.Empty;
                    btnImport.IsEnabled = false;
                }
                else
                {

                    var app = new AppMessage()
                    {
                        IsDone = false,
                        Message = "No Questions were uploaded.",
                        Status = MessageStatus.Error
                    };

                    // System.Windows.Forms.MessageBox.Show(app.Message);

                    ShowStatus(app.Message, IsErrorType: true);

                }



            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("An error occured. Error: " + ex.Message);
            }


        }

        public AssessmentQuestion GetQuestion(string questionText)
        {
            string html = @"<table width='950px' border='0' cellpadding='1' cellspacing='0' style='font-size: 16px;'> <tr><td><div style='font-size: 16px;'>" + questionText +
                          @"</div></td></tr></table>";


            var img = TheArtOfDev.HtmlRenderer.WinForms.HtmlRender.RenderToImage(html);

            var ms = new MemoryStream();

            img.Save(ms, ImageFormat.Png);

            byte[] htmlImage = ms.ToArray();// new Html2ImageBinary(html).GetImage();

            var item = new AssessmentQuestion
            {
                TopicId = default(int?),
                GroupId = default(int?),
                DifficultyLevel = default(int?),
                AnswerType = "Single",
                QuestionText = html,
                QuestionImage = htmlImage,
                OptionsLayoutIsVertical = true
            };

            return item;
        }

        public AssessmentQuestion GetQuestionWithImage(TempItem entry)
        {
            var mime = entry.FileName.ToLower().EndsWith(".jpg") ? "image/jpg" : "image/png";

            string html = @"<table width='950px' border='0' cellpadding='1' cellspacing='0' style='font-size: 16px;'> <tr><td><div style='font-size: 16px;'>" + entry.Question +
                          @"</div></td></tr><tr><td><div style='font-size: 16px;'><img style='" + entry.ImageStyle + "' src='data:" + mime + ";base64," + Convert.ToBase64String(entry.ImageBytes) + "'/></div></td></tr></table>";


            var img = TheArtOfDev.HtmlRenderer.WinForms.HtmlRender.RenderToImage(html);

            var ms = new MemoryStream();

            img.Save(ms, ImageFormat.Png);

            byte[] htmlImage = ms.ToArray();// new Html2ImageBinary(html).GetImage();

            var item = new AssessmentQuestion
            {
                TopicId = default(int?),
                GroupId = default(int?),
                DifficultyLevel = default(int?),
                AnswerType = "Single",
                QuestionText = html,
                QuestionImage = htmlImage,
                OptionsLayoutIsVertical = true
            };

            return item;
        }


        public List<string> GetOptions(ExcelWorksheet workSheet, int row)
        {
            var options = new List<string>();

            for (int x = 2; x <= 6; x++)
            {
                var temp = workSheet.Cells[row, x].Text.Trim();
                if (string.IsNullOrWhiteSpace(temp))
                {
                    break;
                }
                else
                {
                    if (temp.Length > 300) temp = temp.Substring(0, 300);
                    options.Add(temp);
                }
            }


            return options;
        }

        private void TopicGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            bttnDeleteTopic.IsEnabled = bttnEditTopic.IsEnabled = TopicGrid.SelectedItem != null;
        }

        private void LevelGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            bttnEditLevel.IsEnabled = bttnDeleteLevel.IsEnabled = LevelGrid.SelectedItem != null;
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            SelectFile();
        }

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            Import();
        }
    }

    public class TempItem
    {
        public string Question { get; set; }

        public byte[] ImageBytes { get; set; }

        public string ImageStyle { get; set; }

        public string FileName { get; set; }

        public List<TempOption> Options { get; set; } = new List<TempOption>();

    }

    public class TempOption
    {
        public string OptionText { get; set; }

        public bool IsCorrect { get; set; }
    }
}