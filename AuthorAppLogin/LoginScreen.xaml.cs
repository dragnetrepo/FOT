using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace AuthorApp
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : UserControl
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private async void bttnLogin_Click(object sender, RoutedEventArgs e)
        {
           await DoLogin();

            //lblStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF36AE41"));
        }

        private async Task DoLogin()
        {
            try
            {
                loginPanel.IsEnabled = false;

                var ctx = new FotAuthorContext();

                var admin = await ctx.AdminUsers.FirstOrDefaultAsync(x => x.Username == txtUsername.Text);

                if (admin != null)
                {

                    if (admin.Password == txtPassword.Password)
                    {

                        if (admin.Active)
                        {

                            MainWindow.CurrentUser = admin;

                            var assessmentList = new AssessmentList();

                            MainWindow.Instance.ShowButtonsOnLogin();
                            MainWindow.Instance.SetControlAsCurrent(assessmentList);

                        }

                        else
                        {
                            loginPanel.IsEnabled = true;

                            lblStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDC1D1D"));
                            lblStatus.Content = "Account is currently inactive.";
                        }

                    }
                    else
                    {
                        loginPanel.IsEnabled = true;

                        lblStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDC1D1D"));
                        lblStatus.Content = "Invalid username or password";
                    }

                }
                else
                {

                    loginPanel.IsEnabled = true;

                    lblStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDC1D1D"));
                    lblStatus.Content = "Invalid username or password";

                }



            }
            catch (Exception ex)
            {
                loginPanel.IsEnabled = true;

                lblStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDC1D1D"));
                lblStatus.Content = "An Error occured: Error: " + ex.Message;
            }
        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableLogin();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            EnableLogin();
        }


        public void EnableLogin()
        {
            bttnLogin.IsEnabled = (txtPassword.Password.Length > 0 && txtUsername.Text.Length > 0);

        }
      
    }
}
