using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace RTKMiddle
{
      public  class EGPSServer
    {

        
          int port;
          System.Net.Sockets.TcpListener listner;
          TcpClient client;
          public EGPSServer( int port)
          {
            
              this.port = port;

              listner = new System.Net.Sockets.TcpListener(port);
              new System.Threading.Thread(AcceptTask).Start();
          }

          void AcceptTask()
          {
             
              while (true)
              {
                  if (client == null || !client.Connected)
                  {
                      listner.Start();
                      client = listner.AcceptTcpClient();
                  }

                  System.Threading.Thread.Sleep(1000);

              }
          }


          public void Send(byte[] data)
          {
              if (client==null || !client.Connected)
                  return;
              lock (client.GetStream())
              {
                  client.GetStream().Write(data, 0, data.Length);
                  client.GetStream().Flush();
              }

          }

    }
}
