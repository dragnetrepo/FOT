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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AuthorApp.Models;
using AuthorApp.Services;

namespace AuthorApp
{
    /// <summary>
    /// Interaction logic for AssessmentList.xaml
    /// </summary>
    public partial class AssessmentList : UserControl
    {
        public AssessmentList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            LoadAssessments();
        }

        private void LoadAssessments()
        {
           

            Task.Factory.StartNew(() => 
            {
                using (var service = new AssessmentService())
                {
                    var list = service.GetAssessments();

                    Dispatcher.Invoke((() =>
                    {
                        AssessmentGrid.ItemsSource = list;

                    }));

                }

            });
        }

     

        private void bttnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            var newAssessment = new NewAssessment();

            MainWindow.Instance.SetControlAsCurrent(newAssessment);
        }

        private void AssessmentGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(AssessmentGrid.SelectedItem != null)
            {
                var item = (AssessmentViewModel) AssessmentGrid.SelectedItem;

                LoadAssessment(item.AssessmentId);
            }
        }


        public void LoadAssessment(int id)
        {
            var assessment = new NewAssessment();
            assessment.LoadAssessment(id);
            MainWindow.Instance.SetControlAsCurrent(assessment);
        }

        private void bttnEditAssessment_Click(object sender, RoutedEventArgs e)
        {
            if (AssessmentGrid.SelectedItem != null)
            {
                var item = (AssessmentViewModel)AssessmentGrid.SelectedItem;

                LoadAssessment(item.AssessmentId);
            }
        }

        private void bttnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            if (AssessmentGrid.SelectedItem != null)
            {
                MessageBox.BoxResult ret = MessageBox.DisplayMessage("Confirm",
                                                                     "Delete selected assessment? All corresponding attributes will be deleted.",
                                                                     MainWindow.Instance,
                                                                     MessageBox.BoxType.YesNo);
                if (ret == MessageBox.BoxResult.Yes)
                {
                    var item = (AssessmentViewModel)AssessmentGrid.SelectedItem;

                    using (var service = new AssessmentService())
                    {
                        service.Delete(item.AssessmentId);

                    }

                   LoadAssessments();

                }

            }
        }

        private void AssessmentGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bttnDeleteSelected.IsEnabled = bttnEditAssessment.IsEnabled = AssessmentGrid.SelectedItem != null;

            if (AssessmentGrid.SelectedItem != null)
            {
                var item = AssessmentGrid.SelectedItem as AssessmentViewModel;

                bttnPreviewSelected.IsEnabled = item.QuestionCount > 0;

            }
        }

        private void bttnPreviewSelected_Click(object sender, RoutedEventArgs e)
        {
            var item = AssessmentGrid.SelectedItem as AssessmentViewModel;

            var preview = new AssessmentPreview();
            preview.SetAssessmentToPreview(item.AssessmentId);

            MainWindow.Instance.SetControlAsCurrent(preview);
        }

  
    }

  
}
