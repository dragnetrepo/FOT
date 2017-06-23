using System.Windows;
using System.Windows.Media;
using AuthorApp.Models;
using AuthorApp.Services;
using MahApps.Metro.Controls;

namespace AuthorApp
{
    /// <summary>
    /// Interaction logic for AddOrEditTopic.xaml
    /// </summary>
    public partial class AddOrEditTopic : MetroWindow
    {
        public NewAssessment CallerInstance { get; set; }

        public bool IsUpdating = false;
        public int Id = 0;

        public AddOrEditTopic()
        {
            InitializeComponent();
        }


        public void LoadTopic(int id)
        {
            using (var service = new AssessmentTopicService())
            {
                var topic = service.GetTopic(id);

                if(topic != null)
                {
                    txtTopic.Text = topic.Topic;
                    this.Id = topic.TopicId;

                    this.IsUpdating = true;

                    bttnAdd.Content = "Update Topic";

                    this.Title = "Update Topic";

                }

            }
        }

        public void ShowStatus(string message, bool IsErrorType = true)
        {

            lblStatus.Foreground = IsErrorType
                                       ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("red"))
                                       : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#003C60"));


            lblStatus.Text = message;
        }

        private void bttnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtTopic.Text))
            {
                ShowStatus("Enter Topic");
                return;
                
            }

            if(IsUpdating)
            {
                UpdateTopic();
            }
            else
            {
                AddTopic();
            }
        }

        private void UpdateTopic()
        {
            using (var service = new AssessmentTopicService())
            {
                var item = service.GetTopic(this.Id);

                if(item != null)
                {
                    item.Topic = txtTopic.Text;

                    var app = service.Update(item);

                    if (app.IsDone)
                    {
                        ShowStatus(app.Message, false);

                        CallerInstance.LoadTopics();

                    }
                    else
                    {
                        ShowStatus(app.Message);
                    }
                }

            }

        }

        private void AddTopic()
        {
            using (var service = new AssessmentTopicService())
            {
                var item = new AssessmentTopic
                    {
                        Topic = txtTopic.Text,
                        AssessmentId = CallerInstance.Id
                    };


                var app = service.Add(item);

                if (app.IsDone)
                {
                    ShowStatus(app.Message, false);

                   CallerInstance.LoadTopics();

                    txtTopic.Text = string.Empty;

                }
                else
                {
                    ShowStatus(app.Message);
                }

            }
        }

        private void MetroWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
            txtTopic.Focus();

        }
    }
}
