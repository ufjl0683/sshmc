using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteInterface;

namespace Host
{
    public class HCommObject : RemoteClassBase, RemoteInterface.HC.I_HC_Comm
    {
        public void SetSensorValueDegree(int snrid, double value0, double value1, double value2, int degree)
        {
            try
            {
                Program.host.sensor_mgr.SetSensorCurrentDegreeValue(snrid, value0, value1, value2, degree);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "," + ex.StackTrace);
                throw new RemoteInterface.RemoteException(ex.Message);
            }
          //  throw new NotImplementedException();
        }

       

        public void setDeviceStatus(string devName, byte[] hw_status, byte opstatus, byte opmode, bool isConnected)
        {
            try
            {
                throw new NotImplementedException();
           //     DeviceBaseWrapper dev = Program.matrix.getDeviceWrapper(devName);
                //if (dev != null)
                //    dev.set_HW_status(hw_status, opmode, opmode, isConnected);
            }
            catch (Exception ex)
            {
                ConsoleServer.WriteLine(ex.Message + ex.StackTrace);
                throw new RemoteException(ex.Message + ex.StackTrace);
            }
            finally
            {
                //if (dictPerformance.ContainsKey("setDeviceStatus"))
                //{

                //    //  dictPerformance["setDeviceStatus"].CallCount++;
                //    dictPerformance["setDeviceStatus"].InCount--;
                //}
            }
        }
        public string getScriptSource(string devType)//取得script source 文字
        {
            try
            {
                //if (dictPerformance.ContainsKey("getScriptSource"))
                //{
                //    dictPerformance["getScriptSource"].CallCount++;
                //    dictPerformance["getScriptSource"].InCount++;
                //}
                return Program.host.scripts_mgr[devType].getScriptSource();
            }
            catch (Exception ex)
            {
                throw new RemoteException(ex.Message);
            }
            finally
            {
                //if (dictPerformance.ContainsKey("getScriptSource"))
                //{
                //    //dictPerformance["getScriptSource"].CallCount++;
                //    dictPerformance["getScriptSource"].InCount--;
                //}
            }
        }
        public DateTime getDateTime()
        {
            try
            {
                return DateTime.Now;
            }
            catch (Exception ex)
            {
                throw new RemoteInterface.RemoteException(ex.Message);
            }
        }


        public void setDeviceStatus(int controllerid, byte[] hw_status, bool isConnected)
        {
           // throw new NotImplementedException();
        }


        public void NotifySponsor(string siteid, string mailaddress, string mailtitle, string mailbody)
        {
            Program.host.site_mgr.NotifySponsor(siteid, mailaddress, mailtitle, mailbody);
           // throw new NotImplementedException();
        }

        public void SuspendEvent(string siteid)
        {
            Program.host.site_mgr.SuspendEvent(siteid); 
        }

        public void ExeuteEvent(string siteid)
        {
            Program.host.site_mgr.ExeuteEvent(siteid);
        }
    }
}
