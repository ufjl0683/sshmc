using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using RemoteInterface;
using System.Data.SqlClient;
using Host.TC;

namespace Host 
{
    public class DevcieManager
    {

        MFCC.MFCC_Manager mfccmgr;
        public bool IsInLoadWrapper = false;
        //   OdbcConnection cnDevice;
        System.Collections.Hashtable devices = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
        public DevcieManager(MFCC.MFCC_Manager mfccmgr)
        {


            this.mfccmgr = mfccmgr;
            IsInLoadWrapper = true;

            loadAllDeviceWrapper();

            IsInLoadWrapper = false;
        }


        public DeviceBaseWrapper this[string deviceName]
        {
            get
            {
                deviceName = deviceName.Trim();
                if (devices.Contains(deviceName))
                    return (DeviceBaseWrapper)devices[deviceName];
                else
                {
                    if (IsInLoadWrapper)
                        throw new Exception("設備管理啟動中....請稍後再試!");
                    else
                        throw new Exception(deviceName + ",not found!");
                }
            }
        }

        public bool IsContainDevice(string devname)
        {

            return devices.Contains(devname);
        }

        public System.Collections.IEnumerable getOutputDeviceEnum()
        {
            throw new NotImplementedException();

            //System.Collections.IEnumerator ie = this.devices.GetEnumerator();
            //while (ie.MoveNext())
            //{
            //    if (((System.Collections.DictionaryEntry)ie.Current).Value is TC.OutPutDeviceBase)
            //        yield return ((System.Collections.DictionaryEntry)ie.Current).Value;
            //}

        }


        public System.Collections.IEnumerable getAllDeviceEnum()
        {

            System.Collections.IEnumerator ie = this.devices.GetEnumerator();
            while (ie.MoveNext())
            {
                // if (!(((System.Collections.DictionaryEntry)ie.Current).Value is TC.OutPutDeviceBase))
                yield return ((System.Collections.DictionaryEntry)ie.Current).Value;
            }

        }


        public System.Collections.IEnumerable getDataDeviceEnum()
        {
          // throw new NotImplementedException();
            System.Collections.IEnumerator ie = this.devices.GetEnumerator();
            while (ie.MoveNext())
            {
                if ((((System.Collections.DictionaryEntry)ie.Current).Value is TC.DataDeviceBaseWrapper))
                    yield return ((System.Collections.DictionaryEntry)ie.Current).Value;
            }

        }
        public RemoteInterface.MFCC.I_MFCC_Base getRemoteObject(string deviceName)
        {
            if (mfccmgr[((TC.DeviceBaseWrapper)this[deviceName]).mfccid] == null)
                return null;
            else
                return ((MFCC.MFCC_Object)mfccmgr[((TC.DeviceBaseWrapper)this[deviceName]).mfccid]).getRemoteObject();
        }



        private void loadAllDeviceWrapper()
        {
            System.Collections.ArrayList threadAry = new System.Collections.ArrayList();
            SqlConnection cn = new SqlConnection(DbCmdServer.getSSHMC_DbConnectStr());
           SqlCommand cmd = new SqlCommand("select distinct mfcc_id from tblTC", cn);
            SqlDataReader rd;
            //  System.Collections.ArrayList TMPARY = new System.Collections.ArrayList();
            // System.Threading.ThreadPool.SetMaxThreads(300, 300);
            try
            {
                cn.Open();
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    string devType = rd[0].ToString().Trim();


                    System.Threading.Thread th = new System.Threading.Thread(loadDeviceWrapper);
                    th.Name = devType;
                    th.Start(devType);
                    threadAry.Add(th);

                 //   loadDeviceWrapper(devType);
                  

                }
                rd.Close();

                foreach (System.Threading.Thread t in threadAry)
                {
                    System.Console.WriteLine("Waitting " + t.Name);
                    t.Join();
                    //   TMPARY.Remove(t.Name);
                }
                Console.WriteLine("\n[Device Manger Started!]\n");


            }
            catch (Exception ex)
            {
                ConsoleServer.WriteLine(ex.Message + "," + ex.StackTrace);
            }
            finally
            {

                cn.Close();
            }

        }



