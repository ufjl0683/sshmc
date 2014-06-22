using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comm;
using System.Xml.Serialization;
using RemoteInterface.SensorConfig;
using GPSDevice.GPSMessage;
using GPSDevice;
using Comm.Controller;
using OpenPop.Mime;
using System.Net.Mail;
using System.Net;
namespace ClientTest
{
    [XmlInclude(typeof(TiltSensorConfig)), XmlInclude(typeof(GPSSensorConfig))]
    public class Config
    {

       
       
        
      
        //public Comm.SensorConfig.SensorConfigBase sensor;
     
        //public void Serialze(string pathfile)
        //{
        //    System.IO.FileStream fs = System.IO.File.Create(pathfile);
        //    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Config));
        //    x.Serialize(fs, this);
        //    fs.Close();
        //    fs.Dispose();

        //}
        //public Config Deserialize(string pathfile)
        //{
        //    System.IO.FileStream fs = System.IO.File.OpenRead(pathfile);
        //    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Config));
        //    return x.Deserialize(fs) as Config;
        //}
    }
    class Program
    {

        static double[,] Matrix;
     
        static void Main(string[] args)
        {
           // SendMailTest();
            //System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
            //client.Connect("192.168.2.100", 7002);

            //System.IO.Stream stream = client.GetStream();
            //int d;
            //while ((d = stream.ReadByte()) != -1)
            //{
            //    Console.Write(d);
            //}

           HostSetSntValteTest();

//           RTKMiddle.RtkClient client = new RTKMiddle.RtkClient("192.168.2.100", 7002);

//           client.OnData += client_OnData;

//            Console.ReadKey();
//            SSHMC01Entities db = new SSHMC01Entities();
//            var q = from n in db.tblUser  where n.USER_ID=="david" && n.USER_PW=="0988163835" select n;
//            tblUser[] users = q.ToArray();
//            tblUser user = (from n in db.tblUser where n.USER_ID == "david" && n.USER_PW == "0988163835" select n).FirstOrDefault();
//            Console.WriteLine(user.USER_ID);
//            foreach (tblUser u in users)
//            {
//                Console.WriteLine(u.USER_ID);
//            }
//          //  System.Net.Sockets.TcpListener sv = new System.Net.Sockets.TcpListener(
//          //new System.Net.IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 1101));
//          //  sv.Start();
//          // System.Net.Sockets.Socket soc= sv.AcceptSocket();

//          //  byte[] data=new byte[1024];;
//          //  int cnt;
//          //  while ((cnt = soc.Receive(data)) != -1)
//          //  {
//          //      Console.WriteLine(System.Text.UTF8Encoding.UTF8.GetString(data,0,cnt));
//          //  }
//            DataService.SSHMCDataServiceClient c= new DataService.SSHMCDataServiceClient();

//            c.CheckUserIDPassword("david", "0988163835");

//            c.AddSurverDiasterInfo(new DataService.tblSurvey_Disaster() { 
//             BUILD_TIME = DateTime.UtcNow,
//             BUILD_USER = "User",
//             CASUALTIES_0 = 0,
//             CASUALTIES_1 = 0,
//             CASUALTIES_2 = 0,
//             CASUALTIES_3 ="none",
//             DAMAGED_BRIDGE ="none",
//             DAMAGED_BRIDGE_0 = 0,
//             DAMAGED_ROADS = "none",
//             DAMAGED_ROADS_0 = 0,
//              EVACUATION_0 = true,
//              EVACUATION_1 = 0,
//             ISCHECK=true,
//             ISCLOSE =false,
//             OCCUR_TIME = DateTime.UtcNow,
//             PHOTO_PATH="Update",
//             PLACE_NAME="none",
//             SITUATION_0 = 0,
//             SITUATION_1 =0,
//             SITUATION_2 = 0,
//             TYPE_0 = true,
//             TYPE_1=false,
//             TYPE_2=false,
//             TYPE_3=false,
//             TYPE_4=false,
//             TYPE_5=false,
//             TYPE_6=false,
//             USER__POSITION_X = 121.5555,
//             USER__POSITION_Y = 24.9999,
//             X = 121.5555,
//             Y =24.9999
             
            
//            });


//            //foreach(DataService.tblSurvey_Disaster d in  c.GetSurveyDisaster())
//            //{
//            //    Console.WriteLine(d.BUILD_TIME+","+d.SITE_ID+","+d.USER__POSITION_X+","+d.USER__POSITION_Y+","+d.PHOTO_PATH);
//            //}
//         //  Mail();
//            c.Close();
//            Console.ReadKey();
//            //Microsoft.JScript.Vsa.VsaEngine eng = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
//            //object res = Microsoft.JScript.Eval.JScriptEvaluate("var a=10;a+2*6+5;Math.sin(0);", eng);
//            //Console.WriteLine(res.ToString());
//            //string fmla = "{0}+-1.00672e-5*{2}*{2}*{2}+0.00063197*{2}*{2}-0.01225*{2}+1.050715+-0.98048";
//            //  res= Microsoft.JScript.Eval.JScriptEvaluate(string.Format(fmla,1,0,32), eng);
//            //  Console.WriteLine(res.ToString());
//            // Console.ReadKey();


//            //SSHMC01Entities db = new SSHMC01Entities();
//            //var q = from n in db.tblUser select n;
//            //foreach (tblUser user in q)
//            //{
//            //    Console.WriteLine(user.USER_NAME);
//            //}
//           // DateTime dt = new DateTime(2012, 0, 31);
//           //GPSControllerConfig config = new GPSControllerConfig()
//           // {
//           //     build_date = DateTime.Now,
//           //     device_type = "GPS",
//           //     controller_id = 100,
//           //     listen_port = 1001,
//           //     ref_gps = new GPSSensorConfig() { com_type="COM", device_name="GPS0", execution_mode=0, id=0, ip_comport="COM8", is_reference=false, port_baud=115200  },
               
             

//           // };

//           // System.Xml.Serialization.XmlSerializer sr = new XmlSerializer(typeof(ControllerConfigBase));

//           // System.IO.Stream stream=System.IO.File.OpenWrite("c:\\config.xml");
//           // sr.Serialize(stream, config);
//           // stream.Close();

//            //Console.WriteLine(System.BitConverter.ToInt16(new byte[] { (byte)0x55, (byte)0xfd }, 0)*Math.Pow(2,-38)*Math.PI);
           
//          //  UBLOX_Test();
//           //ProcessManagerService.ProcessManager mgr=  new ProcessManagerService.ProcessManager();

//           // GetTileConfig();
//        //    TILECOm();
//          //  Console.WriteLine(ThreeBytesToInt(new byte[] { 0x80, 0x00, 0x00 }));
//          //  QRTest();
////GetPeriodData();

         
            Console.ReadKey();
           // mgr.KillAll();
        }
        public static void SendMailToUser(string mailaddress, string subject, string bodytext)
        {
            WebClient client = new WebClient();
            string url = "http://localhost:8080/WeatherMailService/SendMailToUser?address={0}&subject={1}&bodytext={2}";
           string res= new System.IO.StreamReader(client.OpenRead( string.Format(url, mailaddress, subject, bodytext))).ReadToEnd();
           Console.WriteLine(res);
        }
        static void SendMailTest()
        {
            SendMailToUser("ufjl0683@emome.net", "升級", "升級測試systest");
        }
        static void client_OnData(object sender, string data)
        {
            //throw new NotImplementedException();
        }
        static void HostSetSntValteTest()
        {

            RemoteInterface.HC.I_HC_Comm r_host_comm =(RemoteInterface.HC.I_HC_Comm) 
                RemoteInterface.RemoteBuilder.GetRemoteObj(typeof(RemoteInterface.HC.I_HC_Comm),
                RemoteInterface.RemoteBuilder.getRemoteUri("192.192.161.5", 9010, "Comm"));

            r_host_comm.SetSensorValueDegree(35, 0, 0, 0, 1);
            Console.WriteLine("id:35  to degree 1");
            Console.ReadKey();
            r_host_comm.SetSensorValueDegree(35, 0, 0, 0, 0);
            Console.WriteLine("id:35  to degree 0");
            Console.ReadKey();

            r_host_comm.SetSensorValueDegree(35, 0, 0, 0,1);
            Console.WriteLine("id:35  to degree 1");
            Console.ReadKey();
            r_host_comm.SetSensorValueDegree(35, 0, 0, 0,2);
            Console.WriteLine("id:35  to degree 2");
            Console.ReadKey();
            r_host_comm.SetSensorValueDegree(35, 0, 0, 0, 1);
            Console.WriteLine("id:35  to degree 1");
            Console.ReadKey();
            r_host_comm.SetSensorValueDegree(35, 0, 0, 0,0);
            Console.WriteLine("id:35  to degree 0");
            Console.ReadKey();
        }
        static void Mail()
        {
            string s="致 貴客戶，依中央氣象局發布資料，提醒您以下防災預警資訊:\n";
            s += " 最新氣象消息請參考 www.cwb.gov.tw 中央氣象局地震測報中心　第086號有感地震報告 發　震　時　間： 102年 7月 6日12時17分11.1秒 震　央　位　置： 北　緯　 22.85 ° 東 經　121.32 ° 震　源　深　度：　 22.4 公里 芮　氏　規　模：　 4.0 相 對 位 置： 臺東縣政府東偏北方 20.4 公里 (位於臺灣東部海域) 各 地 震 度 級 　　　　　　　　　　　　　　　臺東縣地區最大震度 3級 　　　　　　　　　　　　　　　　　　東　河 3 　 　　　　　　　　　　　　　　　　　綠　島 3 　　　　　　　　　　　　　　　　　　成　功 3 　　　　　　　　　　　　　　　　　　臺東市 2 　　　　　　　　　　　　　　　　　　池　上 2 　　　　　　　　　　　　　　　　　　初　鹿 1 　　　　　　　　　　　　　　　　　　卑　南 1 　　　　　　　　　　　　　　　　　　利　稻 1";
            string r= System.Text.RegularExpressions.Regex.Replace(s, "[\u3000]+", " ");
            Console.WriteLine(r );
        }
        static void Pop3Test()
        {
            OpenPop.Pop3.Pop3Client mclient = new OpenPop.Pop3.Pop3Client();
            mclient.Connect("mail.cute.edu.tw", 110, false);
            mclient.Authenticate("weather", "0988163835");
            int cnt = mclient.GetMessageCount();
            Console.WriteLine("Message cnt:" + cnt);
            string bodytext;
            for (int i = 1; i <= cnt; i++)
            {
                bodytext = "";
                Message msg = mclient.GetMessage(i);

                Console.WriteLine("from:" + msg.Headers.From.MailAddress.Address);
                Console.WriteLine("subject:" + msg.Headers.Subject);
                if (!msg.MessagePart.IsText)
                {
                    MessagePart msgpart = msg.FindFirstPlainTextVersion();
                    if (msgpart != null && msgpart.IsText)
                    {
                        Console.WriteLine("body:" + msgpart.GetBodyAsText());

                        bodytext = msgpart.GetBodyAsText();
                    }

                }
                else
                {
                    Console.WriteLine("body:" + msg.MessagePart.GetBodyAsText());
                    bodytext = msg.MessagePart.GetBodyAsText();
                }

                if (msg.Headers.Subject.Contains("大雨") || msg.Headers.Subject.Contains("強風") || msg.Headers.Subject.Contains("地震"))
                {
                    System.Net.Mail.SmtpClient c = new SmtpClient("mail.cute.edu.tw", 25);
                    c.DeliveryMethod = SmtpDeliveryMethod.Network;
                    c.Credentials = new System.Net.NetworkCredential("weather", "0988163835");
                    MailMessage m_mesg = new MailMessage(new MailAddress("weather@cute.edu.tw"), new MailAddress("ufjl0683@cute.edu.tw"));
                    m_mesg.Body = bodytext;
                    m_mesg.Subject = msg.Headers.Subject.Replace("\n", "");

                    m_mesg.IsBodyHtml = true;
                    if (bodytext != "")
                        c.Send(m_mesg);
                }


            }
            for (int i = 1; i <= cnt; i++)
                mclient.DeleteMessage(i);

            mclient.Dispose();
        }
        static void  QRTest()
        {
            double[,] matrix = new double[,]{
          {-0.564177933822972,0.090748544614381,0.722694075284377},
{-0.936164069720717,0.649736755363942,-0.519251542684745},
{0.449775027671702,0.443161778031498,-0.200459331990490},
{0.516781028448624,1.206380682676600,-0.244424283250938}
  
            };
            //double[,] matrix = new double[,]{
            //        {1,2,3},
            //        {4,5,6},
            //        {7,8,9},
            //        {10,11,12}
            //};
            double[,]q,r;
            MatrixLibrary.Matrix.QR(matrix,out q,out r);

            Console.WriteLine(MatrixLibrary.Matrix.PrintMat(matrix));
            Console.WriteLine("Q");
           Console.WriteLine( MatrixLibrary.Matrix.PrintMat(q));
           Console.WriteLine("R");
           Console.WriteLine( MatrixLibrary.Matrix.PrintMat(r));
        }
        static void mat(double[,]d)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    d[i, j] = 1;
        }
        static int ThreeBytesToInt(byte[] data)
        {
            byte[] convertData=new byte[4];
            System.Array.Copy(data,convertData,3);
            if ((data[2] & 0x80) == 0)
                convertData[3] = 0;
            else
                convertData[3] = 0xff;


            return System.BitConverter.ToInt32(convertData,0);

        }
        static void TILECOm()
        {
            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
            client.Connect("192.168.168.126", 6000);
       
            while (true)
            {
                int b = client.GetStream().ReadByte();
                System.Console.Write(string.Format("{0:x2} ",b));
            }

        }
        static void UBLOX_Test()
        {
            System.IO.Ports.SerialPort com= new System.IO.Ports.SerialPort("COM8",115200);
            com.Open();
            SirfDLE dev = new SirfDLE("UBLOX", com.BaseStream);
            dev.OnReceiveText += new OnTextPackageEventHandler(dev_OnReceiveText);
            

           // com.Close();

        }

