using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SensorMoniter
{
    /// <summary>
    /// SelectSensor.xaml 的互動邏輯
    /// </summary>
    public partial class SelectSensor : Page
    {
        public SelectSensor()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new System.Uri("/MainWindow.xaml", UriKind.Relative));
            //NavigationWindow NW = new NavigationWindow();
            //NW.Show();

            //foreach (Control ctrl in this groupBox1.Controls)
            //  {
            //      if (ctrl is RadioButton)
            //      {
            //          if (((RadioButton)ctrl).Checked )
            //          {
            //              //添加你需要的操作
            //          }
            //      }
            //  }

            string SensorValue = "", CommunicateValue = "";

            if (TiltRadioButton.IsChecked == true)
                SensorValue = "Tilt";
            else if (GPSRadioButton.IsChecked == true)
                SensorValue = "GPS";
            else if (LaserRadioButton.IsChecked == true)
                SensorValue = "Laser";
            if (TCPRadioButton.IsChecked == true)
                CommunicateValue = "TCP";
            else if (COMRadioButton.IsChecked == true)
                CommunicateValue = "COM";
            

            //Application.Current.Properties["SettingInfo"] = list;
            try
            {
                MainWindow MW = new MainWindow(SensorValue + "," + CommunicateValue + "," + IP_COM_PORT_TXTBOX.Text + "," + POOT_BAUD_TXTBOX.Text);
                MW.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            
        }
    }
}
