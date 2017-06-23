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
    /// Interaction logic for AddOrEditLevel.xaml
    /// </summary>
    public partial class AddOrEditLevel : MetroWindow
    {
        public NewAssessment CallerInstance { get; set; }

        public bool IsUpdating = false;
        public int Id = 0;

        public AddOrEditLevel()
        {
            InitializeComponent();
        }

        public void LoadLevel(int id)
        {
            using (var service = new QuestionDifficultyLevelService())
            {
                var item = service.GetLevel(id);

                if (item != null)
                {
                    txtLevel.Text = item.LevelName;

                    listScale.SelectedValue = item.LevelWeight.ToString();





                    this.Id = item.LevelId;

                    this.IsUpdating = true;

                    bttnAdd.Content = "Update Level";

                    this.Title = "Update Difficulty Level";

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
            if (string.IsNullOrWhiteSpace(txtLevel.Text))
            {
                ShowStatus("Enter Difficulty Level");
                return;

            }

            if (IsUpdating)
            {
                UpdateLevel();
            }
            else
            {
                AddLevel();
            }
        }

        private void AddLevel()
        {
            using (var service = new QuestionDifficultyLevelService())
            {
                var item = new QuestionDifficultyLevel
                {
                    LevelName = txtLevel.Text,
                    AssessmentId = CallerInstance.Id,
                    LevelWeight = Int32.Parse(listScale.SelectedValue.ToString())
                };


                var app = service.Add(item);

                if (app.IsDone)
                {
                    ShowStatus(app.Message, false);

                    CallerInstance.LoadLevels();

                    txtLevel.Text = string.Empty;

                }
                else
                {
                    ShowStatus(app.Message);
                }

            }
        }

        private void UpdateLevel()
        {
            using (var service = new QuestionDifficultyLevelService())
            {
                var item = service.GetLevel(this.Id);

                if (item != null)
                {
                    item.LevelName = txtLevel.Text;
                    item.LevelWeight = Int32.Parse(listScale.SelectedValue.ToString());


                    var app = service.Update(item);

                    if (app.IsDone)
                    {
                        ShowStatus(app.Message, false);

                        CallerInstance.LoadLevels();

                    }
                    else
                    {
                        ShowStatus(app.Message);
                    }
                }

            }
        }

        private void MetroWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
            txtLevel.Focus();

            if(!IsUpdating)
            {
                listScale.SelectedIndex = 0;

            }
        }
    }
}
