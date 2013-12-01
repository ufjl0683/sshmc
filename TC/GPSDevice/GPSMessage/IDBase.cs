using System;
using System.Collections.Generic;
using System.Text;

namespace GPSDevice.GPSMessage
{

    public   class IDBase
    {
        static ItemInfo[] ItemsInfo2;
        static ItemInfo[] ItemsInfo4;
        static ItemInfo[] ItemsInfo7;
        static ItemInfo[] ItemsInfo28;
        static ItemInfo[] ItemsInfo30;
        static ItemInfo[] ItemsInfo29;
       protected   byte[] PayLoad;
       protected ItemInfo[] ItemsInfo;
      

       public  IDBase(byte[] payload)
       {
           this.PayLoad = payload;
           //InitialItemsInfo();
           switch (payload[0])
           {
               case 2:
                   ItemsInfo = ItemsInfo2;
                   break;
               case 4:
                   ItemsInfo = ItemsInfo4;
                   break;
               case 7:
                   ItemsInfo = ItemsInfo7;
                   break;
               case 28:
                   ItemsInfo = ItemsInfo28;
                   break;
               case 30:
                   ItemsInfo = ItemsInfo30;
                   break;
           }
       }

       static  IDBase()
       {
           InitialItemsInfo2();
           InitialItemsInfo4();
           InitialItemsInfo7();
           InitialItemsInfo28();
           InitialItemsInfo29();
           InitialItemsInfo30();
       }


       static void InitialItemsInfo29()
       {
           ItemsInfo29 = new ItemInfo[]{
             new ItemInfo("Message_ID","",1,1),
             new ItemInfo("Satellite ID","",2,1),
             new ItemInfo("IOD","",2,100),
             new ItemInfo("Source","",1,1),
             new ItemInfo("Pseudorange_Correction","m",4,1), 
             new ItemInfo("Pseudorange rate Correction","m/sec",4,1), 
             new ItemInfo("Correction_Age","sec",4,1), 
             new ItemInfo("Reserved1","",4,1), 
             new ItemInfo("Reserved2","",4,1), 

            };
       }
       static  void InitialItemsInfo7()
       {
           ItemsInfo7 = new ItemInfo[]
         {
             new ItemInfo("Message_ID","",1,1),
             new ItemInfo("Extended_GPS_Week","",2,1),
             new ItemInfo("GPS_TOW","secs",4,100),
             new ItemInfo("SVs","",1,1),
             new ItemInfo("Clock_Drift","Hz",4,1), 
             new ItemInfo("Clock_Bias","ns",4,1), 
             new ItemInfo("Estimated_GPS_Time","ms",4,1), 
         };
       }
       static void InitialItemsInfo30()
         {
             // throw new NotImplementedException();

             ItemsInfo30 = new ItemInfo[]{
                new ItemInfo("Message_ID","",1,1),
                new ItemInfo("Satellite_ID","",1,1),
                new ItemInfo("GPS_Time","sec",8,1,true),
                new ItemInfo("Position_X","m",8,1,true),
                new ItemInfo("Position_Y","m",8,1,true),
                new ItemInfo("Position_Z","m",8,1,true),
                new ItemInfo("Velocity_X","m/sec",8,1,true),
                new ItemInfo("Velocity_Y","m/sec",8,1,true),
                new ItemInfo("Velocity_Z","m/sec",8,1,true),
                new ItemInfo("Clock_Bias","sec",8,1,true),
                new ItemInfo("Clock_Drift","sec",4,1,true),
                new ItemInfo("Ephemeris_Flag","",1,1),
                new ItemInfo("Reserved_1","",4,1),
                new ItemInfo("Reserved_2","",4,1),
                 new ItemInfo("Ionospheric_Delay","m",4,1,true),
            };
         }
       static void InitialItemsInfo28()
         {
             ItemsInfo28 = new ItemInfo[]
            {
                new ItemInfo("Message_ID","",1,1),
                new ItemInfo("Channel","",1,1),
                new ItemInfo("Time_Tag","ms",4,1),
                new ItemInfo("Satellite_ID","",1,1),
                // Satellite ID 1\
                new ItemInfo("GPS_Software_Time","sec",8,1,true),
                //GPS Software Time

                new ItemInfo("Pseudorange","m",8,1,true),
                new ItemInfo("Carrier_Frequency","m/s",4,1,true),
                //Carrier Phase 8
                new ItemInfo("Carrier_Phase","m",8,1,true),

                  new ItemInfo("Time_in_Track","ms",2,1),
                  new ItemInfo("Sync_Flags","",1,1),
                  new ItemInfo("C/No_1","dB-Hz",1,1),
                  new ItemInfo("C/No_2","dB-Hz",1,1),
                  new ItemInfo("C/No_3","dB-Hz",1,1),
                  new ItemInfo("C/No_4","dB-Hz",1,1),
                  new ItemInfo("C/No_5","dB-Hz",1,1),
                  new ItemInfo("C/No_6","dB-Hz",1,1),
                  new ItemInfo("C/No_7","dB-Hz",1,1),
                  new ItemInfo("C/No_8","dB-Hz",1,1),
                  new ItemInfo("C/No_9","dB-Hz",1,1),
                  new ItemInfo("C/No_10","dB-Hz",1,1),
                  new ItemInfo("Delta_Range_Interval","ms",2,1),
                  new ItemInfo("Mean_Delta_Range_Time","ms",2,1),
                  new ItemInfo("Extrapolation_Time","ms",2,1),
                    new ItemInfo("Phase_Error_Count","",1,1),
                  new ItemInfo("Low_Power_Count","",1,1)
                  //Low Power Count

            };

             //  throw new NotImplementedException();
         }