        static void GetTileConfig()
        {
            ControllerConfigBase config;
            System.Xml.Serialization.XmlSerializer ser = new XmlSerializer(typeof(ControllerConfigBase));

          //  System.IO.MemoryStream ms = new System.IO.MemoryStream();

           // ser.Serialize(ms, config);

            byte[] data = new byte[] { 0x04, 0x20 };


            Comm.SendPackage pkg = new SendPackage(CmdType.CmdQuery, CmdClass.A, 0xffff, data);

            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
            client.Connect("127.0.0.1", 1002);

            DeviceV2DLE dle = new DeviceV2DLE("tile", client.GetStream());
            dle.Send(pkg);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(pkg.ReturnTextPackage.Text, 10, pkg.ReturnTextPackage.Text.Length - 10);
           // string str = System.Text.UTF8Encoding.UTF8.GetString(ms.GetBuffer());
           // Console.WriteLine(str);
             config = ser.Deserialize(ms) as ControllerConfigBase;
            Console.WriteLine(config.build_date+"finish!");
        }
        //static void TiltConfig()
        //{
        //    ControllerConfigBase config = new ControllerConfigBase()
        //    {
        //        build_date = DateTime.Now,
        //        controller_id = 0x0000,
        //        device_type = "Tilt",
        //        listen_port = 1002,
        //        version_no = 1 ,
        //        sensors = new TiltSensorConfig[]
        //        {
        //          new TiltSensorConfig()
        //          {
        //               com_type="COM",
        //                device_name="TILE1",
        //                 id=0,
        //                  execution_mode=0,
        //                   port_baud=115200,
        //                    ip_comport="COM7",
        //                     sensor_values=new SensorValueConfigBase[]{
        //                      new SensorValueConfigBase()
        //                      {
        //                           coefficient=1,
        //                            offset=0,
        //                             desc="X",
        //                              id=0,
        //                               rules=new SensorValueRuleConfigBase[]
        //                               {
        //                                  new SensorValueRuleConfigBase()
        //                                  {
        //                                       level=1,
        //                                        lower_limit=0,
        //                                         upper_limit=0,
        //                                          hour_ma=0,
        //                                           left_hour_ma1=0,
        //                                            left_hour_ma2 =0,
        //                                             right_hour_ma1=0,
        //                                              right_hour_ma2=0
                                                 
