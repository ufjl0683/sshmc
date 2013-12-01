using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using RemoteInterface;
using System.Linq;
using System.Data.SqlClient;

namespace Host.MFCC
{
    public class MFCC_Manager
    {

        System.Collections.Hashtable mfccs = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());


        public MFCC_Manager()
        {
            System.Data.SqlClient.SqlConnection cn = new SqlConnection(DbCmdServer.getSSHMC_DbConnectStr());
            SqlCommand cmd = new SqlCommand("select host_id,host_ip,mfcc_id,mfcc_type,remote_port from vwhostmfcc where mfcc_type!='HOST'");

            SqlDataReader rd;
            try
            {

                string hostid, hostip, mfccid, mfcctype;
                int remoteport;
                cn.Open();
                cmd.Connection = cn;
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    hostid = rd[0] as string;
                    hostip = rd[1] as string;
                    mfccid = rd[2] as string;
                    mfcctype = rd[3] as string;
                    remoteport = System.Convert.ToInt32(rd[4]);
                    ConsoleServer.WriteLine("begin load " + mfccid);
                    mfccs.Add(mfccid, new MFCC.MFCC_Object(hostid, hostip, remoteport, mfccid, mfcctype));
                    ConsoleServer.WriteLine("load mfcc:" + mfccid);
                }

            }
            catch (Exception ex)
            {
                ConsoleServer.WriteLine(ex.Message + ex.StackTrace);
                System.Environment.Exit(-1);
            }
            finally
            {
                cn.Close();
                cn.Dispose();
            }

        }

        public MFCC_Object this[string mfccid]
        {
            get
            {
                if (mfccs.Contains(mfccid))
                    return (MFCC_Object)mfccs[mfccid];
                else
                    return null;//throw new Exception(mfccid + " not found!");

            }
        }

    }
}
