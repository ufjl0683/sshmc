using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Host
{
   public  class Global
    {
       public static int GetAndUpdateCurrentEvenID()
       {
           SSHMC01Entities1 db = new SSHMC01Entities1();
          
           db.tblSiteEventMaintain.First().SEVENT_ID += 1;
           db.SaveChanges();
          return db.tblSiteEventMaintain.First().SEVENT_ID;
       }


       public static void SendMailToUser(string mailaddress,string subject,string bodytext)
       {
           WebClient client = new WebClient();
           string url = "http://localhost:8080/WeatherMailService/SendMailToUser?address={0}&subject={1}&bodytext={2}";
           string res = new System.IO.StreamReader(client.OpenRead(string.Format(url, mailaddress, subject, bodytext))).ReadToEnd();
           Console.WriteLine(res);
       }

    }
}
