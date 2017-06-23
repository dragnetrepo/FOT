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
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace PhotoCap
{
    /// <summary>
    /// Interaction logic for MessageBox.xaml
    /// </summary>
    public partial class MessageBox : MetroWindow
    {
        public enum BoxType { OK, YesNo }

        BoxType type;

        public enum BoxResult { None, Yes, No, Cancel }

        public BoxResult Result;



        public MessageBox(string Title, string Message, BoxType Type)
        {

            InitializeComponent();

            //ThemeManager.ChangeAppTheme(this);

            this.Title = Title;

            txtMessage.Text = Message;

            type = Type;

            switch (type)
            {

                case BoxType.OK:

                    btn1.Content = "OK";

                    btn1.Visibility = System.Windows.Visibility.Visible;

                    btn2.Visibility = System.Windows.Visibility.Hidden;

                    break;

                case BoxType.YesNo:

                    btn1.Content = "NO";

                    btn2.Content = "YES";

                    btn1.Visibility = System.Windows.Visibility.Visible;

                    btn2.Visibility = System.Windows.Visibility.Visible;

                    break;

                default:

                    break;

            }

        }



        public static BoxResult DisplayMessage(string Title, string Message, Window owner = null, BoxType Type = BoxType.OK)
        {

            MessageBox mb = new MessageBox(Title, Message, Type);

            mb.Owner = owner;

            mb.ShowDialog();



            return mb.Result;

        }



        private void btn1_Click(object sender, RoutedEventArgs e)
        {

            switch (type)
            {

                case BoxType.OK:

                    Result = BoxResult.None;

                    this.Close();

                    break;

                case BoxType.YesNo:

                    Result = BoxResult.No;

                    this.Close();

                    break;

                default:

                    Result = BoxResult.None;

                    break;

            }

        }



        private void btn2_Click(object sender, RoutedEventArgs e)
        {

            switch (type)
            {

                case BoxType.YesNo:

                    Result = BoxResult.Yes;

                    this.Close();

                    break;

                default:

                    Result = BoxResult.None;

                    break;

            }

        }
    }
}
