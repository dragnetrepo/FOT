using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel;
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
using PhotoCap.CaptureService;


namespace PhotoCap
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : UserControl
    {
        string fileName = "host.xml";

        public LoginScreen()
        {
            InitializeComponent();
        }

        public void ShowError(string message)
        {
            lblStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF03030"));
            lblStatus.Text = message;
        }

        public void ShowInfo(string message)
        {
            lblStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0E7C95"));
            lblStatus.Text = message;
        }


        public bool Validate()
        {
            if(string.IsNullOrWhiteSpace(txtHostname.Text))
            {
                ShowError("Enter hostname");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                ShowError("Enter username");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                ShowError("Enter password");
                return false;
            }

            return true;
        }

        private void bttnLogin_Click(object sender, RoutedEventArgs e)
        {
            if(Validate())
            {
                DoLogin();
            }
          
        }

        private  async void DoLogin()
        {
            using(var service = new ImageCaptureServiceClient())
            {
                try
                {
                    EnableControls(false);
                    progressRing.IsActive = true;

                    var hostName = "http://" + txtHostname.Text + "/ImageCaptureService.svc";
                    service.Endpoint.Address = new EndpointAddress(hostName);

                    var result = await service.GetStaffListAsync(txtUsername.Text, txtPassword.Password);

                    if(result.Succeeded)
                    {
                        MainWindow.staffList = result.StaffList;

                        MainWindow.username = txtUsername.Text;
                        MainWindow.password = txtPassword.Password;
                        MainWindow.hostname = txtHostname.Text;

                        MainWindow.CapturePhase = result.CapturePhase;

                        MainWindow.PhaseString = result.CapturePhase.ToString();

                        SetHostname(txtHostname.Text);

                        var candidateList = new CandidateList();
                       

                        MainWindow.Instance.SetControlAsCurrent(candidateList);


                    }
                    else
                    {
                        EnableControls(true);
                        progressRing.IsActive = false;
                        ShowError("An Error occured. Error: "+ result.ErrorMessage);
                    }


                }
                catch(Exception ex)
                {
                    EnableControls(true);
                    progressRing.IsActive = false;

                    ShowError("An Error occured! Error: " + ex.Message);
                }
            }
        }



        public void EnableControls(bool flag)
        {
            bttnLogin.IsEnabled = flag;
            txtHostname.IsEnabled = flag;
            txtUsername.IsEnabled = flag;
            txtPassword.IsEnabled = flag;
        }



        public void SetHostname(string host)
        {
            try
            {

                string data = "<Data><Hostname>" + host + "</Hostname></Data>";

                string fullpath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                FileStream fs = new FileStream(fullpath, FileMode.Create);
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    // Add the data to the file.
                    sw.Write(data);

                }

                fs.Close();

            }
            catch (Exception ex)
            {

            }

        }

        public string GetHostname()
        {
            string fullpath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            if (!File.Exists(fullpath)) return string.Empty;

            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(fullpath);

                DataTable dt = ds.Tables["Data"];

                string host = dt.Rows[0]["Hostname"].ToString();


                return host;

            }

            catch (Exception ex)
            {

                return string.Empty;
            }

        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            txtHostname.Text = GetHostname();
        }
    }
}
