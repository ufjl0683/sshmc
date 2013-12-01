using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Comm;
using System.Net.Sockets;

namespace RTKMiddle
{
    class Program
    {
        static EGPSServer server;
     //   static  RTKMiddle middle;
        static void Main(string[] args)
        {
         
            StreamReader sr=new System.IO.StreamReader(System.IO.File.OpenRead(AppDomain.CurrentDomain.BaseDirectory+"config.txt"));
            string data;
            while ((data = sr.ReadLine()) != null)
            {
                string[] ipport = data.Split(new char[] { ',' });
                if (ipport.Length == 2)
                {
                    new RTKMiddle(ipport[0], System.Convert.ToInt32(ipport[1]));
                }
            }
         //   RtkClient client = new RtkClient("192.192.85.39", 7000);
         //   client.OnData += new RtkEvent(client_OnData);
         //   server = new EGPSServer(7000 + 10000);
            //TcpClient tcp = new TcpClient("127.0.0.1", 17000);
            //SirfDLE device = new SirfDLE("aaa", tcp.GetStream());
            //device.OnReceiveText += new OnTextPackageEventHandler(device_OnReceiveText);
            //System.Threading.Thread.Sleep(10000);
      
            //device.OnReceiveText += new OnTextPackageEventHandler(device_OnReceiveText);
          
        }

        static void device_OnReceiveText(object sender, TextPackage txtObj)
        {
            double x = System.BitConverter.ToDouble(txtObj.Text, 0);
            double y = System.BitConverter.ToDouble(txtObj.Text, 8);
           double z= System.BitConverter.ToDouble(txtObj.Text, 16);
           Console.WriteLine("{0} {1} {2}", x, y, z);
            //throw new NotImplementedException();
        }

        static void client_OnData(object sender, string data)
        {
          data=  System.Text.RegularExpressions.Regex.Replace(data,"[ ]+"," ");
            string[] strs = data.Split(new char[] {' ' });
            double x = System.Convert.ToDouble(strs[2]);
            double y = System.Convert.ToDouble(strs[3]);
            double z = System.Convert.ToDouble(strs[4]);
            int flag = System.Convert.ToInt32(strs[5]);
            int ratio = System.Convert.ToInt32(strs[6]);
            if (flag != 1)
                return;

            MemoryStream ms = new MemoryStream();
            ms.WriteByte((byte)0xa0);
            ms.WriteByte((byte)0xa2);
            int len = 24;
            ms.WriteByte(0);
            ms.WriteByte(24);

            int cks = 0;
             byte[] temp= System.BitConverter.GetBytes(x);
                 foreach(byte  b in temp)
                     cks+=b;
             ms.Write(temp, 0, temp.Length);
            temp=System.BitConverter.GetBytes(y);
            foreach (byte b in temp)
                cks += b;
            ms.Write(temp, 0, temp.Length);
            temp = System.BitConverter.GetBytes(z);
            foreach (byte b in temp)
                cks += b;
            ms.Write(temp, 0, temp.Length);

            cks &= 0x7fff;
            ms.WriteByte((byte)(cks/256));
            ms.WriteByte((byte)(cks % 256));
         
            ms.WriteByte(0xb0);
            ms.WriteByte(0xb3);
            server.Send(ms.ToArray());
            Console.WriteLine(data);
            //throw new NotImplementedException();
        }
    }
}