        private void loadDeviceWrapper(object mfccid)
        {
            SqlConnection cn;
            SqlCommand cmd;
            SqlDataReader rd = null;
            cn = new SqlConnection(DbCmdServer.getSSHMC_DbConnectStr());
            cmd = new SqlCommand();
            cmd.Connection = cn;
            try
            {
                //     Console.WriteLine("load " + mfccid.ToString() + "....");
                cn.Open();
#if !DEBUG
                cmd.CommandText = "select controller_id as DeviceName from tblTC where mfcc_id='" + mfccid.ToString() + "'";
#else
              cmd.CommandText = "select devicename from tbldeviceconfig where    lineid='N1'    and mfccid='" + mfccid.ToString() + "'  ";
             // cmd.CommandText = "select devicename from tbldeviceconfig where enable='Y' and mfccid='" + mfccid.ToString() + "'";
#endif
                rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    string devicename = rd[0].ToString();

                    this.AddDeviceWrapper(devicename, false, cn);

                    ConsoleServer.Write(".");
                    /*
                    string devicetype = rd[1] as string;
                    string mfccid = rd[2] as string;
                    string location = rd[3] as string;
                    string lineid = rd[4] as string;
                    string direction = rd[5] as string;
                    int mile_m = System.Convert.ToInt32(rd[6]);
                    string ip = rd[7] as string;
                    int port = System.Convert.ToInt32(rd[8]);

                    byte[] hwstatus = new byte[4];
                    hwstatus[0]=System.Convert.ToByte(rd[9]);
                    hwstatus[1] = System.Convert.ToByte(rd[10]);
                    hwstatus[2] = System.Convert.ToByte(rd[11]);
                    hwstatus[3] = System.Convert.ToByte(rd[12]);
                    byte opmode = System.Convert.ToByte(rd[13]);
                    byte opstatus = System.Convert.ToByte(rd[14]); 

                    if (devicetype == "VD")
                    {
                        TC.VDDeviceWrapper wrapper = new TC.VDDeviceWrapper(mfccid, devicename, devicetype, ip, port, location,lineid, mile_m,hwstatus,opmode,opstatus);
                        devices.Add(devicename,wrapper);
                    }
                    else if (devicetype == "RGS")
                    {
                        TC.RGSDeviceWrapper wrapper = new TC.RGSDeviceWrapper(mfccid, devicename, devicetype, ip, port, location,lineid, mile_m,hwstatus,opmode,opstatus);
                        devices.Add(devicename, wrapper);
                    }
                    else if (devicetype == "CMS")
                    {
                    
                        TC.CMSDeviceWrapper wrapper = new TC.CMSDeviceWrapper(mfccid, devicename, devicetype, ip, port,location, lineid, mile_m,hwstatus,opmode,opstatus);
                        //devices.Add(devicename, new TC.CMSDeviceWrapper(mfccid, devicename, devicetype, ip, port, location, lineid, mile_m));
                        devices.Add(devicename, wrapper);
                    }
                    else if (devicetype == "RMS")
                    {
                        TC.RMSDeviceWrapper wrapper = new TC.RMSDeviceWrapper(mfccid, devicename, devicetype, ip, port, location, lineid, mile_m,hwstatus,opmode,opstatus);
                      //  devices.Add(devicename, new TC.RMSDeviceWrapper(mfccid, devicename, devicetype, ip, port, location, lineid, mile_m));
                        devices.Add(devicename, wrapper);
                    }
                    else if (devicetype == "WIS")
                    {
                        TC.WISDeviceWrapper wrapper = new TC.WISDeviceWrapper(mfccid, devicename, devicetype, ip, port, location, lineid, mile_m,hwstatus,opmode,opstatus);
                       // devices.Add(devicename, new TC.WISDeviceWrapper(mfccid, devicename, devicetype, ip, port, location, lineid, mile_m));
                        devices.Add(devicename, wrapper);
                    }
                    else if (devicetype == "LCS")
                    {
                        TC.LCSDeviceWrapper wrapper = new TC.LCSDeviceWrapper(mfccid, devicename, devicetype, ip, port, location, lineid, mile_m,hwstatus,opmode,opstatus);
                       // devices.Add(devicename, new TC.LCSDeviceWrapper(mfccid, devicename, devicetype, ip, port, location, lineid, mile_m));
                        devices.Add(devicename, wrapper);
                    }
                    else if (devicetype == "CSLS")
                    {
                        TC.CSLSDeviceWrapper wrapper = new TC.CSLSDeviceWrapper(mfccid, devicename, devicetype, ip, port, location, lineid, mile_m,hwstatus,opmode,opstatus);
                    //    devices.Add(devicename, new TC.CSLSDeviceWrapper(mfccid, devicename, devicetype, ip, port, location, lineid, mile_m));
                        devices.Add(devicename, wrapper);
                    }
                     * */





                }

            }
            catch (Exception ex)
            {
                ConsoleServer.WriteLine(ex.Message + ex.StackTrace);
                System.Environment.Exit(-1);
            }
            finally
            {
                try
                {
                    rd.Close();
                    rd.Dispose();
                    cn.Close();
                }
                catch { ;}

            }
            //    Console.WriteLine("load " + mfccid.ToString() + "finish!");
        }

        public void RemoveDeviceWrapper(string devName)
        {
            if (!devices.ContainsKey(devName))



                throw new Exception(devName + " not in list!");

            DeviceBaseWrapper wrapper = (DeviceBaseWrapper)devices[devName];
            wrapper.getRemoteObj().Remove(wrapper.deviceName);

            devices.Remove(devName);


        }


        public void AddDeviceWrapper(string devName, SqlConnection cn)
        {
            AddDeviceWrapper(devName, false, cn);
        }



        public void AddDeviceWrapper(string devName, bool isMfccAddDevice, SqlConnection cnn)
        {
           SqlDataReader rd = null;
           SqlConnection cn = null; ;
            SqlCommand cmd;
            try
            {

                devName = devName.Trim();
                //if (isMfccAddDevice)
                    cn = new SqlConnection(DbCmdServer.getSSHMC_DbConnectStr());
                //else
                 //   cn = cnn;

                cmd = new SqlCommand();
                cmd.Connection = cn;
                if (this.devices.ContainsKey(devName))
                    throw new Exception(devName + "already in list");

              //  if (isMfccAddDevice)
                    cn.Open();
                // if (devName.StartsWith("VD"))
                cmd.CommandText = "select controller_id as devicename,device_type,mfcc_id,ip,port,hw_status_1,hw_status_2,hw_status_3,hw_status_4 from tbltc  where  controller_id=" + devName;
               rd = cmd.ExecuteReader();

                if (!rd.Read())
                    throw new Exception(devName + " not  found in database!");
                else
                {

                    string devicename =rd[0].ToString();
                    string devicetype = rd[1] as string;
                    string mfccid = rd[2] as string;
                    //string location = rd[3] as string;
                    //string lineid = rd[4] as string;
                    //string direction = rd[5] as string;



                    //int mile_m = System.Convert.ToInt32(rd[6]);
                    string ip = rd[3] as string;
                    int port = System.Convert.ToInt32(rd[4]);

                    byte[] hwstatus = new byte[4];
                    hwstatus[0] = System.Convert.ToByte(rd[5]);
                    hwstatus[1] = System.Convert.ToByte(rd[6]);
                    hwstatus[2] = System.Convert.ToByte(rd[7]);
                    hwstatus[3] = System.Convert.ToByte(rd[8]);
                    // byte opmode = System.Convert.ToByte(rd[13]);
                    // byte opstatus = System.Convert.ToByte(rd[14]);
                    // bool enable = (rd[15].ToString() == "Y") ? true : false;  //目前沒用

                    if (devicetype == "TILT")
                    {
                     //   int start_mileage, end_mileage;
                    //    start_mileage = System.Convert.ToInt32(rd[15 + 1]);

                     //   end_mileage = System.Convert.ToInt32(rd[16 + 1]);
                        TC.TiltDeviceWrapper wrapper = new TC.TiltDeviceWrapper(mfccid, devicename, devicetype, ip, port, hwstatus);

                        //if (isMfccAddDevice)
                        //    wrapper.getRemoteObj().AddDevice(wrapper.deviceName);
                        devices.Add(devicename, wrapper);

                    }
                    else if (devicetype == "GPS")
                    {
                        TC.GPSDeviceWrapper wrapper = new TC.GPSDeviceWrapper(mfccid, devicename, devicetype, ip, port, hwstatus);
                        //if (isMfccAddDevice)
                        //    wrapper.getRemoteObj().AddDevice(wrapper.deviceName);
                        devices.Add(devicename, wrapper);
                    }
                 






                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "," + ex.StackTrace);
            }

            finally
            {
                try
                {
                    cn.Close();
                    rd.Close();
                    rd.Dispose();
                    if (isMfccAddDevice)
                        cn.Close();
                }
                catch (Exception ex)
                {
                    ConsoleServer.WriteLine(ex.Message + ex.StackTrace);
                    ;
                }

            }
        }

    }
}
