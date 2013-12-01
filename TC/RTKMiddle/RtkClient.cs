using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RTKMiddle
{
    public delegate void RtkEvent(object sender,string data);
   public  class RtkClient
    {
       System.Net.Sockets.TcpClient client;
       public event RtkEvent OnData;
       
       string ip;
       int port;

       System.Threading.Timer tmr  ;
       public RtkClient(string ip,int port)
       {
           this.ip = ip;
           this.port = port;
           new System.Threading.Thread(ConnectionTask).Start();
           tmr = new System.Threading.Timer(TimerCallBack);
           tmr.Change(5000, 0);
       }

       void TimerCallBack(object sender)
       {

           try
           {
              // Console.WriteLine("tick!");
               if (client!=null && client.Connected   && !IsInConnectionTask &&  DateTime.Now.Subtract(LastReceiveTime) >= TimeSpan.FromSeconds(8))
               {
                   if(client!=null)
                   
                     client.Close();
                    
               }
           }
           catch
           { ;}
           finally
           {

               tmr.Change(5000, 0);
           }

       }
       bool IsInConnectionTask=false;
       DateTime LastReceiveTime = DateTime.Now;
       void ConnectionTask()
       {
           try
           {
               IsInConnectionTask=true;
               while (true)
               {
                   client = new System.Net.Sockets.TcpClient();
                   try
                   {
                       Console.WriteLine("try to  connect " + ip + ":" + port + " !");
                       client.Connect(ip, port);
                       Console.WriteLine(ip + ":" + port + "connected! ");
                   }
                   catch (Exception ex)
                   {
                       Console.WriteLine(ip + ":" + port + " " + ex.Message);
                   }

                   if (client.Connected)
                   {
                       new System.Threading.Thread(ReceiveTask).Start();
                       return;
                   }


               }
           }
           catch { ;}
           finally
           {
               IsInConnectionTask = false;
           }

       }


       void ReceiveTask()
       {
             if (client == null || !client.Connected)
                   return;
             StreamReader rd = new StreamReader(client.GetStream());
           string res=null ;
           while( true)
           {
               try
               {

                   res = rd.ReadLine();
                   if (this.OnData != null)
                   {
                       try{
                       this.OnData(this, res);
                       }
                       catch(Exception ex1){
                          Console.WriteLine(ex1.Message+","+ex1.StackTrace)    ;
                       }
                   }
                   LastReceiveTime = DateTime.Now;
                  Console.WriteLine(res);
               }
               catch (Exception ex)
               {
                   Console.WriteLine(ex.Message);
                   break;
               }
             if (res == null)
                   break;


           }



             new System.Threading.Thread(ConnectionTask).Start();
       }


    }
}
