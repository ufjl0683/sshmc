using System;
using System.Collections.Generic;
using System.Text;
using RemoteInterface.SensorConfig;
using Comm;
using GPSDevice.GPSMessage;

namespace GPSDevice
{
    public  class UbloxGPSController:Comm.Controller.SSHDataController
    {
        public Queue<GPSMessage.GPSData> refGpsDataQueue = new Queue<GPSMessage.GPSData>();
        public long LastRefTimeStamp = -1;
        public UbloxDevice refDevice;

        // GPSDevice[] devices;
        GPSControllerConfig Configuration;
        string configfile;
      

        public UbloxGPSController(GPSControllerConfig config, PropertyBag property)
             : base(config, property)
         { 
             
             try
             {
                
                 this.Configuration = config;
                 this.PropertyBag.TransmitMode = 1;
                 this.PropertyBag.TransmitCycle = 10;
                 this.SetTransmitCycleTmr();
             
             
             }
             catch(Exception ex)
             {
                 Console.WriteLine("config.xml 讀取錯誤!,"+ex.Message);
                 Environment.Exit(-1);
             }

             try
             {
                 refDevice = (UbloxDevice)CreateDevice(Configuration.ref_gps);



                 refDevice.ProcessCompletedEvent += new ProcessCompletedHandler(dev_ProcessCompletedEvent);
                 
             }
             catch (Exception Exception){
                 Console.WriteLine(Exception.Message+","+Exception.StackTrace);}
         }

       
       
        

       protected override  Comm.SensorBase CreateDevice(SensorConfigBase conf)
         {
          
             GPSSensorConfig config = (GPSSensorConfig)conf;
             if (config.com_type == "TCP")
             {
                // if (config.is_reference)
                 return new UbloxDevice(config.id, this, config.device_name, new System.Net.IPEndPoint(System.Net.IPAddress.Parse(config.ip_comport), config.port_baud), config.refx, config.refy, config.refz, config.is_reference);
                 //else
                 //    return new UbloxDevice(config.id, this, config.device_name, new System.Net.IPEndPoint(System.Net.IPAddress.Parse(config.ip_comport), config.port_baud));
             }
             else  //COM
             {
                // if (config.is_reference)
                 return new UbloxDevice(config.id, this, config.device_name, config.ip_comport, config.port_baud, config.refx, config.refy, config.refz, config.is_reference);
                 //else
                 //    return new UbloxDevice(config.id, this, config.device_name, config.ip_comport, config.port_baud);
             }

         }

          void dev_ProcessCompletedEvent(GPSDeviceBase sender, GPSMessage.GPSData data)
         {
             lock (refGpsDataQueue)
             {
                 if (data.TrackCnt == GPSDevice.constMinValidDataCnt)
                     refGpsDataQueue.Enqueue(data);
                 else
                     return;

                 LastRefTimeStamp = (long)data.TimeStamp;
                 if (refGpsDataQueue.Count >3)
                 {

                     foreach (UbloxDevice dev in devices)
                     {
                         if (dev != null && dev.IsReference == false)
                             dev.Signal();
                     }


                     if (refGpsDataQueue.Count > 10)
                         refGpsDataQueue.Dequeue();

                 }

             }

         }
      
          //public override void OnReceiveText(Comm.TextPackage txt) //from center
          //{
             
             


             
          //}

          //protected override void OnPeriodDataReport()
          //{
          //   // throw new NotImplementedException();
          //}

          protected override void OnOneMinTmrTask()
          {
              try
              {
                  Console.WriteLine("================One Min Task============"); ;
                  if (devices.Length == 0)
                      return;
                  double[] values = new double[devices.Length * 3];
                  for (int i = 0; i < devices.Length; i++)
                  {
                      values[i * 3] = ((UbloxDevice)devices[i]).GetValue(0);
                      values[i * 3 + 1] = ((UbloxDevice)devices[i]).GetValue(1);
                      values[i * 3 + 2] = ((UbloxDevice)devices[i]).GetValue(2);
                      Console.WriteLine(devices[i].SensorName + "  x:{0:0.00000} y:{1:0.000000} z:{2:0.000000}", values[i * 3], values[i * 3 + 1], values[i * 3 + 2]);
                  }

                  DateTime dt = DateTime.Now;
                  dt = GetYMDHM(dt);
                  this.dataStore.PutStoreData(
                      new Comm.DataStore.StoreData<double>(dt, values));

              }

              catch (Exception ex)
              {
                  Console.WriteLine(ex.Message + "," + ex.StackTrace);
              }
            //  throw new NotImplementedException();
          }

          public override void OnSSHDDataController_ReceiveText(TextPackage text)
          {
            //  throw new NotImplementedException();
          }
    }
}