        //                                  },
        //                                   new SensorValueRuleConfigBase()
        //                                  {
        //                                       level=2,
        //                                        lower_limit=0,
        //                                         upper_limit=0,
        //                                          hour_ma=0,
        //                                           left_hour_ma1=0,
        //                                            left_hour_ma2 =0,
        //                                             right_hour_ma1=0,
        //                                              right_hour_ma2=0
        //                                  },
                                                 
                                          
        //                                                new SensorValueRuleConfigBase()
        //                                  {
        //                                       level=3,
        //                                        lower_limit=0,
        //                                         upper_limit=0,
        //                                          hour_ma=0,
        //                                           left_hour_ma1=0,
        //                                            left_hour_ma2 =0,
        //                                             right_hour_ma1=0,
        //                                              right_hour_ma2=0
                                                 
        //                                  }
        //                               }
        //                      },
        //                       new SensorValueConfigBase()
        //                      {
        //                           coefficient=1,
        //                            offset=0,
        //                             desc="Y",
        //                              id=1,
        //                               rules=new SensorValueRuleConfigBase[]
        //                               {
        //                                  new SensorValueRuleConfigBase()
        //                                  {
        //                                       level=1,
        //                                        lower_limit=0,
        //                                         upper_limit=0,
        //                                          hour_ma=0,
        //                                           left_hour_ma1=0,
        //                                            left_hour_ma2 =0,
        //                                             right_hour_ma1=0,
        //                                              right_hour_ma2=0
                                                 
