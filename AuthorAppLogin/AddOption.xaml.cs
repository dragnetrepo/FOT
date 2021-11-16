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
using System.Windows.Shapes;
using AuthorApp.Models;
using AuthorApp.Services;
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace AuthorApp
{
    /// <summary>
    /// Interaction logic for AddOption.xaml
    /// </summary>
    public partial class AddOption : MetroWindow
    {
        public QuestionControl CallerInstance { get; set; }

        public bool Updating { get; set; }
        public int? Id { get; set; }



        public AddOption(int? id = null)
        {
            InitializeComponent();

            if (id.HasValue)
            {
                Updating = true;
                Id = id;

                var ctx = new ServiceBase().Context;

                var item = ctx.AssessmentAnswers.FirstOrDefault(x => x.AnswerId == Id);
                if (item != null)
                {
                    if (item.IsImage == false)
                    {
                        txtOptionText.Text = item.AnswerText;
                    }

                    bttnAdd.Visibility = Visibility.Collapsed;
                    bttnUpdate.Visibility = Visibility.Visible;

                    bttnAddImage.Visibility = Visibility.Collapsed;
                    bttnUpdateImage.Visibility = Visibility.Visible;

                    checkCorrect.Visibility = Visibility.Collapsed;
                    checkCorrectImage.Visibility = Visibility.Collapsed;

                }

            }
        }

        private void bttnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(txtOptionText.Text))
            {
                 AddTextOption();
            }
           
        }

        private void AddTextOption()
        {
            using(var service = new AssessmentAnswerService())
            {
                var item = new AssessmentAnswer
                    {
                        QuestionId = CallerInstance.Id,
                        AnswerText = txtOptionText.Text,
                        IsCorrect = false,
                        IsImage = false

                    };

                if (item.IsCorrect)
                {

                    var ctx = new ServiceBase().Context;

                    var question = ctx.AssessmentQuestions.FirstOrDefault(x => x.QuestionId == CallerInstance.Id);

                    if (question?.AnswerType == "Single")
                    {

                        var list = ctx.AssessmentAnswers.Where(x => x.QuestionId == question.QuestionId).ToList();

                        list.ForEach(x => x.IsCorrect = false);

                        ctx.SaveChanges();

                    }


                }

                var app = service.Add(item);

                if (app.IsDone)
                {
                   

                    CallerInstance.LoadOptions();

                   txtOptionText.Text = string.Empty;

                }
              
            }
        }

        private void bttnBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            openDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";
            if (openDialog.ShowDialog().Value)
            {
                

                var source = new BitmapImage(new Uri(openDialog.FileName));

                if (source.PixelWidth > 700 || source.PixelHeight > 90)
                {
                    txtImgUrl.Text = string.Empty;
                    imgOption.Source = null;
                    return;
                }
                
                imgOption.Source = source;
                

                txtImgUrl.Text = openDialog.FileName;
                
            }
        }

        private void bttnAddImage_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtImgUrl.Text))
            {
                AddImageOption();
            }
        }

        private void AddImageOption()
        {
            var imgBytes = File.ReadAllBytes(txtImgUrl.Text);

            using (var service = new AssessmentAnswerService())
            {
                var item = new AssessmentAnswer
                {
                    QuestionId = CallerInstance.Id,
                    AnswerImage = imgBytes,
                    IsCorrect = false,
                    IsImage = true

                };

                if (item.IsCorrect)
                {

                    var ctx = new ServiceBase().Context;

                    var question = ctx.AssessmentQuestions.FirstOrDefault(x => x.QuestionId == CallerInstance.Id);

                    if (question?.AnswerType == "Single")
                    {

                        var list = ctx.AssessmentAnswers.Where(x => x.QuestionId == question.QuestionId).ToList();

                        list.ForEach(x => x.IsCorrect = false);

                        ctx.SaveChanges();

                    }


                }

                var app = service.Add(item);

                if (app.IsDone)
                {


                    CallerInstance.LoadOptions();

                    txtImgUrl.Text = string.Empty;
                    imgOption.Source = null;
                }

            }
        }


        private void bttnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtOptionText.Text))
            {
                UpdateTextOption();
            }
        }

        private void UpdateTextOption()
        {
            using (var service = new AssessmentAnswerService())
            {
                var item = service.GetAnswer(Id.Value);

                item.AnswerText = txtOptionText.Text;
                item.IsImage = false;
                item.AnswerImage = null;

                var app = service.Update(item);

                if (app.IsDone)
                {


                    CallerInstance.LoadOptions();

                    txtOptionText.Text = string.Empty;

                    this.Close();

                }

            }
        }

        private void bttnUpdateImage_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtImgUrl.Text))
            {
                UpdateImageOption();
            }
        }

        private void UpdateImageOption()
        {
            var imgBytes = File.ReadAllBytes(txtImgUrl.Text);

            using (var service = new AssessmentAnswerService())
            {
                var item = service.GetAnswer(Id.Value);

                item.AnswerImage = imgBytes;
                item.IsImage = true;
                item.AnswerText = null;



                var app = service.Update(item);

                if (app.IsDone)
                {


                    CallerInstance.LoadOptions();

                    txtImgUrl.Text = string.Empty;
                    imgOption.Source = null;
                    txtOptionText.Text = string.Empty;

                    this.Close();
                }

            }
        }


    }
}