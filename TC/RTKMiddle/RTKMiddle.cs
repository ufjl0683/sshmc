using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RTKMiddle
{
  public   class RTKMiddle
    {
       EGPSServer server;
       public string RTK_IP;
       public int port;
     
       public RTKMiddle(string rtkip, int rtkport)
       {
           this.RTK_IP = rtkip;
           this.port = rtkport;

           RtkClient client = new RtkClient(rtkip, rtkport);
           client.OnData += new RtkEvent(client_OnData);
           server = new EGPSServer(rtkport+10000);
         //  TcpClient tcp = new TcpClient("127.0.0.1", 17000);
         //  SirfDLE device = new SirfDLE("aaa", tcp.GetStream());
         //  device.OnReceiveText += new OnTextPackageEventHandler(device_OnReceiveText);
        //   System.Threading.Thread.Sleep(10000);
        //   tcp.Close();
         //  tcp = new TcpClient("127.0.0.1", 17000);
         //  device.OnReceiveText += new OnTextPackageEventHandler(device_OnReceiveText);
       }

       void client_OnData(object sender, string data)
       {
           data = System.Text.RegularExpressions.Regex.Replace(data, "[ ]+", " ");
           string[] strs = data.Split(new char[] { ' ' });
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
           byte[] temp = System.BitConverter.GetBytes(x);
           foreach (byte b in temp)
               cks += b;
           ms.Write(temp, 0, temp.Length);
           temp = System.BitConverter.GetBytes(y);
           foreach (byte b in temp)
               cks += b;
           ms.Write(temp, 0, temp.Length);
           temp = System.BitConverter.GetBytes(z);
           foreach (byte b in temp)
               cks += b;
           ms.Write(temp, 0, temp.Length);

           cks &= 0x7fff;
           ms.WriteByte((byte)(cks / 256));
           ms.WriteByte((byte)(cks % 256));

           ms.WriteByte(0xb0);
           ms.WriteByte(0xb3);
           server.Send(ms.ToArray());
         //  Console.WriteLine(data);
           //throw new NotImplementedException();
       }
    }
}
