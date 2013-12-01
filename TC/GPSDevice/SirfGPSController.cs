using System;
using System.Collections.Generic;
using System.Text;
using Comm;
using RemoteInterface.SensorConfig;

namespace GPSDevice
{
    public class SirfGPSController: Comm.Controller.SSHDataController
    {
        public  Queue<GPSMessage.GPSData> refGpsDataQueue = new Queue<GPSMessage.GPSData>();
        public  long LastRefTimeStamp = -1;
        public GPSDevice refDevice;
     
        // GPSDevice[] devices;
         GPSControllerConfig Configuration;
         string configfile ;



         public SirfGPSController(GPSControllerConfig config, PropertyBag property)
             : base(config, property)
         { 
             
             try
             {
                
                 this.Configuration = config;
                 this.PropertyBag.TransmitCycle = 10;
                 this.PropertyBag.TransmitMode = 1; 
                 
             //    protocol.Parse(System.IO.File.ReadAllText("protocol.txt"), false);
                    
             
             
             }
             catch(Exception ex)
             {
                 Console.WriteLine("config.xml 讀取錯誤!,"+ex.Message);
                 Environment.Exit(-1);
             }

             try
             {
                 refDevice =(GPSDevice) CreateDevice(Configuration.ref_gps);



                 refDevice.ProcessCompletedEvent += new ProcessCompletedHandler(dev_ProcessCompletedEvent);
                 //devices = new GPSDevice[Configuration.gps_configs.Length];
                 //for (int i = 0; i < devices.Length; i++)
                 //{
                 //    try
                 //    {
                 //        devices[i] = (GPSDevice)CreateDevice(Configuration.gps_configs[i]);
                 //    }
                 //    catch { ;}
                 //}
             }
             catch (Exception Exception){
                 Console.WriteLine(Exception.Message+","+Exception.StackTrace);}
         }

       
          //GPSDevice[] devices;
          //Config Configuration;
          //string configfile = "config.xml";
        

       protected override  Comm.SensorBase CreateDevice(SensorConfigBase conf)
         {
             GPSDevice dev;
             GPSSensorConfig config = (GPSSensorConfig)conf;
             if (config.com_type == "TCP")
             {
                 if (config.is_reference)
                     return new GPSDevice(config.id,this,config.device_name, new System.Net.IPEndPoint(System.Net.IPAddress.Parse(config.ip_comport), config.port_baud), config.refx, config.refy, config.refz);
                 else
                     return new GPSDevice(config.id,this,config.device_name, new System.Net.IPEndPoint(System.Net.IPAddress.Parse(config.ip_comport), config.port_baud));
             }
             else  //COM
             {
                 if (config.is_reference)
                     return new GPSDevice(config.id,this,config.device_name, config.ip_comport, config.port_baud, config.refx, config.refy, config.refz);
                 else
                     return new GPSDevice(config.id,this, config.device_name, config.ip_comport, config.port_baud);
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

                     foreach (GPSDevice dev in devices)
                     {
                         if (dev != null && dev.IsReference == false)
                             dev.Signal();
                     }


                     if (refGpsDataQueue.Count > 10)
                         refGpsDataQueue.Dequeue();

                 }

             }

         }
      
          //public override void OnReceiveText(Comm.TextPackage txt)
          //{
             
          //}

          //protected override void OnPeriodDataReport()
          //{
          //   // throw new NotImplementedException();
          //}

          protected override void OnOneMinTmrTask()
          {
             

            //  throw new NotImplementedException();
          }

          public override void OnSSHDDataController_ReceiveText(TextPackage text)
          {
            //  throw new NotImplementedException();
          }
    }
}
