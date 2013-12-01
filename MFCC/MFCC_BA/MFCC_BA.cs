using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteInterface;
using System.Data;
using Comm;
using Comm.MFCC;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;


namespace MFCC_BA
{
    class MFCC_BA  
    {
        public RemoteInterface.HC.I_HC_Comm r_host_comm;
        System.Collections.Generic.Dictionary<int, Comm.TC.BATC> manager = new Dictionary<int, Comm.TC.BATC>();
        string mfccid;
        string devType;
        int remotePort;
        int notifyPort;
        int consolePort;
        string regRemoteName;
        Type regRemoteType;
        protected RemoteInterface.EventNotifyServer notifier;
        System.Threading.Timer tmr10min;
        protected DbCmdServer dbServer = new DbCmdServer();
        bool IsLoadTcCompleted;
        public MFCC_BA(string mfccid, string devType, int remotePort, int notifyPort, int consolePort, string regRemoteName, Type regRemoteType)
           
        {


            this.mfccid = mfccid;
            this.devType = devType;
            this.remotePort = remotePort;
            this.notifyPort = notifyPort;
            this.consolePort = consolePort;
            this.regRemoteName = regRemoteName;
            this.regRemoteType = regRemoteType;

            notifier = new EventNotifyServer(notifyPort);

            ConsoleServer.WriteLine("loading Tc ...");
            loadTC_AndBuildManaer();
            IsLoadTcCompleted = true;
            this.AfterDeviceAllStart();
            ConsoleServer.WriteLine("load Tc Completed!");
            init_RemoteInterface();

            tmr10min = new System.Threading.Timer(new System.Threading.TimerCallback(TmrTask));
            
          //  double secdiff=0;
                Console.WriteLine("next tmr task:"+ DateTime.Now.Add(GetNext10MinTimeSpan()));
            tmr10min.Change((long)GetNext10MinTimeSpan().TotalMilliseconds, -1);

        }

        TimeSpan GetNext10MinTimeSpan()
        {
            int year, month, day, hour, min, sec;
            DateTime dt = System.DateTime.Now;
            year = dt.Year;
            month = dt.Month;
            day = dt.Day;
            hour = dt.Hour;
            min = dt.Minute;
            sec = dt.Second;
            dt = new DateTime(year, month, day, hour, min, 0);
#if DEBUG
             dt = dt.AddMinutes((min+2)/2*2-min );
#else
            dt = dt.AddMinutes((min+10)/10*10-min+1);
#endif

            return dt.Subtract(DateTime.Now);
        

        }
        //TimeSpan GetNext1MinTimeSpan()
        //{
        //    int year, month, day, hour, min, sec;
        //    DateTime dt = System.DateTime.Now;
        //    year = dt.Year;
        //    month = dt.Month;
        //    day = dt.Day;
        //    hour = dt.Hour;
        //    min = dt.Minute;
        //    sec = dt.Second;
        //    dt = new DateTime(year, month, day, hour, min, 0);
        //    dt = dt.AddMinutes(( 1 )  );

        //    return dt.Subtract(DateTime.Now);


