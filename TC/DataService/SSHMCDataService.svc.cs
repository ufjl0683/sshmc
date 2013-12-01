using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DataService
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼、svc 和組態檔中的類別名稱 "SSJMCDataService"。
   [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
     
    public class SSHMCDataService : ISSHMCDataService
    {
        public bool CheckUserIDPassword(string id, string Password)
        {
            SSHMC01Entities db = new SSHMC01Entities();

            tblUser user = (from n in db.tblUser where n.USER_ID == id && n.USER_PW == Password select n).FirstOrDefault();
            if (user == null)
                return false;
            else
                return true;
            //throw new NotImplementedException();
        }
        //public DataService.vwSiteDegree[] GetSiteInfoByCustomID(int id)
        //{

        //    SSHMC01Entities db = new SSHMC01Entities();
        //    if   id==1 )
        //    {
        //        var q = db.vwSiteDegree;
        //        return q.ToArray<DataService.vwSiteDegree>();
        //    }
        //    else
        //    {
        //        tblCustomer cust = db.tblCustomer.Where(n => n.CUSTOMER_ID == user.CUSTOMER_ID).FirstOrDefault();
        //        if (cust.SUB_CUSTOMER_IDS == null || cust.SUB_CUSTOMER_IDS.Trim() == "")
        //        {
        //            var q = (from n in db.vwSiteDegree where n.CUSTOMER_ID == user.CUSTOMER_ID select n);
        //            return q.ToArray<DataService.vwSiteDegree>();
        //        }
        //        else
        //        {
        //            string[] subCustIDstr = cust.SUB_CUSTOMER_IDS.Split(new char[] { ',' });
        //            int[] subCustIDs = new int[subCustIDstr.Length];
        //            for (int i = 0; i < subCustIDstr.Length; i++)
        //            {
        //                subCustIDs[i] = Convert.ToInt32(subCustIDstr[i]);
        //            }

        //            var q = (from n in db.vwSiteDegree where n.CUSTOMER_ID == cust.CUSTOMER_ID select n).Union(from m in db.vwSiteDegree where subCustIDs.Contains(m.CUSTOMER_ID) select m);
        //            return q.ToArray<DataService.vwSiteDegree>();
        //        }
        //    }
        //}
      public  DataService.vwSiteDegree[] GetSiteInfo(string id) 
          //id =="*" 查回全部
        {
            SSHMC01Entities db = new SSHMC01Entities();
            DataService.tblUser user=null;
            if (id !="*")
            {
                  user = (from n in db.tblUser where n.USER_ID == id select n).FirstOrDefault();

                if (user == null)
                    return null;
            }
              if(  id=="*"|| (user.CUSTOMER_ID??-1)==1)
              {
                    var   q = db.vwSiteDegree;
                        return q.ToArray<DataService.vwSiteDegree>();
              }
              else 
              {
                  tblCustomer cust = db.tblCustomer.Where(n => n.CUSTOMER_ID == user.CUSTOMER_ID).FirstOrDefault();
                  if (cust.SUB_CUSTOMER_IDS == null || cust.SUB_CUSTOMER_IDS.Trim()=="" )
                  {
                      var q = (from n in db.vwSiteDegree where n.CUSTOMER_ID == user.CUSTOMER_ID select n);
                      return q.ToArray<DataService.vwSiteDegree>();
                  }
                  else
                  {
                      string[] subCustIDstr = cust.SUB_CUSTOMER_IDS.Split(new char[] { ',' });
                      int[] subCustIDs = new int[subCustIDstr.Length];
                      for  (int i=0;i<subCustIDstr.Length;i++)
                      {
                          subCustIDs[i] = Convert.ToInt32(subCustIDstr[i]);
                      }

                      var q = (from n in db.vwSiteDegree where n.CUSTOMER_ID == cust.CUSTOMER_ID select n).Union(from m in db.vwSiteDegree where subCustIDs.Contains(m.CUSTOMER_ID) select m);
                      return q.ToArray<DataService.vwSiteDegree>();
                  }
              }
           
      
      
       
            


            throw new Exception("GetSiteInfo error!");


        }


      public DataService.vwSensorDegree[] GetSensorInfo(string SiteID)
      {
          SSHMC01Entities db = new SSHMC01Entities();
          var q = from n in db.vwSensorDegree where n.SITE_ID == SiteID select n ;


          return q.ToArray<DataService.vwSensorDegree>();

      }


      public tblCCTV[] GetCCTVInfo(string SiteID)
      {
          SSHMC01Entities db = new SSHMC01Entities();
          var q = from n in db.tblCCTV where n.SITE_ID == SiteID select n;


          return q.ToArray<DataService.tblCCTV>();
         // throw new NotImplementedException();
      }


      public tblPre_disasterNotified[] GetDisasterInfo()
      {
          try
          {
           //   DateTime dt=DateTime.Now.Subtract(TimeSpan.FromDays(7));
              SSHMC01Entities db = new SSHMC01Entities();
              var q = (from n in db.tblPre_disasterNotified      orderby n.TIMESTAMP descending select n).Take(20).ToArray();
              return q.ToArray<DataService.tblPre_disasterNotified>();
          }
          catch (Exception ex)
          {
              throw new FaultException(ex.Message + "," + ex.StackTrace);
          }
         // throw new NotImplementedException();
      }





      public tblSurvey_Disaster[] GetSurveyDisaster()
      {
          SSHMC01Entities db = new SSHMC01Entities();
          var q = from n in db.tblSurvey_Disaster where n.ISCHECK == true && n.ISCLOSE == false select n;
          return q.ToArray<DataService.tblSurvey_Disaster>();

         // throw new NotImplementedException();
      }


      public string AddSurverDiasterInfo(tblSurvey_Disaster info)
      {
         
          SSHMC01Entities db = new SSHMC01Entities();
          try
          {
              db.tblSurvey_Disaster.AddObject(info);
              db.SaveChanges();
          }
          catch (
              Exception ex)
          {
              return ex.Message+","+ex.StackTrace+ex.InnerException.Message;
          }

          return "ok";
      }


      public vwReportNotified[] GettblReportNotified(string userid)
      {
          SSHMC01Entities db = new SSHMC01Entities();
           tblUser user=db.tblUser.Where(n => n.USER_ID == userid).FirstOrDefault();
          if (user == null)
              return null;
          int customerid =(int) user.CUSTOMER_ID;

         
          if (customerid == 1 || userid == "*")
          {
             return (from n in db.vwReportNotified select n).ToArray();
          }
          else
          {


              tblCustomer cust = db.tblCustomer.Where(n => n.CUSTOMER_ID == user.CUSTOMER_ID).FirstOrDefault();
              if (cust.SUB_CUSTOMER_IDS == null || cust.SUB_CUSTOMER_IDS.Trim() == "")
              {
                  var q = (from n in db.vwReportNotified where n.CUSTROMER_ID == user.CUSTOMER_ID select n);
                  return q.ToArray<DataService.vwReportNotified>();
              }
              else
              {
                  string[] subCustIDstr = cust.SUB_CUSTOMER_IDS.Split(new char[] { ',' });
                  int[] subCustIDs = new int[subCustIDstr.Length];
                  for (int i = 0; i < subCustIDstr.Length; i++)
                  {
                      subCustIDs[i] = Convert.ToInt32(subCustIDstr[i]);
                  }

                  var q = (from n in db.vwReportNotified where n.CUSTROMER_ID == cust.CUSTOMER_ID select n).Union(from m in db.vwReportNotified where subCustIDs.Contains(m.CUSTROMER_ID) select m);
                  return q.ToArray<DataService.vwReportNotified>();
              }
          }
          //    q1 = (from n in db.tblSite where n.CUSTROMER_ID == customerid select n.SITE_ID).ToArray();
          //var q = from n in db.tblReportNotified where q1.Contains(n.SITE_ID) select n;
          //return q.ToArray();

        throw new Exception("GettblReportNotified error");

          
      }


      public  DataService.vwSensorValuesAndTC10MinDataLog[]GetvwSensorValuesAndTC10MinDataLog(int snrid,DateTime startDate,DateTime EndDate)
      {
          SSHMC01Entities db = new SSHMC01Entities();
          return db.vwSensorValuesAndTC10MinDataLog.Where(n => n.SENSOR_ID == snrid && n.TIMESTAMP >= startDate && n.TIMESTAMP < EndDate).ToArray();

         // throw new NotImplementedException();
      }
    }
}