        //                                  },
        //                                   new SensorValueRuleConfigBase()
        //                                  {
        //                                       level=2,
        //                                        lower_limit=0,
        //                                         upper_limit=0,
        //                                          hour_ma=0,
        //                                           left_hour_ma1=0,
        //                                            left_hour_ma2 =0,
        //                                             right_hour_ma1=0,
        //                                              right_hour_ma2=0
        //                                  },
                                                 
                                          
        //                                                new SensorValueRuleConfigBase()
        //                                  {
        //                                       level=3,
        //                                        lower_limit=0,
        //                                         upper_limit=0,
        //                                          hour_ma=0,
        //                                           left_hour_ma1=0,
        //                                            left_hour_ma2 =0,
        //                                             right_hour_ma1=0,
        //                                              right_hour_ma2=0
                                                 
        //                                  }
        //                               }
        //                      }


        //                     }

        //          }
        //        }



        //    };

        //    System.Xml.Serialization.XmlSerializer ser = new XmlSerializer(typeof(ControllerConfigBase));
           
        //    System.IO.MemoryStream ms=new System.IO.MemoryStream();
            
        //    ser.Serialize(ms, config);
        //    byte[] data =new byte[ms.Length+3];
        //    data[0]=0x20;
        //    data[1]=(byte)(ms.Length/256);
        //    data[2]=(byte)(ms.Length%256);
        //    System.Array.Copy(ms.GetBuffer(),0,data,3,ms.Length);


