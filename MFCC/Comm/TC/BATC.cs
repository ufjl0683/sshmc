using System;
using System.Collections.Generic;
using System.Text;

namespace Comm.TC
{

    public class BATC
    {
        public event OnConnectedChangedHandler OnConnectChange;

        DateTime lastUpdateTime = System.DateTime.Now;
        bool _IsConnected;
        string IP;
        int port;
        public string DeviceName;
        public int controlid;
        System.Net.Sockets.TcpClient client;
        public BATC(int controlid, string IP, int port, bool IsConnected)
        {
            this.IP = IP;
            this.port = port;
            this.controlid = controlid;
            this.DeviceName = controlid.ToString();
            this.IsConnected = IsConnected;
            new System.Threading.Thread(ConnectTask).Start();
            new System.Threading.Thread(ReceiveTask).Start();
        }


        public void Send(string data)
        {
            if (!this.IsConnected)
                throw new Exception(this.DeviceName + ": not connected! ");
            byte[] d = System.Text.UTF8Encoding.UTF8.GetBytes(data);
            lock (this)
            {
                try
                {
                    client.GetStream().Write(d, 0, d.Length);
                    client.GetStream().Flush();
                }
                catch
                {
                    client.Close();
                }
            }
        }

        System.IO.StreamReader rd;//= new System.IO.StreamReader(client.GetStream(), System.Text.UTF8Encoding.UTF8);

        void ReceiveTask()
        {

            while (true)
            {
                try
                {

                    if (client != null && client.Connected)
                    {
                        int data = 0;

                        char[] chars = new char[1];
                        int cnt = 0;
                        while ((cnt = rd.Read(chars, 0, chars.Length)) != -1)
                        {

                            Console.Write(new String(chars, 0, cnt));
                            lastUpdateTime = DateTime.Now;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    this.Close();
                }


                System.Threading.Thread.Sleep(1000);
            }

        }
        public void Close()
        {
            if (this.client != null)
            {
                try
                {

                    client.Close();
                }
                catch { }
                client = null;
                this.IsConnected = false;

            }
        }
        public bool IsConnected
        {

            get
            {

                return _IsConnected;
            }
            set
            {
                if (_IsConnected != value)
                {
                    _IsConnected = value;
                    if (this.OnConnectChange != null)
                        this.OnConnectChange(this.controlid, this.DeviceName, value);
                }

            }
        }
        object ConnectObj = new object();
        void ConnectTask()
        {
            while (true)
            {
                try
                {
                    if (DateTime.Now.Subtract(lastUpdateTime).TotalMinutes > 20)
                    {
                        this.Close();
                    }
                    if (client == null || !client.Connected)
                    {
                        if (client != null)
                        {
                            try
                            {
                                client.Close();
                            }
                            catch { ;}
                        }

                        client = new System.Net.Sockets.TcpClient();
                        try
                        {
                            Console.WriteLine("connecting to " + this.IP + ":" + this.port);
                            client.Connect(this.IP, this.port);
                            this.IsConnected = true;
                            rd = new System.IO.StreamReader(client.GetStream(), System.Text.UTF8Encoding.UTF8);
                            Console.WriteLine(this.IP + ":" + this.port + "is conneted!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message + "," + ex.StackTrace);
                            this.IsConnected = false;

                            Console.WriteLine(client.Connected);

                        }

                    }

                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.Message + "," + ex1.StackTrace);
                }

                System.Threading.Thread.Sleep(3000);


            }  //while

        }
    }
}
