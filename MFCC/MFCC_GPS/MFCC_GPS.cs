using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using RemoteInterface;
using System.Data;
using Comm;

namespace MFCC_GPS
{
    class MFCC_GPS : Comm.MFCC.MFCC_DataColloetBase
    {




        public MFCC_GPS(string mfccid, string devType, int remotePort, int notifyPort, int consolePort, string regRemoteName, Type regRemoteType)
            : base(mfccid, devType, remotePort, notifyPort, consolePort, regRemoteName, regRemoteType)
        {


       


        }



        void CommtestTask()
        {

            Comm.TC.TILTTC tc = (Comm.TC.TILTTC)this.getTcManager()["VD231"];
            while (true)
            {
                try
                {
                    if (tc.IsConnected)
                    {
                        // tc.getHwStaus();
                        tc.TC_GetHW_Status();
                     //   tc.Tc_GetVDData(System.DateTime.Now);

                        //      Console.WriteLine( tc.m_device.getQueueState());
                        //   System.Threading.Thread.Sleep(3000);

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ex.StackTrace);
                }


            }

        }


   



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

        void nclient_OnConnect()
        {
            
        }


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



        void tc_OnTriggerEvent(object tilttc, DataSet ds)
        {
            //try
            //{
            //    Comm.TC.TILTTC tc = (Comm.TC.TILTTC)tilttc;

            //    string sql = "insert into  tblVdDataActuated (devicename,timestamp,response_type,lane_id,occ_time) values('{0}','{1}',{2},{3},{4})";

            //    int day = System.Convert.ToInt32(ds.Tables[0].Rows[0]["day"]);
            //    int hour = System.Convert.ToInt32(ds.Tables[0].Rows[0]["hour"]);
            //    int minute = System.Convert.ToInt32(ds.Tables[0].Rows[0]["minute"]);
            //    int second = System.Convert.ToInt32(ds.Tables[0].Rows[0]["second"]);
            //    System.DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day, hour, minute, second);
            //    int response_type = System.Convert.ToInt32(ds.Tables[0].Rows[0]["response_type"]);
            //    int lane_id = System.Convert.ToInt32(ds.Tables[0].Rows[0]["lane_id"]);
            //    int occ_time = System.Convert.ToInt32(ds.Tables[0].Rows[0]["occ_time"]);

            //    dbServer.SendSqlCmd(string.Format(sql, tc.DeviceName, Comm.DB2.Db2.getTimeStampString(dt), response_type, lane_id, occ_time));

            //    notifier.NotifyAll(new NotifyEventObject(EventEnumType.VD_Trig_Event, tc.DeviceName, ds));
            //}
            //catch (Exception ex)
            //{
            //    ConsoleServer.WriteLine("In tc_on trigger Event" + ex.Message);
            //}


            
        }

        void tc_OnRealTimeData(object TILTTC, DataSet ds)
        {
            //Comm.TC.TILTTC tc = (Comm.TC.TILTTC)TILTTC;
            //// int num;
            //ds.AcceptChanges();
            //for (int i = 0; i < Convert.ToInt32(ds.Tables[0].Rows[0]["car_volume"]); i++)
            //    this.dbServer.SendSqlCmd(new Comm.TC.VD_SpotSpeedData(tc.DeviceName, Convert.ToInt32(ds.Tables[0].Rows[0]["lane_id"]), ds.Tables[1].Rows[i]).getExecuteSql());

            //ds.AcceptChanges();
            //notifier.NotifyAll(new NotifyEventObject(EventEnumType.VD_Real_Data_Event, tc.DeviceName, ds));


            // 寫入資料庫 

        }






        //public override Comm.MFCC.TC_Manager getTcManager()
        //{
        //    return (TC_Manager)this.vd_manager;
        //    //throw new Exception("The method or operation is not implemented.");
        //}


    }

   
}
