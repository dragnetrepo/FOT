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
using System.Windows.Threading;
using LanMonitor.AppServices;

namespace LanMonitor
{
    /// <summary>
    /// Interaction logic for OverviewControl.xaml
    /// </summary>
    public partial class OverviewControl : UserControl
    {
        public static OverviewControl Instance;

        private DispatcherTimer timer;


        public OverviewControl()
        {
            Instance = this;

            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }

            timer = new DispatcherTimer();


            timer.Interval = TimeSpan.FromSeconds(10);

             timer.Tick += async (sender, args) => await LoadGrid();
            

            timer.IsEnabled = true;

            timer.Start();

        }

        private async void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
           
                 await LoadGrid();
            
           
            
        }

    


        public async Task LoadGrid()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }


            try
            {

                await Task.Run(async () =>
                {
                    var service = new AppServiceClient();

                    var list = await service.GetCandidatesForSummaryAsync();
                    int total = list.Count();
                    int started = list.Count(x => x.AssessmentStarted);
                    int completed = list.Count(x => x.AssessmentCompleted);
                    int synchronized = list.Count(x => x.Synchronized);

                    await Dispatcher.Invoke(async () =>
                    {
                        candidateGrid.ItemsSource = await service.GetCandidatesAsync();

                        txtTotal.Text = total.ToString("#,##0");
                        txtCompleted.Text = completed.ToString("#,##0");
                        txtStarted.Text = started.ToString("#,##0");
                        txtSynchronized.Text = synchronized.ToString("#,##0");
                    });



                });


            }
            catch (Exception ex)
            {
                MessageBox.DisplayMessage("Error", "An Error occured! Error: " + ex.Message, MainWindow.Instance);
            }

        }
     

     
    }


}
