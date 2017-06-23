using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using LanMonitor.AppServices;
using LanMonitor.FotService;
using LanMonitor.Properties;

namespace LanMonitor
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
       

        public static SettingsControl instance;
        public SettingsControl()
        {
            InitializeComponent();

            SettingsControl.instance = this;
        }


        public void CheckCaptureSettings()
        {
            try
            {
                var service = new AppServiceClient();

                service.SetImageCaptureSetting(true);


                Dispatcher.Invoke(() =>
                {
                    chkImageCapture.IsChecked = true;
                });
            }
            catch (Exception ex)
            {
                MessageBox.DisplayMessage("Error", "An Error occured! Error: " + ex.Message, MainWindow.Instance);
            }
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
           
                RefreshStatus();
            
        }

        private async void RefreshStatus()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }

            try
            {
                var service = new AppServiceClient();

                await Task.Run(async () =>
                {


                    bool flag = await service.GetImageCapatureSettingAsync();

                    var items = await service.GetStaffListAsync();

                    var totalCaptured = items.Count(x => x.PostTestPhoto != null);
                    var synchronized = items.Count(x => x.Synchronized);

                    var enableSync = totalCaptured > synchronized;

                    Dispatcher.Invoke(() =>
                    {
                        chkImageCapture.IsChecked = flag;
                        txtTotalCaptured.Text = totalCaptured.ToString();
                        txtTotalSynchronized.Text = synchronized.ToString();

                        bttnSynchronize.IsEnabled = enableSync;

                        if (enableSync)
                        {
                            txtEodPassword.IsEnabled = txtEodUsername.IsEnabled = true;
                        }

                    });

                });

            }

            catch (Exception ex)
            {
                MessageBox.DisplayMessage("Error", "An Error occured! Error: " + ex.Message, MainWindow.Instance);
            }
        }

        private async void chkImageCapture_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                var service = new AppServiceClient();

                await service.SetImageCaptureSettingAsync(true);
            }

            catch (Exception ex)
            {
                MessageBox.DisplayMessage("Error", "An Error occured! Error: " + ex.Message, MainWindow.Instance);
            }

        }

        private async void chkImageCapture_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                var service = new AppServiceClient();

                await service.SetImageCaptureSettingAsync(false);
            }
            catch (Exception ex)
            {
                MessageBox.DisplayMessage("Error", "An Error occured! Error: " + ex.Message, MainWindow.Instance);
            }

        }

        public void ShowError(string message)
        {
            //lblStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF03030"));
            //lblStatus.Text = message;
        }

        public void ShowInfo(string message)
        {
            //lblStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0E7C95"));
           // lblStatus.Text = message;
        }

      

        private void bttnEndOfDay_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEodUsername.Text) || string.IsNullOrWhiteSpace(txtEodPassword.Password))
            {
                MessageBox.DisplayMessage("Error", "Enter username and password", MainWindow.Instance);
                return;
            }

            MessageBox.BoxResult ret = MessageBox.DisplayMessage("Confirm",
                                                               "Triggering the 'End of day' will prevent you from downloading any assessment/schedule for the current day. Are you sure you want to continue?",
                                                               MainWindow.Instance,
                                                               MessageBox.BoxType.YesNo);
            if (ret == MessageBox.BoxResult.Yes)
            {
                TriggerEndOfDay();
            }
        }

        private async void TriggerEndOfDay()
        {

           

            using (var service = new FotServiceClient())
            {
                try
                {

                    var localService = new AppServiceClient();




                    EnableControls(false);

                    var items = await localService.GetPhotoLogsAsync();

                    var logs =
                        items.Select(
                            x =>
                                new PhotoLogModel
                                {
                                    CandidateId = x.CandidateId,
                                    AdminUserId = x.AdminUserId,
                                    ExpungeDate = x.ExpungeDate
                                }).ToList();


                    var result = await service.TriggerEndOfDayAsync(txtEodUsername.Text, txtEodPassword.Password, logs);

                    if(result.IsDone)
                    {
                        var candidateService = new AppServiceClient();
                        
                           await  candidateService.DeleteEverythingAsync();

                        

                        lblStatusEndOfDay.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0E7C95"));
                        lblStatusEndOfDay.Text = "End of day triggered successfully.";

                        RefreshEndOfDayStatus();
                    }
                    else
                    {
                        lblStatusEndOfDay.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF03030"));
                        lblStatusEndOfDay.Text = "Could not trigger end of day.";

                        RefreshEndOfDayStatus();
                    }

                    progressRing.IsActive = false;

                }
                catch (Exception ex)
                {
                    lblStatusEndOfDay.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF03030"));
                    lblStatusEndOfDay.Text = "An error occured. Error: "+ ex.Message;

                    progressRing.IsActive = false;
                }

            }

        }

        public void EnableControls(bool flag)
        {
            bttnEndOfDay.IsEnabled =
                txtEodPassword.IsEnabled =
                txtEodUsername.IsEnabled  = flag;

            progressRing.IsActive = !flag;
        }


        private void bttnEndOfDay_Loaded(object sender, RoutedEventArgs e)
        {
            
                RefreshEndOfDayStatus();
            
        }

        private async void RefreshEndOfDayStatus()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }

            try
            {
                var service = new AppServiceClient();

                await Task.Run(async () =>
                {


                    var enable = await service.CanTriggerEndOfDayAsync();

                    Dispatcher.Invoke(
                        () => { bttnEndOfDay.IsEnabled = txtEodUsername.IsEnabled = txtEodPassword.IsEnabled = enable; });

                });
            }
            catch (Exception ex)
            {
                MessageBox.DisplayMessage("Error", "An Error occured! Error: " + ex.Message, MainWindow.Instance);
            }
        }

        private void bttnSynchronize_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEodUsername.Text) || string.IsNullOrWhiteSpace(txtEodPassword.Password))
            {
                MessageBox.DisplayMessage("Error", "Enter username and password", MainWindow.Instance);
                return;
            }

            SynchronizePhotos();
        }

        private async void SynchronizePhotos()
        {
            using (var service = new FotServiceClient())
            {
                try
                {
                    bttnSynchronize.IsEnabled = false;
                    progressRingSync.IsActive = true;

                    var localService = new AppServiceClient();

                    var adminUsers = await localService.GetPendingStaffListAsync();

                    var staffList = adminUsers.Select(x => new PersonnelPhotoUpdateModel
                    {
                        UserId = x.ActualUserId,
                        IsSupportStaff = x.IsSupportStaff,
                        PreTestPhoto = x.PreTestPhoto,
                        PostTestPhoto = x.PostTestPhoto,
                        PreTestCapturedByAdminId = x.PreTestCapturedByAdminId.Value,
                        PostTestCapturedByAdminId = x.PostTestCapturedByAdminId.Value,
                        TestDate = x.DownloadDate.Date
                    }).ToList();


                    var result = await service.PersonnelPhotoUpdateAsync(txtEodUsername.Text, txtEodPassword.Password, staffList);

                    if (result.IsDone)
                    {
                        lblStatusEndOfDay.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0E7C95"));
                        lblStatusEndOfDay.Text = "Synchronized personnel photos successfully.";

                        await localService.SynchronizeStaffListAsync(adminUsers.Select(x => x.ActualUserId).ToList());

                    }
                    else
                    {
                        lblStatusEndOfDay.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF03030"));
                        lblStatusEndOfDay.Text = "Error: " + result.ErrorMessage;
 
                    }

                    RefreshStatus();
                    progressRingSync.IsActive = false;

                }
                catch (Exception ex)
                {
                    lblStatusSynchronize.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF03030"));
                    lblStatusSynchronize.Text = "An error occured. Error: " + ex.Message;
                    progressRingSync.IsActive = false;
                    bttnSynchronize.IsEnabled = true;
                    
                }

            }

        }
    }
}