        //    Comm.SendPackage pkg = new SendPackage(CmdType.CmdSet, CmdClass.A,0xffff, data);

        //   System.Net.Sockets.TcpClient client=new System.Net.Sockets.TcpClient();
        //    client.Connect("127.0.0.1",1002);

        //    DeviceV2DLE dle = new DeviceV2DLE("tile", client.GetStream());
        //    dle.Send(pkg);


        //}
       
        static void GetPeriodData()
        {
            Protocol protocol = new Protocol();
            protocol.Parse(System.IO.File.ReadAllText("PILT.txt"), false);
            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
            client.Connect("127.0.0.1", 1001);
            DeviceV2DLE dev = new DeviceV2DLE("PILT", client.GetStream());
            dev.OnReceiveText += new OnTextPackageEventHandler(dev_OnReceiveText);
            //while (true)
            //{
            for (byte hour = 0; hour < 23; hour++)
            {
                for (byte min = 0; min < 60; min++)
                {
                    SendPackage pkg = new SendPackage(CmdType.CmdQuery, CmdClass.A, 0xffff,
                    new byte[] { 0x28, 12, hour, min });
                    dev.Send(pkg);

                    if (pkg.ReturnTextPackage != null)
                    {
                        System.Data.DataSet ds = protocol.GetReturnDsByTextPackage(pkg.ReturnTextPackage);
                        if (Convert.ToInt32(ds.Tables[0].Rows[0]["sensor_cnt"]) == 0)
                        {
                            Console.WriteLine("Not Found");
                        }
                        else
                            Console.WriteLine(V2DLE.ToHexString(pkg.ReturnTextPackage.Text));
                    }

                }


            }

        }
        static void get_pilt_level_degree()
        {
            Protocol protocol = new Protocol();
            protocol.Parse(System.IO.File.ReadAllText("PILT.txt"), false);
            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
            client.Connect("127.0.0.1", 1002);
            DeviceV2DLE dev = new DeviceV2DLE("PILT", client.GetStream());
            dev.OnReceiveText += new OnTextPackageEventHandler(dev_OnReceiveText);
            //while (true)
            //{
                SendPackage pkg = new SendPackage(CmdType.CmdQuery, CmdClass.A, 0xffff,
                    new byte[] { 0x04, 0x21 });
                dev.Send(pkg);

                Console.WriteLine(V2DLE.ToHexString(pkg.ReturnTextPackage.Text));
            //}
        }

