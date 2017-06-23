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
    public partial class PasswordUpdate : MetroWindow
    {
       



        public PasswordUpdate()
        {
            InitializeComponent();
        }

      

        public void ShowStatus(string message, bool IsErrorType = true)
        {

            lblStatus.Foreground = IsErrorType
                                       ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("red"))
                                       : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#003C60"));


            lblStatus.Text = message;
        }

        private void bttnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                ChangePassword();
            }
        }

        private void ChangePassword()
        {
           var ctx = new FotAuthorContext();

            var item = ctx.AdminUsers.FirstOrDefault(x => x.AdminId == MainWindow.CurrentUser.AdminId);

            if (item.Password == txtOldPassword.Password)
            {
                item.Password = txtNewPassword.Password;

                ctx.SaveChanges();

                ShowStatus("Password changed successfully", false);

            }
            else
            {
                ShowStatus("Invalid old password");
            }
        }


        public bool IsValid()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtOldPassword.Password))
            {

                ShowStatus("Enter old password");
                isValid = false;

                txtOldPassword.Focus();

                return isValid;
            }

            if (string.IsNullOrWhiteSpace(txtNewPassword.Password))
            {
                ShowStatus("Enter new password");
                isValid = false;

                txtNewPassword.Focus();
                return isValid;
            }

            if (txtNewPassword.Password != txtConfirmPassword.Password)
            {
                ShowStatus("Passwords do not match");

                txtConfirmPassword.Focus();

                isValid = false;

                return isValid;
            }

            return isValid;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtOldPassword.Focus();
        }
    }
}
