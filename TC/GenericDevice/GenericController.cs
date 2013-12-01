using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Comm;
using Comm.DataStore;
using RemoteInterface.SensorConfig;
using System.IO;

namespace GenericDevice
{
    public class GenericController : Comm.Controller.SSHDataController
    {
    
       // SensorBase[] devices;
      
        public GenericController(ControllerConfigBase config,PropertyBag propertybag)
            : base(config, propertybag)
        {
            //initialProperty();
            this.PropertyBag.TransmitCycle = 10;
            this.PropertyBag.TransmitMode = 1;
            SetTransmitCycleTmr();
          //  this.config = config;
          

        }

        //void PiltController_OnConnectionChanged(int id, string senserName, bool IsConnected)
        //{
        //    byte[] connect_status = new byte[2];
        //    connect_status[0] = PropertyBag.HWtatus[2];
        //    connect_status[1] = PropertyBag.HWtatus[3];
        //    System.Collections.BitArray ba = new System.Collections.BitArray(connect_status);
        //    ba.Set(id, IsConnected);
            
        //}
       

       

       protected override SensorBase CreateDevice(SensorConfigBase config)
        {
            GenericDevice dev;

            if (config.com_type == "TCP")
            {
              
                    return new GenericDevice(config.id,this, config.device_name, new System.Net.IPEndPoint(System.Net.IPAddress.Parse(config.ip_comport), config.port_baud));
            }
            else  //COM
            {
               
                return new GenericDevice(config.id,this, config.device_name, config.ip_comport, config.port_baud);
            }

        }
      

        //public override void OnReceiveText(Comm.TextPackage text)
        //{

        //    try
        //    {

        //        switch (text.Cmd)
        //        {
        //            case 0x20:  //set Communication param
        //                SetConfig(text);
        //                break;

        //            case 0x04:
        //                switch (text.Text[1])
        //                {

        //                    case 0x20:
        //                        byte[] configbytes=GetConfigBytes();
        //                        byte[] data = new byte[configbytes.Length + 3];

        //                        data[0] = 0x20;
        //                        data[1] = (byte)(configbytes.Length / 256);
        //                        data[2] = (byte)(configbytes.Length % 256);
        //                        System.Array.Copy(configbytes, 0, data, 3, configbytes.Length);
                                 
        //                        SendPackage pkg = new SendPackage(CmdType.CmdQuery, CmdClass.A, this.ControllerID, To04DataBytes(data));
        //                        pkg.Seq = text.Seq;
        //                        SendDirect(pkg);
        //                        break;
        //                }
        //                break;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message+","+ex.StackTrace);
        //    }

        //}

      
       
        
     
        //void GetDegree(int sensor_id,double value1,double value2,out int value1_level,out int value2_level)
        //{
        //  double diff1,diff2;
        //  //if (config.execution_mode == 0)
        //  //{

        //  //    value1_level = value2_level = 0;
        //  //    return;
        //  //}
        //    TiltSensorConfig pconfig=(TiltSensorConfig)config.sensors[sensor_id];
        //  diff1 = Math.Abs(pconfig.init_value1 - value1);
        //  diff2 = Math.Abs(pconfig.init_value2 - value2);
        //  if (diff1 < pconfig.value1_level1)
        //      value1_level = 0;
        //  else if (diff1 <= pconfig.value1_level2)
        //      value1_level = 1;
        //  else if (diff1 <= pconfig.value1_level3)
        //      value1_level = 2;
        //  else
        //      value1_level = 3;

        //  if (diff2 < pconfig.value2_level1)
        //      value2_level = 0;
        //  else if (diff1 <= pconfig.value2_level2)
        //      value2_level = 1;
        //  else if (diff1 <= pconfig.value2_level3)
        //      value2_level = 2;
        //  else
        //      value2_level = 3;

