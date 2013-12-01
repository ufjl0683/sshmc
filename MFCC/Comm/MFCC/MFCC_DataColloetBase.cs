using System;
using System.Collections.Generic;
using System.Text;
using RemoteInterface;
using System.Data;

namespace Comm.MFCC
{
   public abstract class MFCC_DataColloetBase : MFCC_Base
    {

     protected  System.Collections.Queue InDbQueue = System.Collections.Queue.Synchronized(new System.Collections.Queue());
      public  MFCC_DataColloetBase(string mfccid, string devType, int remotePort, int notifyPort, int consolePort, string regRemoteName, Type regRemoteType)
           : base(mfccid, devType, remotePort, notifyPort, consolePort, regRemoteName, regRemoteType)
       {
        
           new System.Threading.Thread(DataRepairTask).Start();
       }

      public override void BindEvent(object tc)
      {
          //row new NotImplementedException();
          Comm.TCBase tcbase = tc as TCBase;
          tcbase.OnTCReport += new OnTCReportHandler(tcbase_OnTCReport);
      }

      void tcbase_OnTCReport(object tc, TextPackage txt)
      {

         
          if (txt.Text[0] == 0x29) //cycle report
          {
            DataSet ds  = this.protocol.GetSendDsByTextPackage(txt, CmdType.CmdReport);
            int day, hour, min;
            day =System.Convert.ToInt32( ds.Tables[0].Rows[0]["day"]);
            hour = System.Convert.ToInt32(ds.Tables[0].Rows[0]["hour"]);
            min = System.Convert.ToInt32(ds.Tables[0].Rows[0]["minute"]);
            int sensor_cnt = System.Convert.ToInt32(ds.Tables[0].Rows[0]["sensor_cnt"]);
            int value_cnt= System.Convert.ToInt32(ds.Tables[0].Rows[0]["value_cnt"]);
            DateTime timestamp=new DateTime(DateTime.Now.Year,DateTime.Now.Month,day,hour,min,0);
          //  int maxdegree = 0;
            for (int i = 0; i < sensor_cnt; i++)
            {
                double value0, value1, value2;
                value0 =Comm.V2DLE.uLongToDouble( System.Convert.ToUInt64( ds.Tables[1].Rows[i]["value0"]));
                value1 = Comm.V2DLE.uLongToDouble(System.Convert.ToUInt64(ds.Tables[1].Rows[i]["value1"]));
                value2 = Comm.V2DLE.uLongToDouble(System.Convert.ToUInt64(ds.Tables[1].Rows[i]["value2"]));
                int degree = System.Convert.ToInt32(ds.Tables[1].Rows[i]["degree"]);
               
                string isvalid=(System.Convert.ToInt32(ds.Tables[1].Rows[i]["is_valid"])==1)?"Y":"N";
                int execution_mode = 0;
                string sql = "insert into tbltc10mindatalog values({0},'{1}',{2},{3},{4},{5},'{6}',{7},{8},{9},'{10}')";
                dbServer.SendSqlCmd(
                    string.Format(sql, (tc as TCBase).sensor_ids[i], DbCmdServer.getTimeStampString(timestamp), value0, value1, value2, degree, isvalid, execution_mode, (tc as TCBase).DeviceName, value_cnt,'N'));

                sql=string.Format("update tblsensor set current_degree={0},value0={1},value1={2},value2={3}  ,ISVALID='{5}' where sensor_id={4}",degree,value0,value1,value2,(tc as TCBase).sensor_ids[i],isvalid);

                

                dbServer.SendSqlCmd(sql);

                sql = string.Format("update tblTC_Restore set trycnt=trycnt+1,isfinsh='Y' where controller_id={0} and timestamp='{1}'", (tc as TCBase).DeviceName, DbCmdServer.getTimeStampString(timestamp));
                dbServer.SendSqlCmd(sql);
                if (r_host_comm != null)
                {
                    try
                    {
                        r_host_comm.SetSensorValueDegree(System.Convert.ToInt32((tc as TCBase).sensor_ids[i]), value0, value1, value2, degree);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + "," + ex.StackTrace);
                    }
                }
            }

          
              
              Console.WriteLine("cyclic report!");
          }
       
      }

      void DataRepairTask()
      {

          System.Collections.ArrayList aryThread = new System.Collections.ArrayList();

          System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Comm.SQL.SQL.dbConnectionStr);
          System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
          cmd.CommandTimeout = 120;
          StRepairData rpd = null;//=new StRepairData();

          cmd.Connection = cn;

          System.Collections.Queue queue = new System.Collections.Queue();

