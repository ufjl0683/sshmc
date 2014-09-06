using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Host
{
   public  class SiteManager
    {


       System.Collections.Generic.Dictionary<int, Sensor.SensorBase> dictSnrInfo = new System.Collections.Generic.Dictionary<int, Sensor.SensorBase>();
        System.Collections.Generic.Dictionary<string,SiteInfo> dictSiteInfos=new Dictionary<string,SiteInfo>();
        Sensor.SensorManager snrmgr;
       public SiteManager( Sensor.SensorManager snrmgr)
       {
           this.snrmgr = snrmgr;
           LoadDB();
       }


       void LoadDB()
       {

           SSHMC01Entities1 db = new SSHMC01Entities1();
           var q = from n in db.tblSite select new SiteInfo() { SITE_ID = n.SITE_ID, CURRENT_DEGREE = n.CURRENT_DEGREE??0, REF_SENSOR_ID = n.REF_SENDSOR_ID??0, SEVENT_ID = n.SEVENT_ID??0, STATUS = n.STATUS };
           foreach (SiteInfo info in q)
           {
               dictSiteInfos.Add(info.SITE_ID, info);
           }

      //  var  q1 = from n in  select n;
         
          foreach (Sensor.SensorBase info in snrmgr.getAllDeviceEnum())
          {
              dictSnrInfo.Add(info.SensorID, info);
              this.dictSiteInfos[info.Site_ID].AddSiteSensorInfo(info);

               
          }
       }

       //public  void SetSensorCurrentLevel(int snrid,int level)
       //{
           
         
         
       //}


       public void NotifySponsor(string siteid, string mailaddress, string mailtitle, string mailbody)
       {
           if (!this.dictSiteInfos.ContainsKey(siteid))
               return;
               SSHMC01Entities1 db = new SSHMC01Entities1();
           SiteInfo siteinfo = dictSiteInfos[siteid];
           tblSite site = (from n in db.tblSite where n.SITE_ID == siteid select n).FirstOrDefault();

           if (site == null)
               return;

            site.STATUS = "C";  //waiting confirm
            site.CURRENT_DEGREE = siteinfo.CURRENT_DEGREE;
         //  site.SEVENT_ID = this.SEVENT_ID = evtid;
            site.REF_SENDSOR_ID = siteinfo.REF_SENSOR_ID;
           site.HappenTimeStamp =DateTime.Now;
           //site.ConfirmTimes =   0;
           db.SaveChanges();


           Global.SendMailToUser(mailaddress, mailtitle, mailbody);
       }

       public void SuspendEvent(string siteid)
       {
           if (!this.dictSiteInfos.ContainsKey(siteid))
               return;
           SSHMC01Entities1 db = new SSHMC01Entities1();
           SiteInfo siteinfo = dictSiteInfos[siteid];
           tblSite site = (from n in db.tblSite where n.SITE_ID == siteid select n).FirstOrDefault();

           if (site == null)
               return;

           site.STATUS = "S";  //waiting confirm
           site.CURRENT_DEGREE = siteinfo.CURRENT_DEGREE;
           //  site.SEVENT_ID = this.SEVENT_ID = evtid;
           site.REF_SENDSOR_ID = siteinfo.REF_SENSOR_ID;
           site.HappenTimeStamp = DateTime.Now;
           site.ConfirmTimes = 0;
           db.SaveChanges();
           //  throw new NotImplementedException();
       }

       public void ExeuteEvent(string siteid)
       {
           if (!this.dictSiteInfos.ContainsKey(siteid))
               return;
           SSHMC01Entities1 db = new SSHMC01Entities1();
           SiteInfo siteinfo = dictSiteInfos[siteid];
           tblSite site = (from n in db.tblSite where n.SITE_ID == siteid select n).FirstOrDefault();

           if (site == null)
               return;

           site.STATUS = "E";  //waiting confirm
           site.CURRENT_DEGREE = siteinfo.CURRENT_DEGREE;
           //  site.SEVENT_ID = this.SEVENT_ID = evtid;
           site.REF_SENDSOR_ID = siteinfo.REF_SENSOR_ID;
           site.HappenTimeStamp = DateTime.Now;
           site.ConfirmTimes = 0;
           db.SaveChanges();
           //  throw new NotImplementedException();
       }
       

    }

    

   public class SiteInfo
   {
       public string SITE_ID { get; set; }
       
       public int? CURRENT_DEGREE { get; set; }
       public string STATUS { get; set; }
       public int? REF_SENSOR_ID {get;set;}
       public int? SEVENT_ID { get; set; }
       public int? ConfirmTimes { get; set; }
       public DateTime? HappenTimeStamp { get; set; } // wait for confirm time
       
       System.Collections.Generic.List<Sensor.SensorBase> snrList = new List<Sensor.SensorBase>();

        public void AddSiteSensorInfo(Sensor.SensorBase info)
        {
            snrList.Add(info);
            info.OnDegreeChanged += info_OnDegreeChanged;
        }

        string ComposeBody(tblSite site,Sensor.SensorBase snr,int degree)
        {
            string body = "";
            body += site.SITE_NAME + "<br>";
            body += string.Format("sensorid:{0}  {1}  等級:{2} <br> ",snr.SensorID, snr.SensorName, (degree==0|| degree==-1)?degree:4-degree );
            return body;

        }


        void info_OnDegreeChanged(Sensor.SensorBase snr, int degree)
        {
            ////  //  SensorInfo snrinfo = snrList.Where(n => n.SENSOR_ID == info.SENSOR_ID).FirstOrDefault();
            ////    int maxDegree = snrList.Max(n => n.CURRENT_DEGREE);
            ////    // Controll flow here!
            SSHMC01Entities1 db = new SSHMC01Entities1();
           //   Sen snrinfo = snrList.Where(n => n.SENSOR_ID == info.SENSOR_ID).FirstOrDefault();
            int maxDegree = snrList.Max(n => n.CurrentDegree);
            tblSite site = db.tblSite.Where(n => n.SITE_ID == snr.Site_ID).FirstOrDefault();
            Console.Write("sitrid:" + this.SITE_ID + ",degree:" + maxDegree);
           //  Controll flow here!

            if (maxDegree > 0)  
            {
                if (CURRENT_DEGREE == 0) // new evenr
                {
                    int evtid = Global.GetAndUpdateCurrentEvenID();
                  
                    if (site != null)
                    {
                          this.STATUS=   site.STATUS = "A";  //waiting confirm
                           site.CURRENT_DEGREE =  this.CURRENT_DEGREE= maxDegree;
                           site.SEVENT_ID = this.SEVENT_ID= evtid;
                            site.REF_SENDSOR_ID =this.REF_SENSOR_ID= snr.SensorID;
                            site.HappenTimeStamp=this.HappenTimeStamp=DateTime.Now;
                            site.ConfirmTimes = this.ConfirmTimes = 0;
                            db.SaveChanges();

                            //string subject = site.SITE_NAME + (4 - maxDegree) + "級事件發生確認通知";
                            //string body = ComposeBody(site, snr, maxDegree);
                            //Global.SendMailToUser("ufjl0683@emome.net", subject, body);
                           

                        // mail to  waiting confirm
                    }

                }
                else if (CURRENT_DEGREE > 0)
                {
                    if (site.STATUS == "E")  //Execute  upgrade
                    {
                        if (maxDegree > CURRENT_DEGREE)  
                        {
                            //mail notify

                            site.HappenTimeStamp = this.HappenTimeStamp = System.DateTime.Now;
                            site.REF_SENDSOR_ID = this.REF_SENSOR_ID = snr.SensorID;
                            site.CURRENT_DEGREE = this.CURRENT_DEGREE = maxDegree;
                            db.SaveChanges();
                            string subject = ((DateTime)this.HappenTimeStamp).ToString() + "," + site.SITE_NAME + (4 - maxDegree) + "級升級事件通知";
                            string body = ComposeBody(site, snr, maxDegree);
                            string mailto = (from n in db.OMEMP where n.EMPNO == site.ENGINEER select n.MAIL).FirstOrDefault();
                            if (mailto != null)
                            {
                                try
                                {
                                    Global.SendMailToUser(mailto, subject, body);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message + "," + ex.StackTrace);
                                }
                            }
                          //  throw new NotImplementedException();
                        }
                        else   if(maxDegree <CURRENT_DEGREE)  // downgrade
                        {
                            site.HappenTimeStamp = this.HappenTimeStamp = System.DateTime.Now;
                            site.REF_SENDSOR_ID = this.REF_SENSOR_ID = snr.SensorID;
                            site.CURRENT_DEGREE = this.CURRENT_DEGREE = maxDegree;
                            db.SaveChanges();

                            string subject = ((DateTime)this.HappenTimeStamp).ToString() + "," + site.SITE_NAME + (4 - maxDegree) + "級降級級事件通知";
                            string body = ComposeBody(site, snr, maxDegree);
                            string mailto = (from n in db.OMEMP where n.EMPNO == site.ENGINEER select n.MAIL).FirstOrDefault();
                            if (mailto != null)
                            {
                                try
                                {
                                    Global.SendMailToUser(mailto, subject, body);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message + "," + ex.StackTrace);
                                }
                            }

                          //  throw new NotImplementedException();
                        }

                    }
                    else if (site.STATUS == "S" || site.STATUS == "C"  ||  site.STATUS=="A")
                    {
                        if (maxDegree > CURRENT_DEGREE)  //upgrade
                        {
                            //mail notify

                         //   if (site.STATUS == "S")
                                        this.STATUS = site.STATUS ="A";
                           
                            site.CURRENT_DEGREE = this.CURRENT_DEGREE = maxDegree;
                           
                            site.REF_SENDSOR_ID = this.REF_SENSOR_ID = snr.SensorID;
                            site.HappenTimeStamp = this.HappenTimeStamp = DateTime.Now;
                            site.ConfirmTimes = this.ConfirmTimes = 0;
                            db.SaveChanges();
                            if (site.STATUS != "A")
                            {
                              
                                string subject = site.SITE_NAME + (4 - maxDegree) + "級升級事件確認";
                                string body = ComposeBody(site, snr, maxDegree);
                                string mailto = (from n in db.OMEMP where n.EMPNO == site.ENGINEER select n.MAIL).FirstOrDefault();
                                if (mailto != null)
                                {
                                    try
                                    {
                                        Global.SendMailToUser(mailto, subject, body);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message + "," + ex.StackTrace);
                                    }
                                }
                                // Global.SendMailToUser(mailto, subject, body);
                            }
                          //  throw new NotImplementedException();
                        }
                        else if (maxDegree < CURRENT_DEGREE)  // downgrade
                        {
                            site.CURRENT_DEGREE = this.CURRENT_DEGREE = maxDegree;

                            site.REF_SENDSOR_ID = this.REF_SENSOR_ID = snr.SensorID;
                            site.HappenTimeStamp = this.HappenTimeStamp = DateTime.Now;
                           // site.ConfirmTimes = this.ConfirmTimes = 0;
                            db.SaveChanges();
                            if (site.STATUS != "A")
                            {
                                string subject = ((DateTime)this.HappenTimeStamp).ToString() + "," + site.SITE_NAME + (4 - maxDegree) + "級降級級事件通知";
                                string body = ComposeBody(site, snr, maxDegree);
                                string mailto = (from n in db.OMEMP where n.EMPNO == site.ENGINEER select n.MAIL).FirstOrDefault();
                                if (mailto != null)
                                {
                                    try
                                    {
                                        Global.SendMailToUser(mailto, subject, body);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message + "," + ex.StackTrace);
                                    }
                                }
                            }
                           // throw new NotImplementedException();
                        }



                    }


                   
                }

            }
            else if(maxDegree==0||maxDegree==-1)  //判斷是否事件該結束
            {
                if(CURRENT_DEGREE>0)
                {
                    string LastStatus = site.STATUS;
                    this.STATUS = site.STATUS = null;  //waiting confirm
                    site.CURRENT_DEGREE = this.CURRENT_DEGREE = maxDegree;
                    this.SEVENT_ID = null;
                    site.SEVENT_ID  = null;
                    site.REF_SENDSOR_ID = null;
                    this.REF_SENSOR_ID = -1;
                    site.HappenTimeStamp = this.HappenTimeStamp = null;
                    site.ConfirmTimes = this.ConfirmTimes = null;
                    db.SaveChanges();
                    if (LastStatus != "A")
                    {
                        string subject = (DateTime.Now).ToString() + "," + site.SITE_NAME + "事件結束通知";
                        string body = ComposeBody(site, snr, maxDegree);
                        string mailto = (from n in db.OMEMP where n.EMPNO == site.ENGINEER select n.MAIL).FirstOrDefault();
                        if (mailto != null)
                        {
                            try
                            {
                                Global.SendMailToUser(mailto, subject, body);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message + "," + ex.StackTrace);
                            }
                        }
                    }
                    // send mail to notify event has end
                }
            }

            //if(CURRENT_DEGREE!=maxDegree)
            //          CURRENT_DEGREE = maxDegree;
        }

        //void info_DegreeChangedEvent(SensorInfo info, int degree)
        //{

       

            
        //}
    


   }

   // public delegate void SensorDegreeChangeEventHandler(SensorInfo info,int degree);
   //public class SensorInfo
   //{

   //    int _degree;
   //    public string SITE_ID { get; set; }
   //    public int SENSOR_ID { get; set; }
   //    public int CURRENT_DEGREE {
   //        get
   //        {
   //            return _degree;
   //        }
   //        set
   //        {
   //            if (value != _degree)
   //            {
   //                _degree = value;
   //                if (DegreeChangedEvent != null)
   //                    DegreeChangedEvent(this, value);
   //            }
   //        }
          
   //    }

   //    public  event SensorDegreeChangeEventHandler DegreeChangedEvent;

   //}
}
