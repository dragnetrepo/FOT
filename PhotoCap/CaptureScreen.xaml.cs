using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PhotoCap.CaptureService;
using TouchlessLib;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;
using UserControl = System.Windows.Controls.UserControl;

namespace PhotoCap
{
    /// <summary>
    /// Interaction logic for CaptureScreen.xaml
    /// </summary>
    public partial class CaptureScreen : UserControl
    {
        public bool IsStarted = false;
        public BitmapImage currentImage;
        public byte[] currentImageBytes;

        private TouchlessMgr touchMgr;
        private DateTime lastFrameTime;
        private static Bitmap _latestFrame;
       

        public CaptureScreen()
        {
            InitializeComponent();
        }

        public void Preview()
        {
            pic2.Visible = false;
            host2.Visibility = Visibility.Collapsed;


            pic.Visible = true;
            host.Visibility = Visibility.Visible;

            try
            {
                Camera c = (Camera)listCameras.SelectedItem;
                c.OnImageCaptured += new EventHandler<CameraEventArgs>(OnImageCaptured);
                touchMgr.CurrentCamera = c;
                lastFrameTime = DateTime.Now;

                pic.Paint += new PaintEventHandler(drawLatestImage);

                bttnPreviewCapture.Content = "Capture Photo";


                bttnSave.IsEnabled = false;

            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show(ex.Message); 
            }



        }


        private void drawLatestImage(object sender, PaintEventArgs e)
        {
            if (_latestFrame != null)
            {
                // Draw the latest image from the active camera
                e.Graphics.DrawImage(_latestFrame, 0, 0, pic.Width, pic.Height);


            }
        }

        public void OnImageCaptured(object sender, CameraEventArgs args)
        {

            double milliseconds = (DateTime.Now.Ticks - lastFrameTime.Ticks) / TimeSpan.TicksPerMillisecond;
            if (milliseconds >= 100)
            {

                lastFrameTime = DateTime.Now;

                _latestFrame = args.Image;
                pic.Invalidate();
            }


        }


        public void CapturePhoto()
        {

            StopCamera();

            pic2.Visible = true;
            host2.Visibility = Visibility.Visible;


            pic.Visible = false;
            host.Visibility = Visibility.Collapsed;


            bttnPreviewCapture.Content = "Preview Photo";

            pic2.Image = (Bitmap)_latestFrame.Clone();
            pic.Visible = false;
            host.Visibility = Visibility.Collapsed;

            StopCamera();
            IsStarted = false;

            System.Drawing.Image img = pic2.Image;

            

            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            currentImageBytes = ms.ToArray();

            ms.Close();

            bttnSave.IsEnabled = true;

        }


        public void TrashCamera()
        {
            // Trash the old camera
            if (touchMgr.CurrentCamera != null)
            {
                touchMgr.CurrentCamera.OnImageCaptured -= new EventHandler<CameraEventArgs>(OnImageCaptured);
                touchMgr.CurrentCamera.Dispose();
                touchMgr.CurrentCamera = null;

                pic.Paint -= new PaintEventHandler(drawLatestImage);


            }
        }

        public void DisposeTouchMgr()
        {
            TrashCamera();
            // Dispose of the TouchlessMgr object
            if (touchMgr != null)
            {
                touchMgr.Dispose();
                touchMgr = null;
            }
        }

        public void StopCamera()
        {
            // Trash the old camera
            if (touchMgr.CurrentCamera != null)
            {
                touchMgr.CurrentCamera.OnImageCaptured -= new EventHandler<CameraEventArgs>(OnImageCaptured);
                //  touchMgr.CurrentCamera.Dispose();
                touchMgr.CurrentCamera = null;

                pic.Paint -= new PaintEventHandler(drawLatestImage);


            }
        }

  

        private void bttnCandidateList_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.SetControlAsCurrent(new CandidateList());
        }

        private void bttnPreviewCapture_Click(object sender, RoutedEventArgs e)
        {
            if (bttnPreviewCapture.Content.ToString().Equals("Preview Photo"))
            {
                Preview();

                


            }
            else if (bttnPreviewCapture.Content.ToString().Equals("Capture Photo"))
            {
                CapturePhoto();

                
            }
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            lblCandidateName.Text = MainWindow.SelectedCandidate.Firstname + " " + MainWindow.SelectedCandidate.Lastname;

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                touchMgr = new TouchlessMgr();

                foreach (Camera cam in touchMgr.Cameras)
                {
                    listCameras.Items.Add(cam);

                }

                if (listCameras.Items.Count > 0) listCameras.SelectedIndex = 0;
            }

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

        private void bttnSave_Click(object sender, RoutedEventArgs e)
        {
            DoSave();
        }

        private async void DoSave()
        {
            using (var service = new ImageCaptureServiceClient())
            {
                try
                {
                    EnableControls(false);
                    progressRing.IsActive = true;

                    var hostName = "http://" + MainWindow.hostname + "/ImageCaptureService.svc";
                    service.Endpoint.Address = new EndpointAddress(hostName);

                    var result = await service.UpdateCandidateAsync(MainWindow.SelectedCandidate.CandidateId, currentImageBytes, MainWindow.username, MainWindow.password);

                    if (result.Succeeded)
                    {

                        MainWindow.candidateList.Remove(MainWindow.SelectedCandidate);

                        EnableControls(true);
                        progressRing.IsActive = false;


                        ShowInfo("Candidate details updated successfully.");

                        MessageBox.DisplayMessage("Info", "Candidate details updated successfully.", MainWindow.Instance);


                        var candidateList = new CandidateList();


                        MainWindow.Instance.SetControlAsCurrent(candidateList);


                    }
                    else
                    {
                        EnableControls(true);
                        progressRing.IsActive = false;
                        ShowError("An Error occured. Error: " + result.ErrorMessage);
                    }


                }
                catch (Exception ex)
                {
                    EnableControls(true);
                    progressRing.IsActive = false;

                    ShowError("An Error occured! Error: " + ex.Message);
                }
            }
        }

        private void EnableControls(bool flag)
        {
            bttnSave.IsEnabled = flag;
            bttnPreviewCapture.IsEnabled = flag;
            bttnCandidateList.IsEnabled = flag;
        }
    }
}