        //}
        //void SetLevelDegree(Comm.TextPackage text)
        //{
        //    System.Data.DataSet ds;
        //    ds = protocol.GetSendDsByTextPackage(text, CmdType.CmdSet);
        //    System.Data.DataRow row = ds.Tables["tblMain"].Rows[0];
        //    int sensor_cnt = System.Convert.ToInt32(row["sensor_cnt"]);
        //    for (int i = 0; i < sensor_cnt; i++)
        //    {
        //        System.Data.DataRow r = ds.Tables["tblsensor_cnt"].Rows[i];
        //        int id = System.Convert.ToInt32(r["id"]);
        //        double init_value1 = V2DLE.uLongToDouble(System.Convert.ToUInt64(r["init_value1"]));
        //        double init_value2 = V2DLE.uLongToDouble(System.Convert.ToUInt64(r["init_value2"]));
        //        double value1_factor = V2DLE.uLongToDouble(System.Convert.ToUInt64(r["value1_factor"]));
        //        double value2_factor = V2DLE.uLongToDouble(System.Convert.ToUInt64(r["value2_factor"]));
        //        double value1_offset = V2DLE.uLongToDouble(System.Convert.ToUInt64(r["value1_offset"]));
        //        double value2_offset = V2DLE.uLongToDouble(System.Convert.ToUInt64(r["value2_offset"]));
        //        double value1_level1 = V2DLE.uLongToDouble(System.Convert.ToUInt64(r["value1_level1"]));
        //        double value1_level2 = V2DLE.uLongToDouble(System.Convert.ToUInt64(r["value1_level2"]));
        //        double value1_level3 = V2DLE.uLongToDouble(System.Convert.ToUInt64(r["value1_level3"]));

        //        double value2_level1 = V2DLE.uLongToDouble(System.Convert.ToUInt64(r["value2_level1"]));
        //        double value2_level2 = V2DLE.uLongToDouble(System.Convert.ToUInt64(r["value2_level2"]));
        //        double value2_level3 = V2DLE.uLongToDouble(System.Convert.ToUInt64(r["value2_level3"]));

        //        ((TiltSensorConfig)config.sensors[id]).init_value1 = init_value1;
        //        ((TiltSensorConfig)config.sensors[id]).init_value2 = init_value2;

        //        ((TiltSensorConfig)config.sensors[id]).value1_factor = value1_factor;
        //       ((TiltSensorConfig) config.sensors[id]).value2_factor = value2_factor;
        //        ((TiltSensorConfig)config.sensors[id]).value1_offset = value1_offset;
        //        ((TiltSensorConfig)config.sensors[id]).value2_offset = value2_offset;

        //        ((TiltSensorConfig)config.sensors[id]).value1_level1 = value1_level1;
        //       ((TiltSensorConfig) config.sensors[id]).value1_level2 = value1_level2;
        //        ((TiltSensorConfig)config.sensors[id]).value1_level3 = value1_level3;

        //       ((TiltSensorConfig) config.sensors[id]).value2_level1 = value2_level1;
        //        ((TiltSensorConfig)config.sensors[id]).value2_level2 = value2_level2;
        //        ((TiltSensorConfig)config.sensors[id]).value2_level3 = value2_level3;
        //        config.Serialize(AppDomain.CurrentDomain.BaseDirectory+"config.xml");
        //    }
        //}

        //byte[] GetConfigBytes()
        //{

        //    MemoryStream ms = new MemoryStream();
        //    System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(ControllerConfigBase));
        //    ser.Serialize(ms, this.config);
        //    return ms.GetBuffer();

        //}


        //void SetConfig(Comm.TextPackage text)
        //{
           

        //    MemoryStream ms = new MemoryStream(text.Text, 3, text.Text[1] * 256 + text.Text[2]);
        //   this.config=  ControllerConfigBase.Deserialize(ms);

        //    config.Serialize(AppDomain.CurrentDomain.BaseDirectory+"config.xml");
        //}
     

        protected override void OnOneMinTmrTask()
        {
            try
            {
                Console.WriteLine("================One Min Task============"); ;
                if (devices.Length == 0)
                    return;
                double[] values = new double[devices.Length * 3];
                for (int i = 0; i < devices.Length; i++)
                {
                    values[i * 3] =((GenericDevice) devices[i]).GetValue(0);
                    values[i * 3 + 1] = ((GenericDevice)devices[i]).GetValue(1);
                    values[i * 3 + 2] = ((GenericDevice)devices[i]).GetValue(2);
                    //values[i * 3 + 2] = 0;
                    Console.WriteLine("{2} Value1:{0:0.00000} Value2:{1:0.000000} temp{3:0.000000}", values[i * 3], values[i * 3 + 1],devices[i].SensorName,values[i*3+2]);
                }

                DateTime dt = DateTime.Now;
                dt = GetYMDHM(dt);
                this.dataStore.PutStoreData(
                    new Comm.DataStore.StoreData<double>(dt, values));
               
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "," + ex.StackTrace);
            }
             
           
          //  throw new NotImplementedException();
        }

        public override void OnSSHDDataController_ReceiveText(TextPackage text)
        {
          //  throw new NotImplementedException();
        }
    }
}
