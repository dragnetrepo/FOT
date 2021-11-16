using System;
using System.Collections.Generic;
using System.IO;
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
using AuthorApp.Models;
using AuthorApp.Services;

namespace AuthorApp
{
    /// <summary>
    /// Interaction logic for AssessmentPreview.xaml
    /// </summary>
    public partial class AssessmentPreview : UserControl
    {
        public int AssessmentId { get; set; }
        public Assessment assessment { get; set; }
        public List<AssessmentQuestion> questionList { get; set; }
        private int totalQuestions = 0;
        private int currentQuestion = -1;

        private FotAuthorContext ctx;
        public AssessmentPreview()
        {
            InitializeComponent();

             ctx = new FotAuthorContext();

            chkApproved.Visibility = MainWindow.CurrentUser.IsAdmin ? Visibility.Visible : Visibility.Hidden;

        }

        private void bttnAssessments_Click(object sender, RoutedEventArgs e)
        {
            var mainMenu = new AssessmentList();

            MainWindow.Instance.SetControlAsCurrent(mainMenu);
        }

        public void SetAssessmentToPreview(int id)
        {
            this.AssessmentId = id;
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (AssessmentId > 0)
            {
                assessment = new AssessmentService().GetAssessmentForPreview(AssessmentId);

                if(assessment != null)
                LoadPreview();
            }


        }

        private void LoadPreview()
        {
            lblAssessmentName.Text = assessment.Name;

            currentQuestion = 0;

            totalQuestions = assessment.AssessmentQuestions.Count;

             questionList = assessment.AssessmentQuestions.ToList();

            RenderCurrentQuestion();
        }

        private void RenderCurrentQuestion()
        {
            lblQuestionCount.Text = string.Format("QUESTION {0} OF {1}", currentQuestion + 1, totalQuestions);

            bttnPrevious.IsEnabled = (currentQuestion > 0);

            bttnNext.IsEnabled = (currentQuestion < totalQuestions - 1);

            

            using (var stream = new MemoryStream(questionList[currentQuestion].QuestionImage))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();

                imgQuestion.Source = bitmap;
            }

            txtBlockAdditionalText.Text = questionList[currentQuestion].AdditionalText;

            var list = questionList[currentQuestion].AssessmentAnswers.ToList();


            listOptions.Items.Clear();

            foreach (var item in list)
            {
                listOptions.Items.Add(item.IsImage
                                          ? GetImageOption(item.AnswerId, item.AnswerImage, item.IsCorrect)
                                          : GetTextOption(item.AnswerId, item.AnswerText, item.IsCorrect));
            }


            chkApproved.IsChecked = questionList[currentQuestion].Approved;



        }

        private void bttnPrevious_Click(object sender, RoutedEventArgs e)
        {
            --currentQuestion;

            RenderCurrentQuestion();
        }

        private void bttnNext_Click(object sender, RoutedEventArgs e)
        {
            ++currentQuestion;

            RenderCurrentQuestion();
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
            textBlock.Width = 985;
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
            imgBorder.Width = 985;
            imgBorder.Height = 80;
            imgBorder.Padding = new Thickness(0);

            var img = new Image();
            var source = new BitmapImage();
            source.BeginInit();
            source.StreamSource = new MemoryStream(optionImage);
            source.EndInit();


            img.Source = source;

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

        private void chkApproved_Click(object sender, RoutedEventArgs e)
        {
            

            var flag = chkApproved.IsChecked;

            questionList[currentQuestion].Approved = flag.Value;

            var id = questionList[currentQuestion].QuestionId;


            var item = ctx.AssessmentQuestions.FirstOrDefault(x => x.QuestionId == id);

            item.Approved = flag.Value;


            ctx.SaveChanges();


        }
    }
}
