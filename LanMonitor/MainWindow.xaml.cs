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
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace LanMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public static MainWindow Instance;
        public MainWindow()
        {
            Instance = this;

            InitializeComponent();

            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            txtVersion.Text = string.Format("Version: {0}.{1} Build {2}", version.Major, version.Minor, version.Build);

        }

        private void bttnClear_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.DisplayMessage("Confirm", "Clear Entire Database?", this, MessageBox.BoxType.YesNo);
        }
    }
}
