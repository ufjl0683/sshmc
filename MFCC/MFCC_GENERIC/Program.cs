using RemoteInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFCC_GENERIC
{
    class Program
    {

        public   static  MFCC_GENERIC mfcc_generic;
        static void Main(string[] args)
            // mfccid device type remoting_port
        {

            int NotifyPort = -1, RemotingPort = -1, ConsolePort = -1;
            string mfccid;
            if (args.Length == 3)
            {
                RemotingPort = int.Parse(args[2]);

                NotifyPort = RemotingPort - 6000;   // (int)NotifyServerPortEnum.MFCC_TILT1;
                //(int)RemotingPortEnum.MFCC_TILT1;
                ConsolePort = RemotingPort - 1000;// (int)ConsolePortEnum.MFCC_TILT1;
                mfccid = args[0];

                System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
                mfcc_generic = new MFCC_GENERIC(mfccid, args[1], RemotingPort, NotifyPort, ConsolePort,"MFCC_"+args[1].ToUpper(), typeof(RemoteObj));


                ConsoleServer.WriteLine(mfccid + " Start success!");
                //"MFCC_TILT1";

            }
            else
                Console.WriteLine("MFCC_GENERIC.exe  mfccid device type remoting_port");
        }
    }
}
