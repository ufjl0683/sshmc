using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteInterface
{
    [Serializable]
   public  class NotifyEventObject
    {
        public EventEnumType type;
        public object EventObj=null;
       public string deviceName;
    //   public int port=-1;

       //public NotifyEventObject(EventEnumType type, object objEvent)
       // {
       //     this.EventObj = objEvent;
       //     this.type = type;
       //     this.devip = "*";
       // }
       public NotifyEventObject(EventEnumType type, string deviceName, object objEvent)
       {
           this.type = type;
           this.deviceName = deviceName;
           this.EventObj = objEvent;
         //  this.port = -1;//all
       }
       //public NotifyEventObject(EventEnumType type, string device_ip,int port, object objEvent):this( type,  device_ip,  objEvent)
       //{
       //    this.port=port;
       //}
    }

  public  enum EventEnumType
    {
      TEST,
        QY_New_Section_Data,
        QY_New_Travel_Time_Data,
       // RGS_Connection_Event,
      //  RGS_HW_Status_Event,
       // RMS_Connection_Event,
      //  RMS_HW_Status_Event,
        RMS_Mode_Change_Event,
        RGS_Display_Event,
        VD_Real_Data_Event, //�{�I�t�v
        VD_Trig_Event,
        HW_Status_Event,    
        Connection_Event,  //�s�u���A���ܨƥ�
        VD_1min_Cycle_Event,
        MFCC_Report_Event,  //MFCC �D�ʦ^���ƥ�
        MFCC_Comm_Moniter_Event,
        RD_5min_data_Event,
         VI_5min_data_Event,
         WD_10min_data_Event,
        ETTU_CC_Report_Event,  //ettu �ܾ����_�^��,
        ETTU_other_report_Event,
        TC_Manual_Ask_Event,
        NEW_RSP_EVENT,
        TEM_LCS_STATUS_CHANGE_EVENT,
        TIMCC_REP_INSTRUCTION
     
      //  TC_Connect_Status_Change


       
        
    }


    public interface I_RGS_HW_Status_Event_Data : I_HW_Status_Desc
    {
    }
}
