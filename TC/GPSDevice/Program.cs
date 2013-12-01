using System;
using System.Collections.Generic;

using System.Text;
using MatrixLibrary;
using RemoteInterface.SensorConfig;

namespace GPSDevice
{
    class Program
    {

       //public static Queue<GPSMessage.GPSData> refGpsDataQueue = new Queue<GPSMessage.GPSData>();
       //public  static long LastRefTimeStamp=-1;
       //static  GPSDevice refDevice;
    
       //static GPSDevice [] devices;
       //static Config Configuration;
       //static string configfile = "config.xml";

      //  public static GPSController controller=null;
       static void Main(string[] args)
        {

            new UbloxGPSController(GPSControllerConfig.Deserialize(AppDomain.CurrentDomain.BaseDirectory + "config.xml") as GPSControllerConfig, GetPropertyBag());
            
            //while (true)
            //{
            //    System.Console.ReadKey();
            //}
             // Matrix h=new Matrix( new double [,] {{1,2,3},{4,5,6},{7,8,8}});
              //Matrix ht = Matrix.Transpose(h);
              //Matrix hhtinv = Matrix.Inverse(ht * h);
              //Matrix res = hhtinv*ht;
              //Console.WriteLine(Matrix.PrintMat(res));
              //Console.WriteLine(Matrix.PrintMat(Matrix.PINV(h)));

            //try
            //{
            //    if (System.IO.File.Exists(configfile))
            //        Configuration = Config.Deserialize(configfile);
            //    else
            //        Config.Serialze(configfile, Configuration = new Config());
            //}
            //catch
            //{
            //    Console.WriteLine("config.xml 讀取錯誤!");
            //}

           //  double[] p = new double[] { -3026917, 4928973, 2678857 };
            // double[] llh = GPSDevice.xyz2llh(p);
             // B1 Com10 B2 Com8  B3 Com9
            //refDevice = CreateGPSDevice(Configuration.ref_gps);


            // refDevice.ProcessCompletedEvent += new ProcessCompletedHandler(dev_ProcessCompletedEvent);
            // devices = new GPSDevice[Configuration.gps_configs.Length];
            // for (int i = 0; i < devices.Length; i++)
            // {
            //     devices[i] = CreateGPSDevice(Configuration.gps_configs[i]);
            // }


             //device1 = new GPSDevice("B2", "COM8", 115200);

             //device2 = new GPSDevice("B3", "COM9", 115200);

             //device3 = new GPSDevice("xx", new System.Net.IPEndPoint(
             //   new System.Net.IPAddress(new byte[] { 192, 168, 0, 100 }), 6000));

             //devices = new GPSDevice[] {device1, device2, device3 };

          //   GPSDevice dev = new GPSDevice("GPS0", "COM4", 115200);


//  Position test
             //double[] pr = new double[] { 22596135.8877,	22696668.4231	,21171042.567	,22471889.7544,	21000714.3358,	23219522.7366 };
             //double[] init = new double[] { -3026878.4458,	4928841.5691,	2678997.1038,	4.9845 };
             //double[,] vect = new double[,]{
             //   {5084607.176,	15703651.5639,	20808864.1743},
             //   {-23717005.3495,	2510321.5187,	11690537.7945},
             //   {-11739500.4116,	11740842.6947,	20731702.142},
             //   {-12317831.4689,21761584.4304,-8953837.2659},
             //   {-19235172.6371,18242204.7349,1642203.0425},
             //   {4863420.983,25877372.9077,-3489332.1053}



             // };
             //double [] result=  dev.CalcPosition(pr, vect, init, 1e-3);
             // Console.ReadKey();

// KFLSA Test
             //double[] xhat = new double[] {
             //  -3.025193563088618e6,
             //  4.928783566082199e6,
             //  2.681062999728592e6,
             //  8.526422619508519e6,
             // -0.000000279828464e6,
             //  0.000000286319003e6,
             //  0.000000000012844e6,
             //  0.008417849878267e6,
             // -0.000000049175217e6,
             //  0.000000046972916e6,
             // -0.000000000921223e6,
             // -0.005830338353239e6 };
             //double[] yn = new double[] {
             //    -3.025193999998588e6 ,
             //    4.928784000000869e6,
             //    2.681063000000000e6,
             //    8.539606753262397e6,
             //    -0.000000037619083e6,
             //    0.000000030704653e6,
             //    -0.000000002380095e6,
             //    0.000000000065073e6
             //};

             //double[,] px = Matrix.ScalarMultiply(50, Matrix.Identity(12));
             //double[,] r = Matrix.ScalarMultiply(50, Matrix.Identity(8));
             //dev.KFLSA(ref xhat, yn,ref  px, r, 1);
            // Console.ReadKey();
        }
       static GPSPropertyBag GetPropertyBag()
       {
           GPSPropertyBag property;
           if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory+"PropertyBag.xml"))
           {
               System.IO.FileStream fs;
               System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(GPSPropertyBag));
               property = ser.Deserialize(fs = System.IO.File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "PropertyBag.xml")) as GPSPropertyBag;

               fs.Close();
               fs.Dispose();
           }
           else
           {
               property = new GPSPropertyBag();


           }

           property.SetHasLoaded();


           return property;

       }
       //static GPSDevice CreateGPSDevice(GPSConfig config)
       // {
       //     GPSDevice dev;

       //     if (config.com_type == "TCP")
       //     {
       //         if (config.is_reference)
       //             return new GPSDevice(config.device_name, new System.Net.IPEndPoint(System.Net.IPAddress.Parse(config.ip_comport), config.port_baud), config.refx, config.refy, config.refz);
       //         else
       //             return new GPSDevice(config.device_name, new System.Net.IPEndPoint(System.Net.IPAddress.Parse(config.ip_comport), config.port_baud));
       //     }
       //     else  //COM
       //     {
       //           if (config.is_reference)
       //              return new GPSDevice(config.device_name,config.ip_comport,config.port_baud,config.refx, config.refy, config.refz);
       //         else
       //               return new GPSDevice(config.device_name, config.ip_comport, config.port_baud);
       //     }
               
       // }
        
       //static void dev_ProcessCompletedEvent(GPSDevice sender, GPSMessage.GPSData data)
       // {
       //     lock (refGpsDataQueue)
       //     {
       //         refGpsDataQueue.Enqueue(data);
       //         LastRefTimeStamp = (long)data.TimeStamp;
       //         if (refGpsDataQueue.Count > 2)
       //         {
                    
       //             foreach (GPSDevice dev in devices)
       //             {
       //                 if (dev !=null && dev.IsReference == false)
       //                     dev.Signal();
       //             }
                   
                   
       //             if(refGpsDataQueue.Count >5)
       //             refGpsDataQueue.Dequeue();
                    
       //         }
            
       //     }

       // }

      
    }
}
