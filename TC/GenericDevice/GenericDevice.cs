using System;
//using System.Linq;
using Comm;
using RemoteInterface.SensorConfig;
using Microsoft.JScript;

namespace GenericDevice
{
    class GenericDevice : SensorBase
    {
        
        //System.Collections.Generic.Queue<double> queValue1 = new System.Collections.Generic.Queue<double>();
        //System.Collections.Generic.Queue<double> queValue2 = new System.Collections.Generic.Queue<double>();

        GenericController controller;
        public GenericDevice(int id, GenericController controller, string deviceName, System.Net.IPEndPoint endpoint)
            : base(id, deviceName, endpoint )
        {
           // this.DeviceName= GPSName;
           // this.endpoint = endpoint;
          //  com = new System.IO.Ports.SerialPort(ComName, baud, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
          //  com.Open();
            this.controller = controller;
           
          
         
        }

        public GenericDevice(int id, GenericController controller, string devicename, string ComName, int baud)
            : base(id, devicename, ComName, baud )
        {
          //  this.DeviceName= GPSName;

            this.controller = controller;
            //new System.Threading.Thread(ProcessGpsSignal).Start();
          
        }

#if DEBUG

        long readcnt=0;

#endif

        public override void sensorDev_OnReceiveText(object sender, TextPackage txtObj)
        {
          //  throw new NotImplementedException();
            
            double   orgvalue0,orgvalue1,orgvalue2;

            double value0 = System.BitConverter.ToDouble(txtObj.Text, 0);
            double value1 = System.BitConverter.ToDouble(txtObj.Text, 8);
            double value2 = System.BitConverter.ToDouble(txtObj.Text, 16);
           // Console.WriteLine("{0} {1} {2}", x, y, z);
            //volt1 = (txtObj.Text[0] + txtObj.Text[1] * 256 + txtObj.Text[2] * 256 * 256) / Math.Pow(2, 24) * 5;
            //volt2 = (txtObj.Text[3] + txtObj.Text[4] * 256 + txtObj.Text[5] * 256 * 256) / Math.Pow(2, 24) * 5;
            //double temp1, temp2;
            //temp1 = (txtObj.Text[0] - 197.0) / -1.083; ;
            //temp2 = (txtObj.Text[1] - 197.0) / -1.083; ;
            //volt1 = Comm.Util.ThreeBytesToInt(new byte[] { txtObj.Text[0], txtObj.Text[1], txtObj.Text[2] }) / Math.Pow(2, 24) * 4.2 * 2;
            //volt2 = Comm.Util.ThreeBytesToInt(new byte[] { txtObj.Text[3], txtObj.Text[4], txtObj.Text[5] }) / Math.Pow(2, 24) * 4.2 * 2;

            //volt1 = Comm.Util.ThreeBytesToInt(new byte[] { txtObj.Text[2], txtObj.Text[3], txtObj.Text[4] }) / Math.Pow(2, 24) * 4.2 * 2;
            //volt2 = Comm.Util.ThreeBytesToInt(new byte[] { txtObj.Text[5], txtObj.Text[6], txtObj.Text[7] }) / Math.Pow(2, 24) * 4.2 * 2;
            //value1 = volt1 / 0.28;
            //value2 =volt2 / 0.28;
            orgvalue0 = value0;
            orgvalue1=value1  ;
            orgvalue2=value2  ;
         //   temperature = (txtObj.Text[8] - 197.0) / -1.083;
            string formula = this.controller.config.sensors[ID].sensor_values[0].ConvertFormula;
            try
            {
                value0 = System.Convert.ToDouble(Eval.JScriptEvaluate(string.Format(formula, value0, value1, value2), jseng));
            }
            catch (Exception ex)
            {
                Console.WriteLine("snrid:" + ID + "valueinx:0" + ex.Message + "," + ex.StackTrace + string.Format(formula, value0, value1, value2));
                return;
            }
            formula = this.controller.config.sensors[ID].sensor_values[1].ConvertFormula;
            try{
            value1 = System.Convert.ToDouble(Eval.JScriptEvaluate(string.Format(formula, value0, value1, value2), jseng));
            }
            catch(Exception ex)
            {
                 Console.WriteLine("snrid:"+ID+"valueinx:1"+ex.Message+","+ex.StackTrace+ string.Format(formula, value0, value1, value2));
                 return;
            }
            formula = this.controller.config.sensors[ID].sensor_values[2].ConvertFormula;
            try
            {
                value2 = System.Convert.ToDouble(Eval.JScriptEvaluate(string.Format(formula, value0, value1, value2), jseng));
            }
            catch (Exception ex)
            {
                Console.WriteLine("snrid:" + ID + "valueinx:2" + ex.Message + "," + ex.StackTrace + string.Format(formula, value0, value1, value2));
                return;
            }
         //   object res = Eval.JScriptEvaluate("var a=10;a+2*6+5;Math.sin(0);", eng);
            SetDataToQueue(new double[] { value0, value1,value2  });
            Console.WriteLine(this.SensorName + ":" + "V0:{0:0.0000}  V1:{1:0.0000}  V2:{2:0.0000} Org0:{3:0.0000} Org1:{4:0.0000} Org2{5:0.0000}  ", value0, value1,  value2,orgvalue0,orgvalue1,orgvalue2);
#if DEBUG
            //if (readcnt++ % 100 == 0)
            //{
                //System.IO.StreamWriter sw;
                //if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "log.csv"))
                //    sw = System.IO.File.CreateText(AppDomain.CurrentDomain.BaseDirectory + "log.csv");
                //else
                //    sw = System.IO.File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "log.csv");

                //sw.WriteLine(volt1 + "," + temp1 + "," + volt2 + "," + temp2);
                //sw.Flush();
                //sw.Close();

                //Console.WriteLine(this.SensorName + ":" + " Volt1:{0:0.0000}/{2:0.0000} Volt2:{1:00.0000}/{3:00.0000}  temperature1:{4:00.0000}  temperature2:{5:00.0000}", volt1, volt2, volt1 / 0.28, volt2 / .28, temp1, temp2);
            //}
#endif
       
        }

