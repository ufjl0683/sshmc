using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteInterface;

namespace Host
{
   
   public class Host
   {
       public DbCmdServer dbserver=new DbCmdServer();
       public Sensor.SensorManager sensor_mgr = new Sensor.SensorManager();
       public ScriptsManager scripts_mgr = new ScriptsManager();
       public MFCC.MFCC_Manager mfcc_mgr;
       public  DevcieManager dev_mgr;
       public EventNotifyServer notifyServer;
       public Host()
       {
           mfcc_mgr = new MFCC.MFCC_Manager();
           dev_mgr = new DevcieManager(mfcc_mgr);
           initRemoteInterface();
       }

        void initRemoteInterface()
       {

           RemoteInterface.ConsoleServer.Start((int)RemoteInterface.ConsolePortEnum.Host);
           notifyServer = new RemoteInterface.EventNotifyServer((int)RemoteInterface.NotifyServerPortEnum.HOST);


           //ServerFactory.SetChannelPort((int)RemoteInterface.RemotingPortEnum.HOST_FIWS);
           //ServerFactory.RegisterRemoteObject(typeof(HC_FWIS_Robj), "FWIS");
           //ServerFactory.SetChannelPort((int)RemoteInterface.RemotingPortEnum.HOST_TIMCC);
           //ServerFactory.RegisterRemoteObject(typeof(HC_FWIS_Robj), "TIMCC");
           ServerFactory.SetChannelPort((int)RemoteInterface.RemotingPortEnum.HOST);
           ServerFactory.RegisterRemoteObject(typeof(HCommObject), "Comm");

       }


    }
}