       static  void InitialItemsInfo4()
       {
           ItemsInfo4 = new ItemInfo[172];
           
           ItemsInfo4[0] = new ItemInfo("Message_ID", "", 1, 1);
           ItemsInfo4[1] = new ItemInfo("GPS_Week", "", 2, 1);
           ItemsInfo4[2] = new ItemInfo("GPS_TOW", "SECS", 4, 1);
           ItemsInfo4[3] = new ItemInfo("Chans", "", 1, 1);
           try
           {
               for (int i = 0; i < 168; i += 14)
               {

                   ItemsInfo4[4 + i + 0] = new ItemInfo((i / 14 + 1) + "st_SVid", "", 1, 1);
                   ItemsInfo4[4 + i + 1] = new ItemInfo((i / 14 + 1) + "st_Azimuth", "deg", 1, 2.0 / 3);
                   ItemsInfo4[4 + i + 2] = new ItemInfo((i / 14 + 1) + "st_Elev", "deg", 1, 2.0);
                   ItemsInfo4[4 + i + 3] = new ItemInfo((i / 14 + 1) + "st_State", "dB-Hz", 2, 1);
                   ItemsInfo4[4 + i + 4] = new ItemInfo((i / 14 + 1) + "st_C/No_1", "dB-Hz", 1, 1);
                   ItemsInfo4[4 + i + 5] = new ItemInfo((i / 14 + 1) + "st_C/No_2", "dB-Hz", 1, 1);
                   ItemsInfo4[4 + i + 6] = new ItemInfo((i / 14 + 1) + "st_C/No_3", "dB-Hz", 1, 1);
                   ItemsInfo4[4 + i + 7] = new ItemInfo((i / 14 + 1) + "st_C/No_4", "dB-Hz", 1, 1);
                   ItemsInfo4[4 + i + 8] = new ItemInfo((i / 14 + 1) + "st_C/No_5", "dB-Hz", 1, 1);
                   ItemsInfo4[4 + i + 9] = new ItemInfo((i / 14 + 1) + "st_C/No_6", "dB-Hz", 1, 1);
                   ItemsInfo4[4 + i + 10] = new ItemInfo((i / 14 + 1) + "st_C/No_7", "dB-Hz", 1, 1);
                   ItemsInfo4[4 + i + 11] = new ItemInfo((i / 14 + 1) + "st_C/No_8", "dB-Hz", 1, 1);
                   ItemsInfo4[4 + i + 12] = new ItemInfo((i / 14 + 1) + "st_C/No_9", "dB-Hz", 1, 1);
                   ItemsInfo4[4 + i + 13] = new ItemInfo((i / 14 + 1) + "st_C/No_10", "dB-Hz", 1, 1);


               }
           }
           catch (Exception ex)
           {
               Console.WriteLine(ex.Message);
           }
         
       }
       static   void InitialItemsInfo2()
       {
           ItemsInfo2 = new ItemInfo[]
           {

               new ItemInfo("Message_ID","",1,1),
               new ItemInfo("X-position","m",4,1),
               new ItemInfo("Y-position","m",4,1),
               new ItemInfo("Z-position","m",4,1),
               new ItemInfo("X-velocity","m/sec",2,8),
               new ItemInfo("Y-velocity","m/sec",2,8),
               new ItemInfo("Y-velocity","m/sec",2,8),
               new ItemInfo("Mode_1","",1,1),
               new ItemInfo("HDOP","",1,5),
               new ItemInfo("Mode_2","",1,1),
               new ItemInfo("GPS_WEEK","",2,1),
               new ItemInfo("GPS_TOW","secs",4,100),
               new ItemInfo("SVs_in_Fix","",1,1),
               new ItemInfo("CH_1_PRN5","",1,1),
               new ItemInfo("CH_2_PRN5","",1,1),
               new ItemInfo("CH_3_PRN5","",1,1),
               new ItemInfo("CH_4_PRN5","",1,1),
               new ItemInfo("CH_5_PRN5","",1,1),
               new ItemInfo("CH_6_PRN5","",1,1),
               new ItemInfo("CH_7_PRN5","",1,1),
               new ItemInfo("CH_8_PRN5","",1,1),
               new ItemInfo("CH_9_PRN5","",1,1),
               new ItemInfo("CH_10_PRN5","",1,1),
               new ItemInfo("CH_11_PRN5","",1,1),
               new ItemInfo("CH_12_PRN5","",1,1)
           };
       }