        //public double GetValue1()
        //{
        //    System.Collections.Generic.List<double> list = new List<double>(queValue1);
        //    list.Sort();
        //    double[] values = list.ToArray();
        //    double sum = 0;
        //    int totalcnt = list.Count;

        //    int cnt = 0;
        //    for (int i = (int)(totalcnt * 0.2); i < (int)(totalcnt * .8); i++)
        //    {
        //        sum += values[i];
        //        cnt++;
        //    }

        //    if (cnt == 0 || totalcnt != MAX_QUEUE_CNT)
        //    {
        //       // queValue1.Clear();
        //        return double.NegativeInfinity;
        //    }
        //    if (DateTime.Now.Subtract(lastReceiveTime) > TimeSpan.FromMinutes(1))
        //    {
        //        queValue1.Clear();
        //        return double.NegativeInfinity;
        //    }
         
        //    return sum / cnt;


        //}
        //public double GetValue2()
        //{
        //    System.Collections.Generic.List<double> list = new List<double>(queValue2);
        //    list.Sort();
        //    double[] values = list.ToArray();
        //    double sum = 0;
        //    int totalcnt = list.Count;

        //    int cnt = 0;
        //    for (int i = (int)(totalcnt * 0.2); i < (int)(totalcnt * .8); i++)
        //    {
        //        sum += values[i];
        //        cnt++;
        //    }

        //    if (cnt == 0 || totalcnt != MAX_QUEUE_CNT)
        //    {
        //      //  queValue2.Clear();
        //        return double.NegativeInfinity;
               
        //    }

        //    if (DateTime.Now.Subtract(lastReceiveTime) > TimeSpan.FromMinutes(1))
        //    {
        //        queValue2.Clear();
        //        return double.NegativeInfinity;
        //    }
            
        //    return sum / cnt;


        //}



        public override void SensorDev_OnCommError(object sender)
        {
         //   throw new NotImplementedException();
        }
    }
}