        //}
        void TmrTask(object state)
        {

            try
            {
                ToBaTask();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "," + ex.StackTrace);
            }
            try
            {
                ToBaReportTask();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "," + ex.StackTrace);
            }

             
                tmr10min.Change((long)GetNext10MinTimeSpan().TotalMilliseconds, -1);
        
            
            Console.WriteLine(System.DateTime.Now+" tmr task");
        }


        void ToBaReportTask()
        {
            SSHMC01Entities db = new SSHMC01Entities();
             var q =from n in db.vwSiteToBA_Report where n.ISTOBA==false && n.MFCC_ID==this.mfccid      select n;
             foreach (vwSiteToBA_Report rpt in q)
             {
                 try
                 {
                     ToBAReport report = new ToBAReport()
                     {
                         reportType = rpt.TYPE,
                         reportURL = rpt.URL
                     };

                     if (manager.ContainsKey(rpt.CONTROLLER_ID))
                     {
                         if (manager[rpt.CONTROLLER_ID].IsConnected)
                         {
                             manager[rpt.CONTROLLER_ID].Send(report.ToJsonString());
                         }

                         var q1 = from n in db.tblReportNotified where n.SITE_ID == rpt.SITE_ID && n.DATATIME == rpt.DATATIME && n.TYPE == rpt.TYPE select n;

                         tblReportNotified tmp = q1.FirstOrDefault();
                         if (tmp != null)
                             tmp.ISTOBA = true;
                     }
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine(ex.Message + "," + ex.StackTrace);
                 }

             }
             db.SaveChanges();
            
        }
        void ToBaTask()
        {
            SSHMC01Entities db = new SSHMC01Entities();
            var q = db.vwSiteToBA_Status;

            foreach (vwSiteToBA_Status site in q)
            {
                ToBAStatus BaStatus = new ToBAStatus()
                {
                    systemStatus = site.current_degree.ToString()
                };
                try
                {
                    var q1 = (from n in db.vwSensorDegree
                              where
                                  n.SITE_ID == site.SITE_ID  
                              select n);

                    ToBASensorStatus[] snrstat = new ToBASensorStatus[q1.Count()];

                    int i = 0;

                    foreach (vwSensorDegree snr in q1)
                    {
                        snrstat[i++] = new ToBASensorStatus() { deviceAdd = snr.SENSOR_ID.ToString(), deviceType = snr.SENSOR_TYPE, deviceValue = snr.CURRENT_DEGREE.ToString() };
                    }
                    BaStatus.deviceList = snrstat;

                    var q3 = from n in db.tblTC where n.SITE_ID == site.SITE_ID && n.DEVICE_TYPE == "BA" select n;
                    foreach (tblTC tc in q3)
                    {
                        if (manager.ContainsKey(tc.CONTROLLER_ID))
                            if (manager[tc.CONTROLLER_ID].IsConnected)
                            {

                                // send tc here
                              //  Console.WriteLine(BaStatus.ToJsonString());
                                try
                                {
                                    manager[tc.CONTROLLER_ID].Send(BaStatus.ToJsonString());
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message + "," + ex.StackTrace);   
                                }
                            }

                       
                    }


                    // BaStatus.deviceList = snrStatus;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "," + ex.StackTrace);
                }


                
            }
           
        }

        void OnTCConnectedChangeTask(int id, string senserName, bool IsConnected)
        {
            string sql;

            if (IsConnected)
                sql = "update tbltc set IsConnected='Y'  where controller_id=" + id;
            else
                sql = "update tbltc set IsConnected='N'  where controller_id=" + id;
            this.dbServer.SendSqlCmd(sql);
        }

        void loadTC_AndBuildManaer()
        {


            try
            {  
                SSHMC01Entities db = new SSHMC01Entities();
              //  var q = from n in db.tblTC where n.MFCC_ID == this.mfccid && n.ENABLE == "Y" select n;
               var q=  db.tblTC.Where(n => n.ENABLE == "Y" && n.MFCC_ID == this.mfccid);

               foreach (tblTC tc in q)
               {
                   Comm.TC.BATC batc = new Comm.TC.BATC(tc.CONTROLLER_ID, tc.IP, tc.PORT, tc.ISCONNECTED == "Y" ? true : false);
                   manager.Add(batc.controlid,  batc);

                   batc.OnConnectChange +=  OnTCConnectedChangeTask;
                   
                   
               
               }
                    
                    //SqlDataReader rd;
                //SqlConnection cn = new SqlConnection(Comm.SQL.SQL.dbConnectionStr);

                
                //rd = Comm.SQL.SQL.getDeviceConfigReader(cn, this.mfccid);

                //// Comm.TCBase tc=null;
                //while (rd.Read())
                //{
                //    string devtype = rd[8].ToString();
                //    try
                //    {
                //     //   manager..AddDevice(rd[0].ToString());
                //    }
                //    catch (Exception ex)
                //    {
                //        ConsoleServer.WriteLine(ex.Message + ex.StackTrace);
                //    }


                //}
                //rd.Close();
                //cn.Close();




            }
            catch (Exception ex)
            {
                ConsoleServer.WriteLine(ex.Message + ex.StackTrace);
            }
        }

        void AfterDeviceAllStart()
        {
        }

        public virtual void init_RemoteInterface()
        {// mark 2012-7-26
            //try
            //{
            //    r_host_comm = (I_HC_Comm)RemoteBuilder.GetRemoteObj(typeof(I_HC_Comm), RemoteBuilder.getRemoteUri(RemoteBuilder.getHostIP(), (int)RemotingPortEnum.HOST, "Comm"));

            //    if (r_host_comm == null)
            //        new System.Threading.Thread(RemoteObjectConnectTask).Start();
            //}
            //catch(Exception ex) {
            //    ConsoleServer.WriteLine(ex.Message+ex.StackTrace);}


            try
            {
                RemoteInterface.ServerFactory.SetChannelPort(remotePort);
                ServerFactory.RegisterRemoteObject(regRemoteType, regRemoteName);
                ConsoleServer.WriteLine("RemotePort Listen at " + remotePort);
            }
            catch (Exception ex)
            {
                ConsoleServer.WriteLine(ex.Message + ex.StackTrace);
            }
            ConsoleServer.Start(consolePort);



        }

        //void CommtestTask()
        //{

        //    Comm.TC.TILTTC tc = (Comm.TC.TILTTC)this.getTcManager()["VD231"];
        //    while (true)
        //    {
        //        try
        //        {
        //            if (tc.IsConnected)
        //            {
        //                // tc.getHwStaus();
        //                tc.TC_GetHW_Status();
        //             //   tc.Tc_GetVDData(System.DateTime.Now);

        //                //      Console.WriteLine( tc.m_device.getQueueState());
        //                //   System.Threading.Thread.Sleep(3000);

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message + ex.StackTrace);
        //        }


        //    }

        }

    [DataContract]
    public class ToBAStatus
    {
        public ToBAStatus()
        {
            messageType = "status";
        }
        [DataMember]
        public string messageType { get; set; }
        [DataMember]
        public string systemStatus { get; set; }
        //[DataMember]
        //public string IsConfirmed { get; set; }
        [DataMember]
        public ToBASensorStatus[] deviceList { get; set; }

        public string ToJsonString()
        {
            DataContractJsonSerializer jser = new DataContractJsonSerializer(typeof(ToBAStatus));
            MemoryStream sw = new MemoryStream();
            jser.WriteObject(sw, this);
            return System.Text.UTF8Encoding.UTF8.GetString(sw.ToArray());

        }


    }

    [DataContract]
    public class ToBASensorStatus
    {

        [DataMember]
        public string deviceAdd { get; set; }
        [DataMember]
        public string deviceType { get; set; }
        [DataMember]
        public string deviceValue { get; set; }
    }

    [DataContract]
    public class ToBAReport
    {
        public ToBAReport()
        {
            messageType = "report";
        }
        [DataMember]
        public string messageType { get; set; }  //default report
        [DataMember]
        public string reportType { get; set; }
        [DataMember]
        public string reportURL { get; set; }

        public string ToJsonString()
        {
            DataContractJsonSerializer jser = new DataContractJsonSerializer(typeof(ToBAReport));
            MemoryStream sw = new MemoryStream();
            jser.WriteObject(sw, this);
            return System.Text.UTF8Encoding.UTF8.GetString(sw.ToArray());

        }
    }
        //void DataRepairTask()
        //{

        //    System.Collections.ArrayList aryThread = new System.Collections.ArrayList();

        //    System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Comm.SQL.SQL.dbConnectionStr);
        //    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
        //    cmd.CommandTimeout = 120;
        //    StRepairData rpd = null;//=new StRepairData();

        //    cmd.Connection = cn;

        //    System.Collections.Queue queue = new System.Collections.Queue();

        //    int dayinx = 1;
        //    while (true)
        //    {
        //        Comm.TC.TILTTC tc;

        //        if (!IsLoadTcCompleted)
        //        {
        //            System.Threading.Thread.Sleep(1000);
        //            continue;
        //        }

        //        try
        //        {
        //            //  cn= new System.Data.Odbc.OdbcConnection(Comm.DB2.Db2.db2ConnectionStr);
        //            while (this.dbServer.getCurrentQueueCnt() > 50)
        //                System.Threading.Thread.Sleep(1000 * 10);
        //            cn.Open();
        //            ConsoleServer.WriteLine("Repair task begin!");
        //            cmd.Connection = cn;
        //            //   string sqlGetRepair = "select * from (SELECT t1.DEVICENAME, t1.TIMESTAMP ,trycnt,datavalidity FROM TBLVDDATA1MIN t1 inner join tbldeviceconfig t2 on t1.devicename=t2.devicename WHERE  mfccid='{0}'  and comm_state <> 3  and  t1.TIMESTAMP between '{1}' and '{2}' and trycnt<3 fetch first 300 row only  )  where  DATAVALIDITY = 'N' order by trycnt,timestamp desc  ";
        //            string sqlGetRepair = "select t1.DEVICENAME, TIMESTAMP ,trycnt,datavalidity,comm_state from TBLVDDATA1MIN  t1 inner join tblDeviceConfig t2 on t1.devicename=t2.devicename where mfccid='{0}' and  TIMESTAMP between '{1}' and '{2}' and trycnt <1 and datavalidity='N' and comm_state=1  and enable='Y'  order by timestamp desc  fetch first 300  row only ";

        //            cmd.CommandText = string.Format(sqlGetRepair, mfccid, Comm.SQL.SQL.getTimeStampString(System.DateTime.Now.AddDays(-dayinx)), Comm.SQL.SQL.getTimeStampString(System.DateTime.Now.AddDays(-dayinx + 1).AddMinutes(-10)));
        //            System.Data.SqlClient.SqlDataReader rd = cmd.ExecuteReader();

        //            while (rd.Read())
        //            {
        //                string devName = "";
        //                DateTime dt;
        //                devName = rd[0] as string;
        //                dt = System.Convert.ToDateTime(rd[1]);
        //                queue.Enqueue(new StRepairData(dt, devName));

        //            }
        //            rd.Close();

        //            ConsoleServer.WriteLine("total:" + queue.Count + " to repair!");
        //            if (queue.Count < 300)
        //            {
        //                dayinx++;
        //                if (dayinx == 4)
        //                    dayinx = 1;
        //            }
        //            if (queue.Count == 0)
        //                System.Threading.Thread.Sleep(1000 * 60);

        //            aryThread.Clear();
        //            while (queue.Count != 0)
        //            {
        //                try
        //                {

        //                    rpd = (StRepairData)queue.Dequeue();
        //                    if (Program.mfcc_tilt.manager.IsContains(rpd.devName))
        //                        tc = (Comm.TC.TILTTC)Program.mfcc_tilt.manager[rpd.devName];
        //                    else


        //                        continue;


        //                    if (!tc.IsConnected)
        //                    {
        //                        dbServer.SendSqlCmd(string.Format("update tbldeviceconfig  set comm_state=3 where devicename='{0}' ", rpd.devName));

        //                        continue;
        //                    }

        //                    System.Threading.Thread th = new System.Threading.Thread(Repair_job);
        //                    aryThread.Add(th);
        //                    th.Start(rpd);

        //                    if (aryThread.Count >= 5)
        //                    {
        //                        for (int i = 0; i < aryThread.Count; i++)


        //                            ((System.Threading.Thread)aryThread[i]).Join();


        //                        aryThread.Clear();
        //                    }


        //                    //   ConsoleServer.WriteLine("==>repair:" + rpd.devName + "," + rpd.dt.ToString());

        //                }
        //                catch (Exception ex)
        //                {
        //                    ConsoleServer.WriteLine(ex.Message + ex.StackTrace);

        //                }
        //            }


        //            for (int i = 0; i < aryThread.Count; i++)


        //                ((System.Threading.Thread)aryThread[i]).Join();


        //            aryThread.Clear();





        //        }
        //        catch (Exception x)
        //        {
        //            ConsoleServer.WriteLine(x.Message + x.StackTrace);
        //        }
        //        finally
        //        {
        //            try
        //            {
        //                cn.Close();
        //            }
        //            catch { ;}
        //        }


        //    }


        //}

        //public void Repair_job(System.Object repairinfo)
        //{
        //    Comm.TC.TILTTC tc;
        //    StRepairData RepairInfo = repairinfo as StRepairData;
        //    try
        //    {
        //        tc = (Comm.TC.TILTTC)Program.mfcc_tilt.manager[RepairInfo.devName];
        //      //  Comm.TC.VD_MinAvgData data = tc.getOneMinAvgData(tc.Tc_GetVDData(RepairInfo.dt), RepairInfo.dt.Year, RepairInfo.dt.Month);
        //      //  dbServer.SendSqlCmd(data.get1minUpdateSql());
        //        ConsoleServer.WriteLine("==>repair:" + RepairInfo.devName + "," + RepairInfo.dt.ToString());

        //    }
        //    catch (Exception ex)
        //    {
        //        try
        //        {
        //            dbServer.SendSqlCmd(string.Format("update TBLVDDATA1MIN set trycnt=trycnt+1 where devicename='{0}' and  timestamp='{1}'", RepairInfo.devName, Comm.SQL.SQL.getTimeStampString(RepairInfo.dt)));
        //        }

        //        catch (Exception ee)
        //        {
        //            ConsoleServer.WriteLine(ee.Message + ee.StackTrace);
        //        }
        //        ConsoleServer.WriteLine(ex.Message + "," + ex.StackTrace);
        //    }
        //}




        //public override void BindEvent(object tc)
        //{
        //   // Comm.TC.TILTTC vdtc = (Comm.TC.TILTTC)tc;
        //    //((Comm.TC.TILTTC)tc).OnRealTimeData += new Comm.TC.OnRealTimeEventHandler(tc_OnRealTimeData);
        //    //((Comm.TC.TILTTC)tc).OnTriggerEvent += new Comm.TC.OnTriggerEventHandler(tc_OnTriggerEvent);
        //    //((Comm.TC.TILTTC)tc).On1MinTrafficData += new Comm.TC.On1MinEventDataHandler(tc_On1MinTrafficData);
        //    //((Comm.TC.TILTTC)tc).On20SecEvent += new Comm.TC.On20SecDataHandler(MFCC_VD_On20SecEvent);
        //    //((Comm.TC.TILTTC)tc).OnFiveMinAvgData += new Comm.TC.FiveMinAvgEventHandler(MFCC_VD_OnFiveMinAvgData);

        //}

        //void MFCC_VD_OnFiveMinAvgData(object vdtc, Comm.TC.VD_MinAvgData data)
        //{

        //    try
        //    {
        //        if (r_host_comm == null)
        //            return;
        //        // lock(this.r_host_comm)   
        //        this.r_host_comm.setVDFiveMinData(data.devName, data.ToVD1MinCycleEventData());
        //    }
        //    catch (Exception ex)
        //    {
        //        ConsoleServer.WriteLine("HostException!" + ex.Message + ex.StackTrace);
        //    }

        //}


        //object fileLockObj = new object();
        //void MFCC_VD_On20SecEvent(object vdtc, Comm.TC.VD_MinAvgData data)
        //{


        //    Comm.TC.VDTC tc = (Comm.TC.VDTC)vdtc;

        //    ConsoleServer.WriteLine(tc.DeviceName + "," + data.dateTime + ",20SecEvent!");

        //    lock (fileLockObj)
        //    {
        //        string path = Comm.Util.CPath(System.AppDomain.CurrentDomain.BaseDirectory + string.Format(@"\Vd20Sec\{0:00}{1:00}{2:00}", data.year, data.month, data.day));
        //        if (!System.IO.File.Exists(path))
        //            System.IO.Directory.CreateDirectory(Comm.Util.CPath(path));



        //        System.IO.StreamWriter wr = System.IO.File.AppendText(Comm.Util.CPath(path + "\\" + string.Format("{0}.txt", tc.DeviceName)));
        //        // wr.WriteLine(tc.DeviceName+","+data.dateTime+",20SecEvent!");

        //        wr.WriteLine(VD20SecToCsvStr(tc.DeviceName, data));
        //        wr.Flush();
        //        wr.Close();
        //        wr.Dispose();
        //    }

        //    //throw new Exception("The method or operation is not implemented.");
        //}


        //private string VD20SecToCsvStr(string devName, Comm.TC.VD_MinAvgData data)
        //{
        //    //    RemoteInterface.MFCC.VD1MinCycleEventData Vd1Min = (RemoteInterface.MFCC.VD1MinCycleEventData)objEvent.EventObj;
        //    System.Data.DataSet ds = data.orgds;
        //    ds.AcceptChanges();
        //    String type = ds.Tables[0].Rows[0]["response_type"].ToString().Trim().ToUpper();
        //    String response_type = String.Empty;
        //    if (type == "0")
        //        response_type = "1";
        //    else
        //        response_type = "0";

        //    //string TIMESTAMP = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" +
        //    //                   ((ds.Tables[0].Rows[0]["day"].ToString().Length == 2) ? ds.Tables[0].Rows[0]["day"].ToString() : "0" + ds.Tables[0].Rows[0]["day"].ToString()) + " " +
        //    //                   ((ds.Tables[0].Rows[0]["hour"].ToString().Length == 2) ? ds.Tables[0].Rows[0]["hour"].ToString() : "0" + ds.Tables[0].Rows[0]["hour"].ToString()) + ":" +
        //    //                   ((ds.Tables[0].Rows[0]["minute"].ToString().Length == 2) ? ds.Tables[0].Rows[0]["minute"].ToString() : "0" + ds.Tables[0].Rows[0]["minute"].ToString()) + ":00";
        //    string TIMESTAMP = "TIMESTAMP('" + Comm.DB2.Db2.getTimeStampString(data.dateTime) + "')";
        //    int lane_count = data.orgVDLaneData.Length;// Convert.ToInt32(ds.Tables[0].Rows[0]["lane_count"]);
        //    String[,] myString = new String[14, 7];

        //    for (int i = 0; i < lane_count; i++)
        //    {

        //        myString[0, i] = data.orgVDLaneData[i].small_car_volume.ToString();          // Convert.ToString(ds.Tables[1].Rows[i]["small_car_volume"]);
        //        myString[1, i] = data.orgVDLaneData[i].big_car_volume.ToString();    //Convert.ToString(ds.Tables[1].Rows[i]["big_car_volume"]);
        //        myString[2, i] = data.orgVDLaneData[i].connect_car_volume.ToString();//Convert.ToString(ds.Tables[1].Rows[i]["connect_car_volume"]);
        //        myString[3, i] = "-1";
        //        myString[4, i] = data.orgVDLaneData[i].small_car_speed.ToString(); //Convert.ToString(ds.Tables[1].Rows[i]["small_car_speed"]);
        //        myString[5, i] = data.orgVDLaneData[i].big_car_speed.ToString(); //Convert.ToString(ds.Tables[1].Rows[i]["big_car_speed"]);
        //        myString[6, i] = data.orgVDLaneData[i].connect_car_speed.ToString();//Convert.ToString(ds.Tables[1].Rows[i]["connect_car_speed"]);
        //        myString[7, i] = "-1";
        //        myString[8, i] = data.orgVDLaneData[i].small_car_length.ToString();  // Convert.ToString(ds.Tables[1].Rows[i]["small_car_length"]);
        //        myString[9, i] = data.orgVDLaneData[i].big_car_length.ToString(); //Convert.ToString(ds.Tables[1].Rows[i]["big_car_length"]);
        //        myString[10, i] = data.orgVDLaneData[i].connect_car_length.ToString();// Convert.ToString(ds.Tables[1].Rows[i]["connect_car_length"]);
        //        myString[11, i] = "-1";
        //        myString[12, i] = data.orgVDLaneData[i].interval.ToString();//Convert.ToString(ds.Tables[1].Rows[i]["average_car_interval"]);
        //        myString[13, i] = data.orgVDLaneData[i].occupancy.ToString();//Convert.ToString(ds.Tables[1].Rows[i]["average_occupancy"]);
        //    }

        //    String second20 = "";
        //    second20 += "'" + devName + "'," + TIMESTAMP + ((data.IsValid) ? ",'V'," : ",'I',") + response_type + ",";
        //    for (int i = 0; i < 14; i++)
        //        for (int j = 0; j < 7; j++)
        //        {
        //            if (myString[i, j] != null)
        //                second20 += myString[i, j] + ",";
        //            else
        //                second20 += "-1,";
        //        }
        //    second20 = second20.Substring(0, second20.Length - 1);
        //    return second20;
        //}

        //void nclient_OnConnect()
        //{
            
        //}


        //public override void loadTC_AndBuildManaer()
        //{
        ////    try
        ////    {
        ////        //System.Data.Odbc.OdbcDataReader rd;
        ////        //System.Data.Odbc.OdbcConnection cn = new System.Data.Odbc.OdbcConnection(Comm.DB2.Db2.db2ConnectionStr);


        ////        //rd = Comm.DB2.Db2.getDeviceConfigReader(cn, this.mfccid);


        ////        //while (rd.Read())
        ////        //{
        ////        //    byte[] hw_status = new byte[4];
        ////        //    for (int i = 0; i < 4; i++)
        ////        //        hw_status[i] = System.Convert.ToByte(rd[3 + i]);

        ////        //    Comm.TC.VDTC tc = new Comm.TC.VDTC(protocol, rd[0].ToString().Trim(), rd[1].ToString(), (int)rd[2], 0xffff, hw_status);
        ////        //    ConsoleServer.WriteLine(string.Format("load tc:{0} ip:{1} port:{2}", rd[0], rd[1], rd[2]));
        ////        //    tcAry.Add(tc);

        ////        //}
        ////        //rd.Close();
        ////        //cn.Close();

        //        Comm.TC.VDTC tc=new Comm.TC.VDTC(protocol,"VD231","192.168.22.231",1001,0xffff,new byte[]{0,0,0,0});
        //        tc.OnRealTimeData += new Comm.TC.OnRealTimeEventHandler(tc_OnRealTimeData);
        //        tc.OnTriggerEvent += new Comm.TC.OnTriggerEventHandler(tc_OnTriggerEvent);
        //     //   tc.OnHwStatusChanged += new HWStatusChangeHandler(tc_OnHwStatusChanged);
        //      //  tc.OnConnectStatusChanged += new ConnectStatusChangeHandler(tc_OnConnectStatusChanged);
        //        tc.On1MinTrafficData += new Comm.TC.On1MinEventDataHandler(tc_On1MinTrafficData);
        //        tcAry.Add(tc);

        //        this.manager = new TC_Manager(tcAry);

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    ConsoleServer.WriteLine(ex.Message);
        //    //}


        //}

        //void tc_On1MinTrafficData(object vdtc, Comm.TC.VD_MinAvgData data)
        //{
           
            //try
            //{
            //    //   this.ToDb(data.getExecuteSql());

            //    if (data.IsValid)
            //        dbServer.SendSqlCmd(data.getExecuteSql());
            //    else
            //    {
            //        if (r_host_comm != null)
            //            r_host_comm.DoVD_InteropData((vdtc as TCBase).DeviceName, data.dateTime);
            //    }

            //    RemoteInterface.MFCC.VD1MinCycleEventData vd1min;

            //    //if (((TCBase)vdtc).DeviceName == "VD-N1-S-149.5")
            //    //{
            //    vd1min = data.ToVD1MinCycleEventData();
            //    //}
            //    //else
            //    //{
            //    //   vd1min = data.ToVD1MinCycleEventData();
            //    //}
            //    this.notifier.NotifyAll(new NotifyEventObject(EventEnumType.VD_1min_Cycle_Event, ((TCBase)vdtc).DeviceName, vd1min));
            //}
            //catch (Exception ex)
            //{
            //    ConsoleServer.WriteLine(ex.Message + ex.StackTrace);
            //}





        //}

        //void tc_OnConnectStatusChanged(object tcc)
        //{
        //    Comm.TCBase tc = (Comm.TCBase)tcc;
        //    notifier.NotifyAll(new NotifyEventObject(EventEnumType.Connection_Event, tc.DeviceName, tc.IsConnected));

        //}



        //void tc_OnTriggerEvent(object tilttc, DataSet ds)
        //{
        //    //try
        //    //{
        //    //    Comm.TC.TILTTC tc = (Comm.TC.TILTTC)tilttc;

        //    //    string sql = "insert into  tblVdDataActuated (devicename,timestamp,response_type,lane_id,occ_time) values('{0}','{1}',{2},{3},{4})";

        //    //    int day = System.Convert.ToInt32(ds.Tables[0].Rows[0]["day"]);
        //    //    int hour = System.Convert.ToInt32(ds.Tables[0].Rows[0]["hour"]);
        //    //    int minute = System.Convert.ToInt32(ds.Tables[0].Rows[0]["minute"]);
        //    //    int second = System.Convert.ToInt32(ds.Tables[0].Rows[0]["second"]);
        //    //    System.DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day, hour, minute, second);
        //    //    int response_type = System.Convert.ToInt32(ds.Tables[0].Rows[0]["response_type"]);
        //    //    int lane_id = System.Convert.ToInt32(ds.Tables[0].Rows[0]["lane_id"]);
        //    //    int occ_time = System.Convert.ToInt32(ds.Tables[0].Rows[0]["occ_time"]);

        //    //    dbServer.SendSqlCmd(string.Format(sql, tc.DeviceName, Comm.DB2.Db2.getTimeStampString(dt), response_type, lane_id, occ_time));

        //    //    notifier.NotifyAll(new NotifyEventObject(EventEnumType.VD_Trig_Event, tc.DeviceName, ds));
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    ConsoleServer.WriteLine("In tc_on trigger Event" + ex.Message);
        //    //}


            
        //}

        //void tc_OnRealTimeData(object TILTTC, DataSet ds)
        //{
        //    //Comm.TC.TILTTC tc = (Comm.TC.TILTTC)TILTTC;
        //    //// int num;
        //    //ds.AcceptChanges();
        //    //for (int i = 0; i < Convert.ToInt32(ds.Tables[0].Rows[0]["car_volume"]); i++)
        //    //    this.dbServer.SendSqlCmd(new Comm.TC.VD_SpotSpeedData(tc.DeviceName, Convert.ToInt32(ds.Tables[0].Rows[0]["lane_id"]), ds.Tables[1].Rows[i]).getExecuteSql());

        //    //ds.AcceptChanges();
        //    //notifier.NotifyAll(new NotifyEventObject(EventEnumType.VD_Real_Data_Event, tc.DeviceName, ds));


        //    // 寫入資料庫 

        //}






        //public override Comm.MFCC.TC_Manager getTcManager()
        //{
        //    return (TC_Manager)this.vd_manager;
        //    //throw new Exception("The method or operation is not implemented.");
        //}


    }

    //class StRepairData
    //{
    //    public DateTime dt;
    //    public string devName;
    //    public StRepairData(DateTime dt, string devName)
    //    {
    //        this.dt = dt;
    //        this.devName = devName;

    //    }
    //}
 