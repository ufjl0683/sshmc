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
using Visifire.Charts;
using Visifire.Commons;
using System.IO;

namespace SensorMoniter
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {

      
        Comm.SirfDLE dev;
        System.IO.Ports.SerialPort port;
        int inx = 0;
        bool SaveStatus = false;
        Microsoft.Win32.SaveFileDialog file = new Microsoft.Win32.SaveFileDialog();
        public MainWindow(object settinginfo)
        {
            InitializeComponent();

            string[] settings = settinginfo.ToString().Split(",".ToCharArray());
           

            

            

            CreateChart(settinginfo);


            //Comm.SirfDLE dev;
            //System.IO.Ports.SerialPort port = new System.IO.Ports.SerialPort("COM4", 115200, System.IO.Ports.Parity.None);
            //port.Open();
            //dev = new Comm.SirfDLE("Tile", port.BaseStream);

            //dev.OnReceiveText += new Comm.OnTextPackageEventHandler(dev_OnReceiveText);
           

        }





        System.Collections.Generic.List<Point> vc1 = new List<Point>();
        System.Collections.Generic.List<Point> vc2 = new List<Point>();







        double value1, value2,temp1,temp2 ;
        //int linecont = 0;
        private void CreateChart(object settinginfo)
        {

            string SensorValue = "", CommunicateValue = "", IP_COM_portValue = "";
            int Poot_BaudValue = 0;
            string[] split = settinginfo.ToString().Split(',');
            SensorValue = split[0];
            CommunicateValue = split[1];
            IP_COM_portValue = split[2];
            Poot_BaudValue = Convert.ToInt32(split[3]);


            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.QuickLine;
            dataSeries.Color = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            dataSeries.LightingEnabled = true;
            //dataSeries.MovingMarkerEnabled = true;
             DataPoint dataPoint = new DataPoint();

             DataSeries dataSeries2 = new DataSeries();
             dataSeries2.LightingEnabled = true;
             dataSeries2.RenderAs = RenderAs.QuickLine;
             dataSeries2.Color = new SolidColorBrush(Color.FromRgb(0,255,0));
             //dataSeries2.MovingMarkerEnabled = true;
             DataPoint dataPoint2 = new DataPoint();

            Axis axis = new Axis();
            axis.StartFromZero = false;
            axis.LineThickness = 0;
            //axis.AxisMaximum = 5;
            //axis.AxisMinimum = 0;

            Axis axis2 = new Axis();
            axis2.StartFromZero = false;
            axis2.LineThickness = 0;
            //axis2.AxisMaximum = 5;
            //axis2.AxisMinimum = 0;


            chart1.AxesY.Add(axis);
            chart2.AxesY.Add(axis2);


            chart1.Titles.Add(new Title() { Text = SensorValue + "1" });
            chart2.Titles.Add(new Title() { Text = SensorValue + "2" });




            

            //System.IO.Ports.SerialPort port = new System.IO.Ports.SerialPort("COM4", 115200, System.IO.Ports.Parity.None);
         if(CommunicateValue=="TCP")
             {
                 System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
                 try
                 {
                     client.Connect(IP_COM_portValue, Poot_BaudValue);
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show(ex.Message);
                     this.Close();
                     throw ex;
                 }
                 dev= new  Comm.SirfDLE("Tile", client.GetStream());
             }
            else
            {
            port = new System.IO.Ports.SerialPort("COM"+IP_COM_portValue, Poot_BaudValue, System.IO.Ports.Parity.None);
            port.Open();
            
            dev = new Comm.SirfDLE("Tile", port.BaseStream);
            }

            dev.OnCommError += new Comm.OnCommErrHandler(dev_OnCommError);


            
            dev.OnReceiveText += (s1, e1) =>
            {
               // double volt1, volt2, value1, value2, temperature;

                //volt1 = (txtObj.Text[0] + txtObj.Text[1] * 256 + txtObj.Text[2] * 256 * 256) / Math.Pow(2, 24) * 5;
                //volt2 = (txtObj.Text[3] + txtObj.Text[4] * 256 + txtObj.Text[5] * 256 * 256) / Math.Pow(2, 24) * 5;
              //  double temp1, temp2;
               // temp1 = (e1.Text[8] - 197.0) / -1.083; ;
                
                //volt1 = Comm.Util.ThreeBytesToInt(new byte[] { txtObj.Text[0], txtObj.Text[1], txtObj.Text[2] }) / Math.Pow(2, 24) * 4.2 * 2;
                //volt2 = Comm.Util.ThreeBytesToInt(new byte[] { txtObj.Text[3], txtObj.Text[4], txtObj.Text[5] }) / Math.Pow(2, 24) * 4.2 * 2;

                //volt1 = Comm.Util.ThreeBytesToInt(new byte[] { e1.Text[2], e1.Text[3], e1.Text[4] }) / Math.Pow(2, 24) * 4.2 * 2;
                //volt2 = Comm.Util.ThreeBytesToInt(new byte[] { e1.Text[5], e1.Text[6], e1.Text[7] }) / Math.Pow(2, 24) * 4.2 * 2;
           //     value1 = volt1 / 0.28;
              //  value2 = volt2 / 0.28;
                try
                {
                    value1 = System.BitConverter.ToInt32(e1.Text, 0) / 1e6;
                    value2 = System.BitConverter.ToInt32(e1.Text, 4) / 1e6;
                    temp1 = (e1.Text[8] - 197.0) / -1.083;
                    temp2 = (e1.Text[9] - 197.0) / -1.083;   // temp2
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "," + ex.StackTrace);
                }
              //  SetDataToQueue(new double[] { value1, value2 });

                //volt1 = ((e1.Text[0] + e1.Text[1] * 256 + e1.Text[2] * 256 * 256) / Math.Pow(2, 24) * 5);
                //volt2 = ((e1.Text[3] + e1.Text[4] * 256 + e1.Text[5] * 256 * 256) / Math.Pow(2, 24) * 5)  ;

               
                inx++;

             

                this.Dispatcher.Invoke(
                 new Action(delegate()
                 {

                     try
                     {

                         DataPoint p1 = new DataPoint();


                         if (txtCom.Text.Length > 3000)
                             txtCom.Text = txtCom.Text.Substring(0, 1000);
                         txtCom.Text = Comm.V2DLE.ToHexString(e1.Text) + "\r\n" + txtCom.Text;

                         //p1.AxisXLabel = inx.ToString();
                         p1.YValue = value1;
                         dataSeries.DataPoints.Add(p1);






                         DataPoint p2 = new DataPoint();

                         //p2.AxisXLabel = inx.ToString();
                         p2.YValue = value2;
                         dataSeries2.DataPoints.Add(p2);



                         if (dataSeries.DataPoints.Count > 50)
                             dataSeries.DataPoints.RemoveAt(0);
                         if (dataSeries2.DataPoints.Count > 50)
                             dataSeries2.DataPoints.RemoveAt(0);

                         text1.Text = value1.ToString();
                         text2.Text = value2.ToString();



                         //存檔條件判斷
                         if (SaveStatus == true)
                         {

                             if (!(file.FileName == ""))
                             {
                                 if (File.Exists(file.FileName) == false)
                                 {
                                     using (StreamWriter sw = File.CreateText(file.FileName))
                                     {
                                         sw.WriteLine(value1 + " , " + value2 + "," + temp1 + "," + temp2);// temp2
                                         //sw.WriteLine(DateTime.Now);

                                         sw.Close();
                                     }
                                 }
                                 else
                                 {
                                     try
                                     {
                                         using (StreamWriter sw = File.AppendText(file.FileName))
                                         {
                                             sw.WriteLine(value1 + " , " + value2 + "," + temp1 + "," + temp2);  //temp2
                                             //sw.WriteLine(DateTime.Now);

                                             sw.Close();
                                         }

                                     }
                                     catch (Exception ex)
                                     {

                                     }
                                 }
                             }



                             //this.txtFile.Text = file.SafeFileName;




                         }
                         else
                         {
                             Save.Content = "Start Save";
                         }
                     }
                     catch (Exception ex)
                     {
                         MessageBox.Show(ex.Message + "," + ex.StackTrace);
                     }


                 }));

                     
            };

            chart1.ScrollingEnabled = false;
            chart1.AnimatedUpdate = true;
            chart1.Series.Add(dataSeries);
            chart1.Theme = "Theme3";
            //canvas1.Children.Add(chart);




            chart2.ScrollingEnabled = false;
            chart2.AnimatedUpdate = true;
            chart2.Series.Add(dataSeries2);
            chart2.Theme = "Theme3";
            //canvas2.Children.Add(chart2);
        }

        void dev_OnCommError(object sender, Exception ex)
        {
            MessageBox.Show(ex.Message);

            //throw new NotImplementedException();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (SaveStatus == false)
            {
                SaveStatus = true;
                file.Filter = "(*.txt)|*.txt";
                file.Title = "選擇存檔路徑";
                file.ShowDialog();
                Save.Content = "Stop Save";
                
            }
            else if (SaveStatus == true)
                SaveStatus = false;
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                dev.Close();
                port.Close();
            }
            catch { ;}
           
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //if(!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "log.txt"))
            //   System.IO.File.
             


           StreamWriter wr=  System.IO.File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "log.txt");

           wr.WriteLine(txtIdentify.Text + "," + value1 + "," + value2 + "," + temp1  );
           wr.Close();
        }

    
       
    }
}
