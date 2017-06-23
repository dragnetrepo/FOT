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
using MahApps.Metro.Controls;

namespace AuthorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public static MainWindow Instance;

        public static AdminUser CurrentUser;

        public MainWindow()
        {
            Instance = this;

            InitializeComponent();

            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            txtVersion.Text = string.Format("Version: {0}.{1} Build {2}", version.Major, version.Minor, version.Build);
        }


        public void SetControlAsCurrent(UserControl control)
        {
            if (control is AssessmentList && CurrentUser.IsAdmin)
            {
                control = new AdminAssessmentList();
            }

            MainGrid.Children.Clear();
            MainGrid.Children.Add(control);
        }

        public void ShowButtonsOnLogin()
        {
            bttnCommands.Visibility = Visibility.Visible;


            bttnAssessments.Visibility =
                bttnUsers.Visibility =
                    (CurrentUser.IsAdmin) ? Visibility.Visible : Visibility.Hidden;


            if (CurrentUser.IsAdmin)
            {
                bttnAssessments.IsEnabled = false;
                bttnUsers.IsEnabled = true;
            }


        }

        public void HideButtonsOnLogout()
        {
            bttnCommands.Visibility = Visibility.Hidden;
        }

        private void bttnLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.CurrentUser = null;

            var loginScreen = new LoginScreen();
            

            MainWindow.Instance.HideButtonsOnLogout();

            MainWindow.Instance.SetControlAsCurrent(loginScreen);

        }

        private void bttnUsers_Click(object sender, RoutedEventArgs e)
        {
            var screen = new UserManagementScreen();

            Instance.SetControlAsCurrent(screen);

            bttnUsers.IsEnabled = false;
            bttnAssessments.IsEnabled = true;
        }

        private void bttnAssessments_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AssessmentList();

            Instance.SetControlAsCurrent(screen);

            bttnUsers.IsEnabled = true;
            bttnAssessments.IsEnabled = false;
        }

        private void bttnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var window = new PasswordUpdate();
            window.Owner = MainWindow.Instance;


            window.ShowDialog();
        }
    }
}