        static void dev_OnReceiveText(object sender, TextPackage txtObj)
        {

            try
            {
                if (txtObj.Text[0] == 0x02 && txtObj.Text[1] == 0x30 || txtObj.Text[0] == 0x02 && txtObj.Text[1] == 0x31)
                {

                   // Console.WriteLine("[" + string.Format("{0:X2}", txtObj.Text[0]) + " " + string.Format("{0:X2}", txtObj.Text[1]) + "],"+txtObj.Text.Length);
                 //   Console.WriteLine(V2DLE.ToHexString(txtObj.Text));

                    UBIDBase idbase = new GPSDevice.GPSMessage.UBIDBase(txtObj.Text);
                    Console.WriteLine(idbase.ToString());
                    if (Matrix != null)
                    {
                        int svid =(int) idbase[1];

                        if (idbase.GetMessageID() == 0x0230)  //alm
                        {
                            //for (int i = 0; i < 8; i++)
                            //    Matrix[svid - 1, i] = (uint)idbase[3 + i];
                            Matrix[svid - 1, 0] = idbase.alm_Eccentricity;
                            Matrix[svid - 1, 1] = idbase.alm_toa;
                            Matrix[svid - 1, 2] = idbase.alm_delti;
                            Matrix[svid - 1, 3] = idbase.alm_omegadot;
                            Matrix[svid - 1, 4] = idbase.alm_sqrtA;
                            Matrix[svid - 1, 5] = idbase.alm_omega0;
                            Matrix[svid - 1, 6] = idbase.alm_w;
                            Matrix[svid - 1, 7] = idbase.alm_mean0;
                            Matrix[svid - 1, 8] = idbase.alm_af0;
                            Matrix[svid - 1, 9] = idbase.alm_af1;
                            Console.WriteLine("Eccentricity=" + idbase.alm_Eccentricity);
                            Console.WriteLine("toa=" + idbase.alm_toa);
                            Console.WriteLine("delti=" + idbase.alm_delti);
                            Console.WriteLine("omegadot=" + idbase.alm_omegadot);
                            Console.WriteLine("sqrtA=" + idbase.alm_sqrtA);
                            Console.WriteLine("omega0=" + idbase.alm_omega0);
                            Console.WriteLine("w=" + idbase.alm_w);
                            Console.WriteLine("mean0=" + idbase.alm_mean0);
                            Console.WriteLine("af0=" + idbase.alm_af0);
                            Console.WriteLine("af1=" + idbase.alm_af1);
                          
                            Console.WriteLine("=============================");
                        }

                        if (idbase.GetMessageID() == 0x0231)  //eph
                        {
                            //for (int i = 0; i < 24; i++)
                            //{
                                // fill eph here
                                //Matrix[svid - 1, 8 + i] = (uint)idbase[3 + i];
                                Matrix[svid-1,10]=idbase.eph_toc;
                                Matrix[svid - 1, 11] = idbase.eph_af2;
                                Matrix[svid - 1, 12] = idbase.eph_af1;
                                Matrix[svid - 1, 13] = idbase.eph_af0;
                                Matrix[svid - 1, 14] = idbase.eph_crs;
                                Matrix[svid - 1, 15] = idbase.eph_deltan;
                                Matrix[svid - 1, 16] = idbase.eph_m0;
                                Matrix[svid - 1, 17] = idbase.eph_cuc;
                                Matrix[svid - 1, 18] = idbase.eph_e;
                                Matrix[svid - 1, 19] = idbase.eph_cus;
                                Matrix[svid - 1, 20] = idbase.eph_sqrtA;
                                Matrix[svid - 1, 21] = idbase.eph_toe;
                                Matrix[svid - 1, 22] = idbase.eph_cic;
                                Matrix[svid - 1, 23] = idbase.eph_w0;
                                Matrix[svid - 1, 24] = idbase.eph_cis;
                                Matrix[svid - 1, 25] = idbase.eph_i0;
                                Matrix[svid - 1, 26] = idbase.eph_crc;
                                Matrix[svid - 1, 27] = idbase.eph_w;
                                Matrix[svid - 1, 28] = idbase.eph_wdot;
                                Matrix[svid - 1, 29] = idbase.eph_idot;
                                Console.WriteLine("eph_toc="+ idbase.eph_toc);
                                Console.WriteLine("eph_af2=" + idbase.eph_af2);
                                Console.WriteLine("eph_af1=" + idbase.eph_af1);
                                Console.WriteLine("eph_af0=" + idbase.eph_af0);
                                Console.WriteLine("eph_crs=" + idbase.eph_crs);
                                Console.WriteLine("eph_deltan=" + idbase.eph_deltan);
                                Console.WriteLine("eph_m0=" + idbase.eph_m0);
                                Console.WriteLine("eph_cuc=" + idbase.eph_cuc);
                                Console.WriteLine("eph_e=" + idbase.eph_e);
                                Console.WriteLine("eph_cus=" + idbase.eph_cus);
                                Console.WriteLine("eph_sqrtA=" + idbase.eph_sqrtA);
                                Console.WriteLine("eph_toe=" + idbase.eph_toe);
                                Console.WriteLine("eph_cic=" + idbase.eph_cic);
                                Console.WriteLine("eph_w0=" + idbase.eph_w0);
                                Console.WriteLine("eph_cis=" + idbase.eph_cis);
                                Console.WriteLine("eph_i0=" + idbase.eph_i0);
                                Console.WriteLine("eph_crc=" + idbase.eph_crc);
                                Console.WriteLine("eph_w=" + idbase.eph_w);
                                Console.WriteLine("eph_wdot=" + idbase.eph_wdot);
                                Console.WriteLine("eph_idot=" + idbase.eph_idot);
                                double[] xyz = idbase.eph_xyz(0);
                                Console.WriteLine("x:{0} y:{1} z:{2}", xyz[0], xyz[1], xyz[2]);




                                Console.WriteLine("=============================");
                                if (!idbase.IsValid)
                                    for (int j = 0; j < 10; j++)
                                        Matrix[svid - 1, j] = 0;
                            //}

                            if (svid == 32) // print matrix
                            {

                                string str = "[\r\n";
                                for (int row = 0; row < 32; row++)
                                {
                                   
                                    for (int col = 0; col < 30 ; col++)  // for alm only
                                       str+=Matrix[row, col] + ",";

                                    str = str.TrimEnd(",".ToCharArray())+";" + "\r\n"; 
                                    
                                }
                                str = str.TrimEnd(",".ToCharArray()) + "]\r\n";

                                Console.Write(str);
                                System.IO.File.AppendAllText("log.txt", str);
                                //Write File Here
                              
                                Console.WriteLine();
                              
                            }

                        }

                        
                    }

                    if (idbase.GetMessageID() == 0x0231 && (int)idbase[1] == 32)
                        Matrix = new double[32, 30];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "," + ex.StackTrace);
            }
            //throw new NotImplementedException();
        }

       

