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
using MahApps.Metro.Controls;
using PhotoCap.CaptureService;

namespace PhotoCap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public static MainWindow Instance;
        public static string username;
        public static string password;
        public static string hostname;

        public static StaffViewModel SelectedStaff;

        public static List<StaffViewModel> staffList;

        public static CapturePhase CapturePhase;

        public static string PhaseString;

        public MainWindow()
        {
            Instance = this;

            InitializeComponent();

            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            txtVersion.Text = string.Format("Version: {0}.{1} Build {2}", version.Major, version.Minor, version.Build);
        }

        public void SetControlAsCurrent(UserControl control)
        {
            mainGrid.Children.Clear();
            mainGrid.Children.Add(control);
        }
    }
}
