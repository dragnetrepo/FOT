using System;
using System.Collections.Generic;
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

namespace AuthorApp
{
    /// <summary>
    /// Interaction logic for AddOrEditGroup.xaml
    /// </summary>
    public partial class AddOrEditGroup : MetroWindow
    {
           public QuestionControl CallerInstance { get; set; }

           public int AssessmentId { get; set; }

        public AddOrEditGroup(int Id)
        {
            this.AssessmentId = Id;

            InitializeComponent();
        }


    

        public void ShowStatus(string message, bool IsErrorType = true)
        {

            lblStatus.Foreground = IsErrorType
                                       ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("red"))
                                       : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#003C60"));


            lblStatus.Text = message;
        }



  



        private void MetroWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
            txtGroupName.Focus();

            LoadGrid();
        }

        private void LoadGrid()
        {
            Task.Factory.StartNew(() =>
                {
                    var groupList = new QuestionGroupService().GetGroups(this.AssessmentId);


                    Dispatcher.Invoke(() => { GroupGrid.ItemsSource = groupList; });
                });
        }

        private void txtGroupName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void GroupGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bttnDelete.IsEnabled = GroupGrid.SelectedItem != null;

        }

        private void bttnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (GroupGrid.SelectedItem != null)
            {
                var item = GroupGrid.SelectedItem as QuestionGroup;

                new QuestionGroupService().Delete(item.GroupId);
                LoadGrid();
                GroupGrid.SelectedIndex = -1;

                ShowStatus("Entry deleted!", false);
            }
        }

        private void bttnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddGroup();
        }

        private void AddGroup()
        {
            if (string.IsNullOrWhiteSpace(txtGroupName.Text))
            {
                ShowStatus("No group name specified.");
                return;
            }


            var item = new QuestionGroup
                {
                    AssessmentId = this.AssessmentId,
                    GroupName = txtGroupName.Text

                };

            var app = new QuestionGroupService().Add(item);

            if (app.IsDone)
            {
                ShowStatus(app.Message, false);
                txtGroupName.Text = string.Empty;
                LoadGrid();
            }
            else
            {
                ShowStatus(app.Message);
            }
        }
    }
}
