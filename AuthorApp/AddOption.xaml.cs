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


       


        public AddOption()
        {
            InitializeComponent();
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
                        IsCorrect = checkCorrect.IsChecked.HasValue && checkCorrect.IsChecked.Value,
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
                    IsCorrect = checkCorrectImage.IsChecked.HasValue && checkCorrectImage.IsChecked.Value,
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
    }
}