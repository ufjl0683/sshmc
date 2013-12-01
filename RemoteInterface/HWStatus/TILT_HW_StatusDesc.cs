using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteInterface.HWStatus
{
/*
    byte 2:
bit 0: 車道1之車輛偵測及影像處理單元故障
bit 1: 車道1之閃光燈單元故障
bit 2: 車道1之攝影機單元故障
bit 3: 車道1之水箱缺水
bit 4: 車道2之車輛偵測及影像處理單元故障
bit 5: 車道2之閃光燈單元故障
bit 6: 車道2之攝影機單元故障
bit 7: 車道2之水箱缺水
byte 3:
bit 0: 車道3之車輛偵測及影像處理單元故障
bit 1: 車道3之閃光燈單元故障
bit 2: 車道3之攝影機單元故障
bit 3: 車道3之水箱缺水
bit 4: 車道4之車輛偵測及影像處理單元故障
bit 5: 車道4之閃光燈單元故障
bit 6: 車道4之攝影機單元故障
bit 7: 車道4之水箱缺水
byte 4:
bit 0: 車道5之車輛偵測及影像處理單元故障
bit 1: 車道5之閃光燈單元故障
bit 2: 車道5之攝影機單元故障
bit 3: 車道5之水箱缺水
bit 4: 車道6之車輛偵測及影像處理單元故障
bit 5: 車道6之閃光燈單元故障
bit 6: 車道6之攝影機單元故障
bit 7: 車道6之水箱缺水
 * */

    public enum TILT_HW_Status_Bit_Enum
    {
        DeviceErr = 0,
        CabineteOpen = 1,
        PortableTest = 2,
        PanelOperate = 3,
        ParameterRequest = 4,
        AutoRestart = 5,
        LightSignalOff = 6,
        IO_unitFail = 7,
        Lane1VDImageProcessUnitErr = 8,
        Lane1FashLightErr = 9,
        Lane1CamErr = 10,
        Lane1WaterContainerErr = 11,
        Lane2VDImageProcessUnitErr =12,
        Lane2FashLightErr = 13,
        Lane2CamErr = 14,
        Lane2WaterContainerErr = 15,
        Sensor1Connected = 16,
        Sensor2Connected = 17,
        Sensor3Connected = 18,
        Sensor4Connected = 19,
        Sensor5Connected = 20,
        Sensor6Connected = 21,
        Sensor7Connected = 22,
        Sensor8Connected = 23,
        Sensor9Connected = 24,
        Sensor10Connected = 25,
        Sensor11Connected = 26,
        Sensor12Connected = 27,
        Sensor13Connected = 28,
        Sensor14Connected = 29,
        Sensor15Connected = 30,
        Sensor16Connected = 31,
    }

  public  class TILT_HW_StatusDesc : I_HW_Status_Desc
    {
        public static string[] hw_status_desc = new string[]
           {
               "設備故障","箱門開啟","手提測試機操作","現場操作","要求下傳基本參數","自行重新起動","燈號熄減","輸入單元故障",
               "車道1之車輛偵測及影像處理單元故障","車道1之閃光燈單元故障","車道1之攝影機單元故障","車道1之水箱缺水",
               "車道2之車輛偵測及影像處理單元故障","車道2之閃光燈單元故障","車道2之攝影機單元故障","車道2之水箱缺水",
               "Sensor0 連線","Sensor1 連線","Sensor2 連線","Sensor3 連線",
                "Sensor4 連線","Sensor5 連線","Sensor6 連線","Sensor7 連線",
                "Sensor8 連線","Sensor9 連線","Sensor10 連線","Sensor11 連線",
                "Sensor12 連線","Sensor13 連線","Sensor14 連線","Sensor15 連線",
           };


        
        System.Collections.BitArray ArrayhwStatus;
        byte[] diff;
         string devName;
      byte[] m_status;
         public TILT_HW_StatusDesc(string devName,byte[] hw_status, byte[] diff)
        {
            ArrayhwStatus = new System.Collections.BitArray(hw_status);
            this.diff = diff;
             this.devName=devName;
             this.m_status = hw_status;
        }
        public TILT_HW_StatusDesc(string devName, byte[] hw_status)
            : this(devName,hw_status, new byte[] { 0xff, 0xff, 0xff, 0xff })
        {

        }
        public string getDesc(int bitinx)
        {
            return ((TILT_HW_Status_Bit_Enum)bitinx).ToString();
        }
        public string getChiDesc(int inx)
        {
            return hw_status_desc[inx];
        }
        public bool getStatus(int bitinx)
        {
            return ArrayhwStatus.Get(bitinx);
        }

        public System.Collections.IEnumerable getEnum(byte[] indexs)
        {
            System.Collections.BitArray aryInx = new System.Collections.BitArray(indexs);
            for (int i = 0; i < aryInx.Count; i++)
            {
                if (aryInx.Get(i))
                    yield return (TILT_HW_Status_Bit_Enum)i;
            }
        }
        public System.Collections.IEnumerable getEnum()
        {
            System.Collections.BitArray aryInx = new System.Collections.BitArray(diff);
            for (int i = 0; i < aryInx.Count; i++)
            {
                if (aryInx.Get(i))
                    yield return (TILT_HW_Status_Bit_Enum)i;
            }
        }

        #region I_HW_Status_Desc 成員


        public string getDeviceName()
        {
            return this.devName;
        }

        #endregion


        public byte[] getHW_status()
        {
            return m_status;
        }


    }
}