        static void set_pilt_level_degree()
        {
            Protocol protocol = new Protocol();
            protocol.Parse(System.IO.File.ReadAllText("PILT.txt"),false);
            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
            client.Connect("127.0.0.1", 1002);
            DeviceV2DLE dev = new DeviceV2DLE("PILT", client.GetStream());
           System.Data.DataSet ds= protocol.GetSendDataSet("set_level_degree");
           ds.Tables[0].Rows[0]["sensor_cnt"] = 1;

           System.Data.DataRow row = ds.Tables[1].NewRow();
           row["id"] = 0;
           row["init_value1"] = V2DLE.DoubleTouLong(1.1111);
           row["value1_factor"] = V2DLE.DoubleTouLong(1.1);
           row["value1_offset"] = V2DLE.DoubleTouLong(0.1);
           row["value1_level1"] =V2DLE.DoubleTouLong(1.1);
           row["value1_level2"] = V2DLE.DoubleTouLong(1.2);
           row["value1_level3"] = V2DLE.DoubleTouLong(1.3);

           row["init_value2"] = V2DLE.DoubleTouLong(2.2222);
           row["value2_factor"] =V2DLE.DoubleTouLong( 2.1);
           row["value2_offset"] =V2DLE.DoubleTouLong( 0.2);
           row["value2_level1"] = V2DLE.DoubleTouLong(2.1);
           row["value2_level2"] = V2DLE.DoubleTouLong(2.2);
           row["value2_level3"] =V2DLE.DoubleTouLong( 2.3);
           ds.Tables[1].Rows.Add(row);
           ds.AcceptChanges();
           SendPackage pkg= protocol.GetSendPackage(ds, 0xffff);
           dev.Send(pkg);
        }

        
        static void get_Pilt_Param()
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
            client.Connect("127.0.0.1", 1002);
            DeviceV2DLE dev = new DeviceV2DLE("PILT", client.GetStream());
           // dev.OnReceiveText += new OnTextPackageEventHandler(dev_OnReceiveText);
            byte[] data = new byte[] { 0x04, 0x20 };
            SendPackage pkg = new SendPackage(CmdType.CmdQuery, CmdClass.A, 0xffff, data);