       public int GetMessageID()
       {
           return PayLoad[0];
       }

       public int GetItemsCount()
       {
           return ItemsInfo.Length;
       }

       public int GetTotalLength()
       {
           int ret = 0;
           foreach (ItemInfo item in ItemsInfo)
               ret += item.Length;
           return ret;
       }
        
       public override string ToString()
       {
           StringBuilder sb=new StringBuilder();
          
           for (int i = 0; i < ItemsInfo.Length; i++)
           {
               if(i!=0)
               sb.Append(" ");
               sb.Append(ItemsInfo[i].Name + ":" + this[i] +" "+ ItemsInfo[i].Unit);
               if (i % 4 == 0)
                   sb.Append("\n");
               else
                   sb.Append("\t");
           }
           if (this.GetMessageID() == 2)
           {
               double[] llh=GPSDevice.xyz2llh(new double[]{this[1],this[2],this[3]});
               sb.Append(string.Format("\nlongtitude:{0},latitude:{1},height:{2}", llh[0], llh[1], llh[2]));
           }
           sb.Append("\nTotal Length:"+ GetTotalLength()+"\n");
           return sb.ToString();
           //return base.ToString();
       }

       protected int GetLength(int inx)
       {
           return ItemsInfo[inx].Length;
       }

        protected double GetScale(int inx)
        {
            return ItemsInfo[inx].Scale;
        }

        public double this[string ItemName]
        {
            get
            {
                string str=ItemName.ToUpper();
                for (int i = 0; i < ItemsInfo.Length; i++)
                {
                    if (ItemsInfo[i].Name.ToUpper() == str)
                        return this[i];
                }

                throw new Exception("ItemName:" + ItemName + " not found!");
            }
        }

       public  double this[int inx]
       {
           get
           {
               
               int startpos = 0;
               for (int i = 0; i < inx; i++)
                   startpos += this.GetLength(i);

               byte[] data=new  byte[this.GetLength(inx)];

               System.Array.Copy(PayLoad, startpos, data, 0, data.Length);
               if (System.BitConverter.IsLittleEndian)
               {
                   System.Array.Reverse(data);
               }

               switch(this.GetLength(inx))
               {
                   case 1:
                       return (double)data[0]/GetScale(inx) ;
                     
                   case 2:
                       return (double)System.BitConverter.ToInt16(data,0)/GetScale(inx);
                     
                   case 4:
                       if (this.ItemsInfo[inx].IsDouble)
                           return (double)System.BitConverter.ToSingle(data, 0);
                       return (double)System.BitConverter.ToInt32(data,0) / GetScale(inx);
                   case 8:
                       if (this.ItemsInfo[inx].IsDouble)
                           return System.BitConverter.ToDouble(data,0);
                       else
                           return (double)System.BitConverter.ToInt64(data, 0) / GetScale(inx);
                   default:
                       throw new Exception("Unknown Data Length!");
                     

               }


           }

       }
       
       
    }

   

    public class ItemInfo
    {
        public string Name,Unit;
        public int Length;
        public  double Scale;
        
        public bool IsDouble = false;
        public ItemInfo(string name, string unit,int len,double scale):this(name,unit,len,scale,false)
        {
                                
                               
        }

        public ItemInfo(string name, string unit, int len, double scale,bool isDouble)
        {
            this.Name = name;
            this.Unit = unit;
            this.Length = len;
            this.Scale = scale;
            this.IsDouble = isDouble;
        }

     

    }

}
