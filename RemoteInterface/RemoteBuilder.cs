using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting;
//using System.Data.Odbc;
using System.Data.SqlClient;


namespace RemoteInterface
{
    public delegate void StopEventHandler();
    
    public class RemoteBuilder
    {
     // static    string HOST_IP = "10.21.50.4";
        static string DB_IP = "192.168.161.2";
         static string db2ConnectionStr ="Data Source=192.192.161.2;Initial Catalog=SSHMC01;User ID=david;Password=ufjl0683@";
        public static event StopEventHandler OnStop;
        static System.Collections.Hashtable tcpchannels = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());

     //   public static string MasterIp, SlaveIp, ReadyIp,MasterHeartBeatIp,SlaveHeartBeatIp;
         static RemoteBuilder()
        {
           

        }



        public static string  getHostIP()
        {
            SqlConnection cn = new SqlConnection(db2ConnectionStr);
            try
            {
               
                SqlCommand cmd = new SqlCommand("select host_ip from tblhost_config where host_id='HOST'", cn);
                cmd.Connection = cn;
                cn.Open();
                return cmd.ExecuteScalar().ToString();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message+","+ ex.StackTrace);
                throw new Exception(ex.Message+ex.StackTrace);
            }
            finally
            {
                cn.Close();
                
            }
        }

        public static string getDbIP()
        {
            return DB_IP;
        }


        public static string getMFCC_IP(string mfccid)
        {
            SqlConnection cn = new SqlConnection(db2ConnectionStr);
            SqlCommand cmd = new SqlCommand(string.Format("select hostip from vwHostMfcc where mfccid='{0}'", mfccid), cn);
            try
            {
                cn.Open();
                return cmd.ExecuteScalar().ToString();
            }
            finally
            {

                cn.Close();
            }
          
        }

       public static   string getRemoteUri(string ip, int port, string regObjName)
        {
            return string.Format("tcp://{0}:{1}/{2}", ip, port, regObjName);
        }

       

        //public static void StartHeartBeat()
        //{
        //    try
        //    {
        //        RegisterRemoteObject(typeof(RemoteInterface.HeartBeat), "HeartBeat");
        //        Console.WriteLine("HeartBeat Start!");
        //    }
        //    catch (Exception)
        //    {
        //        Console.WriteLine("HeartBeat Start error!");
        //    }
        //}
        ////public static void SetModeAAndIPs(bool _isMasterMode,string _ReadyIP,string _MasterIp,string _SlaveIp,string _MasterHeartBeatIp,string _SlaveHeartBeatIp)
        //{
        //    IsMasterMode = _isMasterMode;
        //    ReadyIp = _ReadyIP;
        //    MasterIp = _MasterIp;
        //    SlaveIp = _SlaveIp;
        //    MasterHeartBeatIp = _MasterHeartBeatIp;
        //    SlaveHeartBeatIp = _SlaveHeartBeatIp;
        //}

       public static int getTcpChannelsCount()
       {
           return System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels.Length;
           //System.Runtime.Remoting.Channels.Tcp.TcpChannel tcp = new TcpChannel();
           
       }
        public static object GetRemoteObj(System.Type type, string uri)
        {
            object obj = null;
            if (tcpchannels[uri] == null)
            {   
                tcpchannels.Add(uri, new System.Runtime.Remoting.Channels.Tcp.TcpClientChannel(uri, null));
                System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel((TcpClientChannel)  tcpchannels[uri],false);
            
            }
            try
            {
                
                obj = Activator.GetObject(
                    type,
                    uri);
                ((RemoteClassBase)obj).HelloWorld();
            }
            catch (Exception ex)
            {
               // Utils.Util.SysLog("remoting.log", ex.Message + "," + ex.StackTrace);
                obj = null;
                try
                {
                    System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel((IChannel)tcpchannels[uri]);
                    tcpchannels.Remove(uri);
                  
                }
                catch{
                   
                    throw new Exception(ex.Message+ex.StackTrace);
                }
                


            }
            
            return obj;
        }             

        public static Object GetRemoteObj(System.Type type,System.Runtime.Remoting.Channels.Tcp.TcpClientChannel tcp,string url)
           //return null for Fail
        {
            if (tcp != null)
            {
                foreach (System.Runtime.Remoting.Channels.IChannel tcpchannel in System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels)
                {
                    if(tcp.ChannelName==tcpchannel.ChannelName)
                        System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(tcpchannel);
                }
            }
            else
                tcp = new System.Runtime.Remoting.Channels.Tcp.TcpClientChannel(url,null);
             

           
            System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(tcp, false);

           System.Object obj = null;

            try
            {

                obj = Activator.GetObject(
                    type,
                    url);
                ((RemoteClassBase)obj).HelloWorld();
            }
            catch(Exception ex)
            {
                obj = null;
                try
                {
                    System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(tcp);
                }
                catch { }
                throw new Exception(ex.Message+ex.StackTrace);
              

            }
            finally
            {
              
            }
            return obj;
        }

        //public static HeartBeat GetHeartBeat(string ip)
        //{
            
        
        //    try
        //    {
        //        RemoteInterface.HeartBeat hbObj = (RemoteInterface.HeartBeat)RemoteInterface.RemoteBuilder.GetRemoteObj(typeof(RemoteInterface.HeartBeat), HeartbeatClientTcp, "tcp://" + ip + ":9090/HeartBeat");
        //        return hbObj;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return null;
        //    }
            

        //}

        //public static bool IsHeatBeat(string ip)
        //{
        //    if(GetHeartBeat(ip)!=null)
        //        return true;
        //    else
        //        return false;
        //}

     
        public static void  InvokeStop()
        {
           if(OnStop!=null)
              OnStop();
        }
        public static  bool IsRemoteObjectAliive(object robj)
        {
            try
            {
                if (robj == null)
                    return false;
                ((RemoteInterface.RemoteClassBase)robj).HelloWorld();
                return true;
            }
            catch
            {
                return false;
            }

        }
        
    }



  



   
}
