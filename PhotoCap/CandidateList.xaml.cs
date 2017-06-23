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
using PhotoCap.CaptureService;

namespace PhotoCap
{
    /// <summary>
    /// Interaction logic for CandidateList.xaml
    /// </summary>
    public partial class CandidateList : UserControl
    {
        public CandidateList()
        {
            InitializeComponent();
        }

        private void bttnNext_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.SetControlAsCurrent(new CaptureScreen());
        }



        public void LoadCandidates()
        {
            CandidateGrid.ItemsSource = MainWindow.candidateList;

        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            LoadCandidates();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearch.Text.Trim().Length < 1)
            {
                CandidateGrid.ItemsSource = MainWindow.candidateList;
            }
            else
            {
                string text = txtSearch.Text.Trim().ToLower();

                var subList = MainWindow.candidateList.Where(x => x.Username.ToLower().Contains(text) || x.Firstname.ToLower().Contains(text) || x.Lastname.ToLower().Contains(text)).ToList();

                CandidateGrid.ItemsSource = subList;

            }

            lblCandidateName.Text = string.Empty;
            bttnNext.IsEnabled = false;
            MainWindow.SelectedCandidate = null;
        }

        private void CandidateGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

            if(CandidateGrid.SelectedItem != null)
            {
                bttnNext.IsEnabled = true;

                var item = (CandidateViewModel)CandidateGrid.SelectedItem;

                lblCandidateName.Text = item.Firstname + " " + item.Lastname;

                MainWindow.SelectedCandidate = item;

            }
        }
    }
}