          int dayinx = 1;
          while (true)
          {
              Comm.TCBase tc;

              if (!IsLoadTcCompleted)
              {
                  System.Threading.Thread.Sleep(1000);
                  continue;
              }

              try
              {

                  while (this.dbServer.getCurrentQueueCnt() > 50)
                      System.Threading.Thread.Sleep(1000 * 10);
                  cn.Open();
                  ConsoleServer.WriteLine("Repair task begin!");
                  cmd.Connection = cn;

                  //   string sqlGetRepair = "select t1.DEVICENAME, TIMESTAMP ,trycnt,datavalidity,comm_state from TBLVDDATA1MIN  t1 inner join tblDeviceConfig t2 on t1.devicename=t2.devicename where mfccid='{0}' and  TIMESTAMP between '{1}' and '{2}' and trycnt <1 and datavalidity='N' and comm_state=1  and enable='Y'  order by timestamp desc  fetch first 300  row only ";
                  string sqlGetRepair = "select top 300 t1.controller_id, TIMESTAMP ,trycnt  from tbltc_restore t1 inner join tbltc t2 on t1.controller_id=t2.controller_id where mfcc_id='{0}' and  TIMESTAMP between '{1}' and '{2}' and trycnt <1 and Isfinsh='N' and isconnected='Y'  and enable='Y'  order by timestamp desc    ";
                  cmd.CommandText = string.Format(sqlGetRepair, mfccid, Comm.SQL.SQL.getTimeStampString(System.DateTime.Now.AddDays(-dayinx)), Comm.SQL.SQL.getTimeStampString(System.DateTime.Now.AddDays(-dayinx + 1).AddMinutes(-10)));
                  System.Data.SqlClient.SqlDataReader rd = cmd.ExecuteReader();

                  while (rd.Read())
                  {
                      string devName = "";
                      DateTime dt;
                      devName = rd[0].ToString();
                      dt = System.Convert.ToDateTime(rd[1]);
                      queue.Enqueue(new StRepairData(dt, devName));

                  }
                  rd.Close();

                  ConsoleServer.WriteLine("total:" + queue.Count + " to repair!");
                  if (queue.Count < 300)
                  {
                      dayinx++;
                      if (dayinx == 3)
                          dayinx = 1;
                  }
                  if (queue.Count == 0)
                      System.Threading.Thread.Sleep(1000 * 60);

                  aryThread.Clear();
                  while (queue.Count != 0)
                  {
                      try
                      {

                          rpd = (StRepairData)queue.Dequeue();
                          if (this.manager.IsContains(rpd.devName))
                              tc = (Comm.TCBase)this.manager[rpd.devName];
                          else


                              continue;


                          if (!tc.IsConnected)
                          {
                             dbServer.SendSqlCmd(string.Format("update tblTC  set Isconnected='N' where controller_id={0} ", rpd.devName));

                              continue;
                          }

                          System.Threading.Thread th = new System.Threading.Thread(Repair_job);
                          aryThread.Add(th);
                          th.Start(rpd);

                          if (aryThread.Count >= 5)
                          {
                              for (int i = 0; i < aryThread.Count; i++)


                                  ((System.Threading.Thread)aryThread[i]).Join();


                              aryThread.Clear();
                          }


                          //   ConsoleServer.WriteLine("==>repair:" + rpd.devName + "," + rpd.dt.ToString());

                      }
                      catch (Exception ex)
                      {
                          ConsoleServer.WriteLine(ex.Message + ex.StackTrace);

                      }
                  }


                  for (int i = 0; i < aryThread.Count; i++)


                      ((System.Threading.Thread)aryThread[i]).Join();


                  aryThread.Clear();





              }
              catch (Exception x)
              {
                  ConsoleServer.WriteLine(x.Message + x.StackTrace);
              }
              finally
              {
                  try
                  {
                      cn.Close();
                  }
                  catch { ;}
              }


          }


      }

      public void Repair_job(System.Object repairinfo)
      {
          Comm.TCBase tc;
          StRepairData RepairInfo = repairinfo as StRepairData;
          try
          {
              tc = (Comm.TCBase)this.manager[RepairInfo.devName];

              //  Comm.TC.VD_MinAvgData data = tc.getOneMinAvgData(tc.Tc_GetVDData(RepairInfo.dt), RepairInfo.dt.Year, RepairInfo.dt.Month);
              //  dbServer.SendSqlCmd(data.get1minUpdateSql());

              DataSet ds = protocol.GetSendDataSet("get_cycle_data");
              ds.Tables[0].Rows[0]["day"] = RepairInfo.dt.Day;
              ds.Tables[0].Rows[0]["hour"] = RepairInfo.dt.Hour;
              ds.Tables[0].Rows[0]["minute"] = RepairInfo.dt.Minute;
              SendPackage pkg = protocol.GetSendPackage(ds, 0xffff);
              tc.Send(pkg);
              Console.WriteLine(tc.DeviceName);
              if (pkg.result == CmdResult.ACK && pkg.ReturnTextPackage != null)
              {
                  ds = protocol.GetReturnDsByTextPackage(pkg.ReturnTextPackage);

                  int day, hour, min;
                  day = System.Convert.ToInt32(ds.Tables[0].Rows[0]["day"]);
                  hour = System.Convert.ToInt32(ds.Tables[0].Rows[0]["hour"]);
                  min = System.Convert.ToInt32(ds.Tables[0].Rows[0]["minute"]);
                  int sensor_cnt = System.Convert.ToInt32(ds.Tables[0].Rows[0]["sensor_cnt"]);
                  int value_cnt = System.Convert.ToInt32(ds.Tables[0].Rows[0]["value_cnt"]);
                  DateTime timestamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day, hour, min, 0);
                  //  int maxdegree = 0;
                  //if (sensor_cnt == 0)
                  //{
                  //    this.get
                  //    string sql = "insert into tbltc10mindatalog values({0},'{1}',{2},{3},{4},{5},'{6}',{7},{8},{9},'{10}')";
                  //    //dbServer.SendSqlCmd(
                  //    //    string.Format(sql, (tc as TCBase).sensor_ids[i], DbCmdServer.getTimeStampString(timestamp), 0, 0, 0, 0, isvalid, execution_mode, (tc as TCBase).DeviceName, value_cnt, 'N'));

                  //}
                  
                  for (int i = 0; i < sensor_cnt; i++)
                  {
                      double value0, value1, value2;
                      value0 = Comm.V2DLE.uLongToDouble(System.Convert.ToUInt64(ds.Tables[1].Rows[i]["value0"]));
                      value1 = Comm.V2DLE.uLongToDouble(System.Convert.ToUInt64(ds.Tables[1].Rows[i]["value1"]));
                      value2 = Comm.V2DLE.uLongToDouble(System.Convert.ToUInt64(ds.Tables[1].Rows[i]["value2"]));
                      int degree = System.Convert.ToInt32(ds.Tables[1].Rows[i]["degree"]);

                      string isvalid = (System.Convert.ToInt32(ds.Tables[1].Rows[i]["is_valid"]) == 1) ? "Y" : "N";
                      int execution_mode = 0;
                      string sql = "insert into tbltc10mindatalog values({0},'{1}',{2},{3},{4},{5},'{6}',{7},{8},{9},'{10}')";
                      dbServer.SendSqlCmd(
                          string.Format(sql, (tc as TCBase).sensor_ids[i], DbCmdServer.getTimeStampString(timestamp), value0, value1, value2, degree, isvalid, execution_mode, (tc as TCBase).DeviceName, value_cnt, 'Y'));

                   //   sql = string.Format("update tblsensor set current_degree={0},value0={1},value1={2},value2={3} where sensor_id={4}", degree, value0, value1, value2, (tc as TCBase).sensor_ids[i]);



                   //   dbServer.SendSqlCmd(sql);

                     

                  }

                  string RestoreSQL = string.Format("update tblTC_Restore set trycnt=trycnt+1,isfinsh='Y' where controller_id={0} and timestamp='{1}'", (tc as TCBase).DeviceName, DbCmdServer.getTimeStampString(timestamp));
                  dbServer.SendSqlCmd(RestoreSQL);








                  ConsoleServer.WriteLine("==>repair:" + RepairInfo.devName + "," + RepairInfo.dt.ToString());
              }

          }
          catch (Exception ex)
          {
              try
              {
                  dbServer.SendSqlCmd(string.Format("update tbltc_restore set trycnt=trycnt+1 where controller_id={0} and  timestamp='{1}'", RepairInfo.devName, Comm.SQL.SQL.getTimeStampString(RepairInfo.dt)));
              }

              catch (Exception ee)
              {
                  ConsoleServer.WriteLine(ee.Message + ee.StackTrace);
              }
              ConsoleServer.WriteLine(ex.Message + "," + ex.StackTrace);
          }
      }
       //void WriteDbTask()
       //{
       //    System.Data.Odbc.OdbcConnection cn;
       //    cn = new System.Data.Odbc.OdbcConnection(Comm.DB2.Db2.db2ConnectionStr);
       //    System.Data.Odbc.OdbcCommand cmd = new System.Data.Odbc.OdbcCommand();
       //    cmd.Connection = cn;

       //    while (true)
       //    {


       //        try
       //        {

       //            lock (InDbQueue)
       //            {
       //                if (InDbQueue.Count == 0)
       //                    System.Threading.Monitor.Wait(InDbQueue);
       //            }

       //            cn.Open();
       //            while (InDbQueue.Count != 0)
       //            {
       //              //  Comm.I_DB_able data = (Comm.I_DB_able)InDbQueue.Dequeue();
       //                string data = InDbQueue.Dequeue().ToString();
       //                cmd.CommandText = data; // data.getExecuteSql();
       //                cmd.ExecuteNonQuery();


       //            }


       //        }
       //        catch (Exception ex)
       //        {
       //            ConsoleServer.WriteLine(ex.Message + ex.StackTrace);
       //        }
       //        finally
       //        {
       //            try
       //            {
       //                cn.Close();
       //            }
       //            catch { ;}

       //        }
       //        ConsoleServer.WriteLine("One min vd data to db completed!!");

       //    }  //while


       //}
       //protected void ToDb(string sqlstr)
       //{
       //    this.InDbQueue.Enqueue(sqlstr);
       //    lock (this.InDbQueue)
       //    {
       //        System.Threading.Monitor.PulseAll(InDbQueue);
       //    }

       //}

    }

   class StRepairData
   {
       public DateTime dt;
       public string devName;
       public StRepairData(DateTime dt, string devName)
       {
           this.dt = dt;
           this.devName = devName;

       }
   }
}
