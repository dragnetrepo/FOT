using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for UserManagementScreen.xaml
    /// </summary>
    public partial class UserManagementScreen : UserControl
    {

        public ObservableCollection<AdminUser> dataContext = new ObservableCollection<AdminUser>();

        public UserManagementScreen()
        {
            InitializeComponent();

            gridUsers.ItemsSource = dataContext;



        }


        public async Task Refresh()
        {
            var ctx = new FotAuthorContext();

            var list = ctx.AdminUsers.Where(x => x.IsAdmin == false).ToList();

            dataContext.Clear();

            list.ForEach(x => dataContext.Add(x));
        }

        private void gridUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bttnEditSelected.IsEnabled = bttnDeleteSelected.IsEnabled = gridUsers.SelectedItem != null;
        }

        private void bttnEditSelected_Click(object sender, RoutedEventArgs e)
        {
            DoEditMode();
        }

        private void bttnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            DoDelete();
        }

        private async void DoDelete()
        {

            var item = gridUsers.SelectedItem as AdminUser;


            var result = MessageBox.DisplayMessage("Confirm", "Delete this user?", MainWindow.Instance,
                MessageBox.BoxType.YesNo);

            if (result == MessageBox.BoxResult.Yes)
            {
                var ctx = new FotAuthorContext();

                var hasItems = ctx.Assessments.Any(x => x.AuthorAdminId == item.AdminId);

                if (hasItems)
                {
                    MessageBox.DisplayMessage("Delete Denied",
                        "The specified user has assessments in the system. Delete all assessments tied to this user before attempting to delete.");

                }
                else
                {
                  var admin =  ctx.AdminUsers.FirstOrDefault(x => x.AdminId == item.AdminId);

                    if (admin != null)
                    {
                        ctx.AdminUsers.Remove(admin);

                        ctx.SaveChanges();

                        await Refresh();
                    }

                }

            }


            gridUsers.SelectedIndex = -1;
        }



        public void DoEditMode()
        {
            var item = gridUsers.SelectedItem as AdminUser;

            if (item != null)
            {

                bttnUpdateUser.Tag = item.AdminId.ToString();

                bttnAddUser.Visibility = Visibility.Collapsed;
                bttnUpdateUser.Visibility = Visibility.Visible;

                txtUsername.IsReadOnly = true;

                txtUsername.Text = item.Username;
                txtFirstname.Text = item.Firstname;
                txtLastname.Text = item.Lastname;
                txtPassword.Password = item.Password;
                chkActive.IsChecked = item.Active;

                gridUsers.SelectedIndex = -1;
            }

        }


        public void ClearFields()
        {
            txtUsername.Text = string.Empty;
            txtFirstname.Text = string.Empty;
            txtLastname.Text = string.Empty;
            txtPassword.Password = string.Empty;
            chkActive.IsChecked = true;
        }
 

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await Refresh();
        }

        private void bttnAddUser_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                AddUser();
            }
        }

        private async void AddUser()
        {
            try
            {
                var ctx = new FotAuthorContext();


                var temp = ctx.AdminUsers.FirstOrDefault(x => x.Username == txtUsername.Text);

                if (temp != null)
                {
                    ShowStatus("Specified username already exists");
                    return;
                }

                var admin = new AdminUser();


                admin.Username = txtUsername.Text;
                admin.Password = txtPassword.Password;
                admin.Firstname = txtFirstname.Text;
                admin.Lastname = txtLastname.Text;

                admin.Active = chkActive.IsChecked.Value;

                admin.RegDate = DateTime.Now;

                ctx.AdminUsers.Add(admin);

                ctx.SaveChanges();

                ShowStatus("user added successfully.", false);

                ClearFields();

                await Refresh();
            }

            catch (Exception ex)
            {
                ShowStatus("An error occured. Error: " + ex.Message);
            }

        }

        private void bttnUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                UpdateUser(); 
            }
           
        }

        private async void UpdateUser()
        {
            try
            {
                var ctx = new FotAuthorContext();

                var id = Int32.Parse(bttnUpdateUser.Tag.ToString());

                var admin = ctx.AdminUsers.FirstOrDefault(x => x.AdminId == id);

                if (admin != null)
                {

                    admin.Password = txtPassword.Password;
                    admin.Firstname = txtFirstname.Text;
                    admin.Lastname = txtLastname.Text;

                    admin.Active = chkActive.IsChecked.Value;


                    ctx.SaveChanges();


                    ShowStatus("user updated successfully.", false);

                    ClearFields();

                    bttnUpdateUser.Visibility = Visibility.Collapsed;
                    bttnAddUser.Visibility = Visibility.Visible;

                    await Refresh();

                }
            }
            catch (Exception ex)
            {
                ShowStatus("An error occured. Error: "+ex.Message);
            }

        }

        public bool IsValid()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {

                MessageBox.DisplayMessage("Required Field", "Username is required");

                isValid = false;

                txtUsername.Focus();

                return isValid;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.DisplayMessage("Required Field", "Password is required");
                isValid = false;

                txtPassword.Focus();
                return isValid;
            }

            if (string.IsNullOrWhiteSpace(txtFirstname.Text))
            {
                MessageBox.DisplayMessage("Required Field", "First name is required");

                txtFirstname.Focus();

                isValid = false;

                return isValid;
            }

            if (string.IsNullOrWhiteSpace(txtFirstname.Text))
            {
                MessageBox.DisplayMessage("Required Field", "Last name is required");

                txtLastname.Focus();

                isValid = false;

                return isValid;
            }

            return isValid;
        }

        public void ShowStatus(string message, bool IsErrorType = true)
        {

            lblStatus.Foreground = IsErrorType
                                       ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("red"))
                                       : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#003C60"));


            lblStatus.Text = message;
        }


    }
}