            dev.Send(pkg);
            Console.WriteLine(Comm.V2DLE.ToHexString(pkg.ReturnTextPackage.Text));
        }

        //static void dev_OnReceiveText(object sender, TextPackage txtObj)
        //{
        //    //throw new NotImplementedException();
        //}

        static void Set_Pilt_Param()
        {
            System.IO.MemoryStream ms=new System.IO.MemoryStream();
           System.Net.Sockets.TcpClient client=new System.Net.Sockets.TcpClient();
            client.Connect("127.0.0.1",1002);

            DeviceV2DLE dev = new DeviceV2DLE("PILT", client.GetStream());
            uint port_baud=115200;
            byte[] data ;
            ms.WriteByte(0x20);
            ms.WriteByte(1);
            ms.WriteByte(1); //id
            ms.WriteByte(0);  //tcp/com
            ms.WriteByte(4);
            ms.WriteByte(0);
             ms.WriteByte(0);
             ms.WriteByte(0);
             byte[] bdata = System.BitConverter.GetBytes(port_baud);
             for(int i=bdata.Length-1;i>=0;i--)
                 ms.WriteByte(bdata[i]);
             data = ms.ToArray();
            dev.Send(new SendPackage( CmdType.CmdSet, CmdClass.A,0xffff,data));

           // System.Data.DataSet ds;
            //byte[] data = new byte[]
            //{ 0x01,1,0,4,0,0,0
        
        }
        static void Tc_Test()
        {
            Comm.V2DLE dle;
            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
            client.Connect("192.192.85.40", 1001);
            dle = new Comm.V2DLE("test", client.GetStream());
            dle.OnReceiveText += new Comm.OnTextPackageEventHandler(dev_OnReceiveText);
            dle.OnReport += new Comm.OnTextPackageEventHandler(dle_OnReport);
            //  dle.Send(new Comm.SendPackage(Comm.CmdType.CmdSet, Comm.CmdClass.A,0xffff,new byte[]{0x02,11,10,1}));
            Comm.SendPackage pkg;
            while (true)
            {
                try
                {
                    Console.ReadKey();

                    byte[] data = new byte[]
                    {
                      // 03, 1,60,1,10
                      0x0b
                    };
                    pkg = new Comm.SendPackage(Comm.CmdType.CmdQuery, Comm.CmdClass.A, 0xffff, data);
                    dle.Send(pkg);
                    Console.WriteLine(Comm.V2DLE.ToHexString(pkg.ReturnTextPackage.Text));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ex.StackTrace);
                }

            }
        }

        static void dle_OnReport(object sender, Comm.TextPackage txtObj)
        {
            //throw new NotImplementedException();
            Console.WriteLine(Comm.V2DLE.ToHexString(txtObj.Text));
        }

       
    }
}
