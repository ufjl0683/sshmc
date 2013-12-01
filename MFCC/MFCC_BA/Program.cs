using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteInterface;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace MFCC_BA
{
    class Program
    {
        public static MFCC_BA mfcc_ba;
        static void Main(string[] args)
        {

            int NotifyPort = -1, RemotingPort = -1, ConsolePort = -1;
            string mfccid = "MFCC_BA1";
            if (args.Length == 0 || args[0] == "MFCC_BA1")
            {
                NotifyPort = (int)NotifyServerPortEnum.MFCC_BA1;
                RemotingPort = (int)RemotingPortEnum.MFCC_BA1;
                ConsolePort = (int)ConsolePortEnum.MFCC_BA1;
                mfccid = "MFCC_BA1";

            }
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
            mfcc_ba = new MFCC_BA(mfccid, "BA", RemotingPort, NotifyPort, ConsolePort, "MFCC_BA", typeof(RemoteObj));


            ConsoleServer.WriteLine("MFCC_BA Start success!");
            //ToBAReport rpt = new ToBAReport() { reportURL = @"http://www.hinet.net", reportType = "N3" };
            //Console.WriteLine(rpt.ToJsonString());
            //ToBAStatus stat = new ToBAStatus()
            //{
            //    //IsConfirmed = "N",

            //    systemStatus = "0",
            //    deviceList = new ToBASensorStatus[]{
            //       new ToBASensorStatus(){ deviceType="GPS",deviceAdd="0000000", deviceValue="0"},
            //          new ToBASensorStatus(){ deviceType="TILT",deviceAdd="0000001", deviceValue="0"},
            //            new ToBASensorStatus(){ deviceType="TILT",deviceAdd="0000002", deviceValue="0"}
               
            //   }
            //};
            //Console.WriteLine(stat.ToJsonString());
        }
    }

  
}
