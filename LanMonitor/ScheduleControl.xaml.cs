using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LanMonitor.AppServices;
using LanMonitor.FotService;

namespace LanMonitor
{
    /// <summary>
    /// Interaction logic for ScheduleControl.xaml
    /// </summary>
    public partial class ScheduleControl : UserControl
    {
        public string VersionHash = "15ba233d69c54d4683c11e09f531f68e";

        public ScheduleControl()
        {
            InitializeComponent();
        }


        public void EnableControls(bool flag)
        {
            bttnDownloadSchedule.IsEnabled =
                txtPassword.IsEnabled =
                txtUsername.IsEnabled = bttnDownloadAssessment.IsEnabled = listBoxAssessments.IsEnabled = flag;

            progressRing.IsActive = !flag;
        }


        private void bttnDownloadSchedule_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.DisplayMessage("Error", "Enter username and password", MainWindow.Instance);
                return;
            }

            MessageBox.BoxResult ret = MessageBox.DisplayMessage("Confirm",
                                                                 "Downloading a new schedule will delete all candidates that were downloaded in a previous day. It will also bring in any candidate that has been recently added to the schedule. Are you sure you want to continue?",
                                                                 MainWindow.Instance,
                                                                 MessageBox.BoxType.YesNo);
            if (ret == MessageBox.BoxResult.Yes)
            {
                DownloadSchedule();

                SettingsControl.instance.CheckCaptureSettings();

            }
        }

        private async void DownloadSchedule()
        {
            using (var service = new FotServiceClient())
            {
                try
                {
                    lblStatus.Text = string.Empty;

                    EnableControls(false);


                    SchedulePackage result = await service.GetSchedulesAsync(txtUsername.Text, txtPassword.Password, VersionHash);
                    

                    if (result.IsDone)
                    {
                        if (result.ScheduleList.Count > 0)
                        {

                           await Task.Factory.StartNew(() => ProcessPackage(result));
						   EnableControls(true);
                        }
                        else
                        {
                            ShowInfo("No entries found.");
							EnableControls(true);
                        }

                        
                    }
                    else
                    {
                        EnableControls(true);

                        ShowError(result.ErrorMessage);
                    }
                }
                catch (Exception ex)
                {
                    EnableControls(true);
                    ShowError("An Error occured! Error: " + ex.Message);
                }
            }
        }

        private async void ProcessPackage(SchedulePackage result)
        {
            try
            {
                var service = new AppServiceClient();

                await service.DeleteAllAsync(result.DownloadDate);

                var candidateList = new List<Candidate>();

                var staffList = new List<AdminUser>();


                result.ScheduleList.ForEach(x => candidateList.Add(new Candidate
                {
                    CandidateId = x.CandidateId,
                    Username = x.Username,
                    Password = x.Password,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    MobileNo = x.MobileNo,
                    BundleId = x.BundleId,
                    CampaignId = x.CampaignId,
                    ShowFeedback = x.ShowFeedback,
                    SessionId = x.SessionId,
                    CandidateGuid = Guid.NewGuid().ToString().Replace("-", ""),
                    DownloadDate = result.DownloadDate
                }));

                await service.AddAsync(candidateList);

                List<RequiredAssessment> realList =
                    await
                        service.GetRequiredAssessmentListAsync(
                            result.AssessmentList.Select(
                                x => new RequiredAssessment {BundleId = x.BundleId, Name = x.Name}).ToList());

                await service.AddRequiredAssessmentsAsync(realList);


                result.StaffList.ForEach(x => staffList.Add(new AdminUser
                {
                    ActualUserId = x.UserId,
                    IsCaptureAdmin = x.IsCaptureAdmin,
                    IsSupportStaff = x.IsSupportStaff,
                    Username = x.Username,
                    Password = x.Password,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    DownloadDate = result.DownloadDate
                }));


                await service.AddStaffAsync(staffList);



                var requiredList = await service.GetRequiredAssessmentsAsync();

                Dispatcher.Invoke(() =>
                {
                    listBoxAssessments.ItemsSource = requiredList;

                    ShowInfo("Processed " + result.ScheduleList.Count + " candidates successfully.");
                });



                await OverviewControl.Instance.LoadGrid();

            }
            catch (Exception ex)
            {
                MessageBox.DisplayMessage("Error", "An Error occured! Error: " + ex.Message, MainWindow.Instance);
            }
        }


        public void ShowError(string message)
        {
            lblStatus.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FFF03030"));
            lblStatus.Text = message;
        }

        public void ShowInfo(string message)
        {
            lblStatus.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FF0E7C95"));
            lblStatus.Text = message;
        }

        private void bttnDownloadAssessment_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxAssessments.SelectedIndex < 0)
            {
                MessageBox.DisplayMessage("Error", "No assessment was selected.", MainWindow.Instance);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.DisplayMessage("Error", "Enter username and password", MainWindow.Instance);
                return;
            }


            ProcessDownload();
        }

        private async void ProcessDownload()
        {
            using (var service = new FotServiceClient())
            {
                try
                {
                    lblStatus.Text = string.Empty;

                    EnableControls(false);

                    var item = listBoxAssessments.SelectedItem as RequiredAssessment;


                    BundlePackage result =
                        await service.GetAssessmentPackageAsync(txtUsername.Text, txtPassword.Password, item.BundleId);


                    if (result.IsDone)
                    {
                       SavePackage(result);

                        EnableControls(true);
                    }
                    else
                    {
                        EnableControls(true);

                        ShowError("An Error occured! Error: " + result.ErrorMessage);
                    }
                }
                catch (Exception ex)
                {
                    EnableControls(true);
                    ShowError("An Error occured! Error: " + ex.Message);
                }
            }
        }

        private async void SavePackage(BundlePackage result)
        {
            try
            {
                var service = new AppServiceClient();

                var assessmentPackage = new AssessmentPackage
                {
                    BundleId = result.BundleId,
                    BundleName = result.BundleName,
                    BundleData = result.BundleData,
                    DownloadDate = result.DownloadDate
                };

                await service.SaveBundleAsync(assessmentPackage);

                listBoxAssessments.ItemsSource = service.GetRequiredAssessments();

                ShowInfo("Assessment downloaded successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.DisplayMessage("Error", "An Error occured! Error: " + ex.Message, MainWindow.Instance);
            }

        }

   

        private async Task LoadList()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }

            try
            {

                Task task = Task.Delay(1000);

                var service = new AppServiceClient();

                await task.ContinueWith(_ =>
                {

                    Dispatcher.Invoke(() => { listBoxAssessments.ItemsSource = service.GetRequiredAssessments(); });

                });
            }
            catch (Exception ex)
            {
                MessageBox.DisplayMessage("Error", "An Error occured! Error: " + ex.Message, MainWindow.Instance);
            }
        }

   

        private async void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }

            await LoadList();
            
        }
    }
}