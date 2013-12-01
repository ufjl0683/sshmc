using System;
using System.Collections.Generic;
using System.Text;

namespace GPSDevice.GPSMessage
{
   public  class UBIDBase
    {
        static UBXItemInfo[] ItemsInfo0230;  //ALM
        static UBXItemInfo[] ItemsInfo0231; //eph
        static UBXItemInfo[] ItemsInfo0122;
        static UBXItemInfo[] ItemsInfo0220;
        static UBXItemInfo[] ItemsInfo0210;
        static UBXItemInfo[] ItemsInfo0132;
        static UBXItemInfo[] ItemsInfo0101; 
        //static ItemInfo[] ItemsInfo0122;
        //static ItemInfo[] ItemsInfo7;
        //static ItemInfo[] ItemsInfo28;
        //static ItemInfo[] ItemsInfo30;
        //static ItemInfo[] ItemsInfo29;
        protected byte[] PayLoad;
        protected UBXItemInfo[] ItemsInfo;

        public System.Collections.Generic.Dictionary<string, int> dictItemName_Index = new Dictionary<string, int>();

        public int svid
        {
            get{
                if (this.GetMessageID() == 0x0230 || this.GetMessageID() == 0x0231)
                    return (int)this[1];
                else
                    return 0;
               }
        }

       

        public UBIDBase(byte[] payload)
       {
           this.PayLoad = payload;
           //InitialItemsInfo();
           switch (payload[0]*256+payload[1])
           {
               case 0x0230:    //alm
                   ItemsInfo = ItemsInfo0230;
                   break;
               case 0x0231:   //eph
                   ItemsInfo = ItemsInfo0231;
                   break;

               case 0x0122:
                   ItemsInfo = ItemsInfo0122;
                   break;
               case 0x0220:
                   ItemsInfo = ItemsInfo0220;
                   break;
               case 0x0210:
                   ItemsInfo = ItemsInfo0210;
                   break;
               case 0x0132:
                   ItemsInfo = ItemsInfo0132;
                   break;
               case 0x0101:
                   ItemsInfo = ItemsInfo0101;
                   break;
               default:
                   return;
               //case 7:
               //    ItemsInfo = ItemsInfo7;
               //    break;
               //case 28:
               //    ItemsInfo = ItemsInfo28;
               //    break;
               //case 30:
               //    ItemsInfo = ItemsInfo30;
               //    break;
           }
           for (int inx = 0; inx < ItemsInfo.Length; inx++)
               dictItemName_Index.Add(ItemsInfo[inx].Name.Trim().ToUpper(), inx);
       }

        public bool IsValid
        {
            get
            {
                return this.PayLoad.Length == this.GetTotalLength();
            }
        }
        static UBIDBase()
       {
           InitialItemsInfo0230();
           InitialItemsInfo0231();
           InitialItemsInfo0122();
           InitialItemsInfo0220();
           InitialItemsInfo0210();
           InitialItemsInfo0132();
           InitialItemsInfo0101();
            
           //InitialItemsInfo7();
           //InitialItemsInfo28();
           //InitialItemsInfo29();
           //InitialItemsInfo30();
       }

     
   


        public int GetMessageID()
       {
           return PayLoad[0] * 256 + PayLoad[1] ;
       }

       public int GetItemsCount()
       {
           return ItemsInfo.Length;
       }

       public int GetTotalLength()
       {
           int ret = 0;
           foreach (UBXItemInfo item in ItemsInfo)
               ret += item.Length;
           return ret;
       }

       public int GetPayLoadLendth()
       {
           return PayLoad.Length;
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
                if(!dictItemName_Index.ContainsKey(ItemName.Trim().ToUpper()))
                    throw new Exception("ItemName:" + ItemName + " not found!");

                return this[dictItemName_Index[ItemName.Trim().ToUpper()]];

                //string str=ItemName.ToUpper();
                //for (int i = 0; i < ItemsInfo.Length; i++)
                //{
                //    if (ItemsInfo[i].Name.ToUpper() == str)
                //        return this[i];
                //}

                
            }
        }

        public double this[int inx]
           {
               get
               {

                   int startpos = 0;
                   for (int i = 0; i < inx; i++)
                       startpos += this.GetLength(i);

                   byte[] data = new byte[this.GetLength(inx)];

                   if ((this.GetMessageID() == 0x0231 || this.GetMessageID()==0x0230)  && this.GetPayLoadLendth() - 2 == 8 && inx >1  )
                       return 0;

                   //if (this.GetMessageID() == 0x0230 && this.GetPayLoadLendth() - 2 == 8 && inx != 0)
                   //    return 0;

                   System.Array.Copy(PayLoad, startpos, data, 0, data.Length);


                 

                 
                   if(inx==0)
                          System.Array.Reverse(data);
                 

                   switch (this.GetLength(inx))
                   {
                       case 1:
                           if (this.ItemsInfo[inx].ItemType == ItemInfoType.DOUBLE)
                               return (sbyte)data[0] / GetScale(inx);
                           else if (this.ItemsInfo[inx].ItemType == ItemInfoType.INT)
                               return (sbyte)data[0] / GetScale(inx);
                           else
                               return data[0] / GetScale(inx);

                       case 2:
                           if (this.ItemsInfo[inx].ItemType == ItemInfoType.DOUBLE)
                                return System.BitConverter.ToInt16(data, 0) / GetScale(inx);
                              else if (this.ItemsInfo[inx].ItemType == ItemInfoType.INT)
                                 return System.BitConverter.ToInt16(data, 0) / GetScale(inx);
                             else
                               return System.BitConverter.ToUInt16(data, 0) / GetScale(inx);
                          
                       case 4:
                           if (this.ItemsInfo[inx].ItemType== ItemInfoType.DOUBLE)
                         
                               return (double)System.BitConverter.ToSingle(data, 0);
                         
                           else if(this.ItemsInfo[inx].ItemType== ItemInfoType.UINT)
                               return (double)System.BitConverter.ToUInt32(data, 0) / GetScale(inx);
                           else
                               return (double)System.BitConverter.ToInt32(data, 0) / GetScale(inx);
                       case 8:
                           if (this.ItemsInfo[inx].ItemType== ItemInfoType.DOUBLE)
                               return System.BitConverter.ToDouble(data, 0);
                           else if (this.ItemsInfo[inx].ItemType == ItemInfoType.UINT)
                               return (double)System.BitConverter.ToUInt64(data, 0) / GetScale(inx);
                           else
                               return (double)System.BitConverter.ToInt64(data, 0) / GetScale(inx);

                       default:
                           throw new Exception("Unknown Data Length!");


                   }


               }
           }

        static void InitialItemsInfo0122()
           {
               ItemsInfo0122 = new UBXItemInfo[]
                {
                      new UBXItemInfo("ID","",2,1),
                     new UBXItemInfo("itow","ms",4,1),
                     new UBXItemInfo("clkb","ns",4,1, ItemInfoType.INT),
                     new UBXItemInfo("clkd","ns/s",4,1, ItemInfoType.INT),
                     new UBXItemInfo("tacc","ns",4,1, ItemInfoType.UINT),
                     new UBXItemInfo("facc","ps/s",4,1, ItemInfoType.UINT)
                };
           }
        static void InitialItemsInfo0220()
        {
            ItemsInfo0220 = new UBXItemInfo[100*5+5];
            ItemsInfo0220[0] = new UBXItemInfo("ID", "", 2, 1);
            ItemsInfo0220[1] = new UBXItemInfo("itow", "ms", 4, 1, ItemInfoType.INT);
            ItemsInfo0220[2] = new UBXItemInfo("week", "weeks", 2, 1, ItemInfoType.INT);
            ItemsInfo0220[3] = new UBXItemInfo("numvis", "", 1, 1, ItemInfoType.UINT);
            ItemsInfo0220[4] = new UBXItemInfo("numsv", "", 1, 1, ItemInfoType.UINT);
            for (int i = 0; i < 100; i++)
            {
                ItemsInfo0220[5 + i * 5] = new UBXItemInfo("svid_" + (i ), "", 1, 1, ItemInfoType.UINT);
                ItemsInfo0220[5 + i * 5+1] = new UBXItemInfo("svflag_" + (i ), "", 1, 1, ItemInfoType.UINT);
                ItemsInfo0220[5 + i * 5 + 2] = new UBXItemInfo("azim_" + (i ), "", 2, 1, ItemInfoType.INT);
                ItemsInfo0220[5 + i * 5 + 3] = new UBXItemInfo("elev_" + (i ), "", 1, 1, ItemInfoType.INT);
                ItemsInfo0220[5 + i * 5 + 4] = new UBXItemInfo("age_" + (i ), "", 1, 1, ItemInfoType.UINT);
            }

        }

        static void InitialItemsInfo0210()
        {
            ItemsInfo0210 = new UBXItemInfo[32*7 + 5];
            ItemsInfo0210[0] = new UBXItemInfo("ID", "", 2, 1);
            ItemsInfo0210[1] = new UBXItemInfo("itow", "ms", 4, 1, ItemInfoType.INT);
            ItemsInfo0210[2] = new UBXItemInfo("week", "weeks", 2, 1, ItemInfoType.INT);
            ItemsInfo0210[3] = new UBXItemInfo("numsv", "", 1, 1, ItemInfoType.UINT);
            ItemsInfo0210[4] = new UBXItemInfo("reserved1", "", 1, 1, ItemInfoType.UINT);
            for (int i = 0; i < 32; i++)
            {
                ItemsInfo0210[5 + i * 7] = new UBXItemInfo("cpmes_" + (i ), "cycles", 8, 1, ItemInfoType.DOUBLE);
                ItemsInfo0210[5 + i * 7 + 1] = new UBXItemInfo("prmes_" + (i ), "m", 8, 1, ItemInfoType.DOUBLE);
                ItemsInfo0210[5 + i * 7 + 2] = new UBXItemInfo("domes_" + (i ), "hz", 4, 1, ItemInfoType.DOUBLE);
                ItemsInfo0210[5 + i * 7 + 3] = new UBXItemInfo("sv_" + (i ), "", 1, 1, ItemInfoType.UINT);
                ItemsInfo0210[5 + i * 7 + 4] = new UBXItemInfo("mesqi_" + (i ), "", 1, 1, ItemInfoType.INT);
                ItemsInfo0210[5 + i * 7 + 5] = new UBXItemInfo("cno_" + (i), "dbHz", 1, 1, ItemInfoType.INT);
                ItemsInfo0210[5 + i * 7 + 6] = new UBXItemInfo("lli_" + (i), "", 1, 1, ItemInfoType.UINT);
            }

        }

//0 U4 - iTOW ms GPS Millisecond time of week
//4 U1 - geo - PRN Number of the GEO where correction and
//integrity data is used from
//5 U1 - mode - SBAS Mode
//0 Disabled
//1 Enabled Integrity
//3 Enabled Testmode
//6 I1 - sys - SBAS System (WAAS/EGNOS/...)
//-1 Unknown
//0 WAAS
//1 EGNOS
//2 MSAS
//16 GPS
//7 X1 - service - SBAS Services available (see graphic below)
//8 U1 - cnt - Number of SV data following
//9 U1[3] - reserved0 - Reserved

        static void InitialItemsInfo0132()
        {

            ItemsInfo0132 = new UBXItemInfo[32 * 9 + 10];
            ItemsInfo0132[0] = new UBXItemInfo("ID", "", 2, 1);
            ItemsInfo0132[1] = new UBXItemInfo("itow", "ms", 4, 1, ItemInfoType.UINT);
            ItemsInfo0132[2] = new UBXItemInfo("geo", "", 1, 1, ItemInfoType.UINT);
            ItemsInfo0132[3] = new UBXItemInfo("mode", "", 1, 1, ItemInfoType.UINT);
            ItemsInfo0132[4] = new UBXItemInfo("sys", "", 1, 1, ItemInfoType.INT);
            ItemsInfo0132[5] = new UBXItemInfo("service", "", 1, 1, ItemInfoType.UINT);
            ItemsInfo0132[6] = new UBXItemInfo("cnt", "", 1, 1, ItemInfoType.UINT);
            ItemsInfo0132[7] = new UBXItemInfo("reserved0", "", 1, 1, ItemInfoType.UINT);
            ItemsInfo0132[8] = new UBXItemInfo("reserved1", "", 1, 1, ItemInfoType.UINT);
            ItemsInfo0132[9] = new UBXItemInfo("reserved2", "", 1, 1, ItemInfoType.UINT);
            for (int i = 0; i < 32; i++)
            {
              //  U1 - svid - SV Id
                ItemsInfo0132[10+i*9] = new UBXItemInfo("svid_"+i, "", 1, 1, ItemInfoType.UINT);
                //U1 - flags - Flags for this SV
                ItemsInfo0132[10 + i * 9 + 1] = new UBXItemInfo("flags_" + i, "", 1, 1, ItemInfoType.UINT);
                //U1 - udre - Monitoring status
                ItemsInfo0132[10 + i * 9 + 2] = new UBXItemInfo("udre_" + i, "", 1, 1, ItemInfoType.UINT);
                //U1 - svSys - System (WAAS/EGNOS/...)
                ItemsInfo0132[10 + i * 9 + 3] = new UBXItemInfo("svsys_" + i, "", 1, 1, ItemInfoType.UINT);
                //U1 - svService - Services available
                ItemsInfo0132[10 + i * 9 + 4] = new UBXItemInfo("svservice_" + i, "", 1, 1, ItemInfoType.UINT);
                //U1 - reserved1 - Reserved
                ItemsInfo0132[10 + i * 9 + 5] = new UBXItemInfo("reserved1_" + i, "", 1, 1, ItemInfoType.UINT);
                //I2 - prc cm Pseudo Range correction in [cm]
                ItemsInfo0132[10 + i * 9 + 6] = new UBXItemInfo("prc_" + i, "", 2, 1, ItemInfoType.INT);

                //U2 - reserved2 - Reserved
                ItemsInfo0132[10 + i * 9 + 7] = new UBXItemInfo("reserved2_" + i, "", 2, 1, ItemInfoType.UINT);
                //I2 - ic cm Ionosphere correction in [cm]
                ItemsInfo0132[10 + i * 9 + 8] = new UBXItemInfo("ic_" + i, "", 2, 1, ItemInfoType.INT);
            }

        }

        static void InitialItemsInfo0230()
           {

               ItemsInfo0230 = new UBXItemInfo[]
            {
                new UBXItemInfo("ID","",2,1),
                new UBXItemInfo("svid","",4,1),
                new UBXItemInfo("week","",4,1),
                new UBXItemInfo("dwrd0","",4,1),
                new UBXItemInfo("dwrd1","",4,1),
                new UBXItemInfo("dwrd2","",4,1),
                new UBXItemInfo("dwrd3","",4,1),
                new UBXItemInfo("dwrd4","",4,1),
                new UBXItemInfo("dwrd5","",4,1),
                new UBXItemInfo("dwrd6","",4,1),
                new UBXItemInfo("dwrd7","",4,1),
            };

           }
        static void InitialItemsInfo0231()
        {

            ItemsInfo0231 = new UBXItemInfo[]
            {
                new UBXItemInfo("ID","",2,1),
                new UBXItemInfo("svid","",4,1),
                new UBXItemInfo("how","",4,1),
                new UBXItemInfo("sf1d0","",4,1),
                new UBXItemInfo("sf1d1","",4,1),
                new UBXItemInfo("sf1d2","",4,1),
                new UBXItemInfo("sf1d3","",4,1),
                new UBXItemInfo("sf1d4","",4,1),
                new UBXItemInfo("sf1d5","",4,1),
                new UBXItemInfo("sf1d6","",4,1),
                new UBXItemInfo("sf1d7","",4,1),
                new UBXItemInfo("sf2d0","",4,1),
                new UBXItemInfo("sf2d1","",4,1),
                new UBXItemInfo("sf2d2","",4,1),
                new UBXItemInfo("sf2d3","",4,1),
                new UBXItemInfo("sf2d4","",4,1),
                new UBXItemInfo("sf2d5","",4,1),
                new UBXItemInfo("sf2d6","",4,1),
                new UBXItemInfo("sf2d7","",4,1),
                new UBXItemInfo("sf3d0","",4,1),
                new UBXItemInfo("sf3d1","",4,1),
                new UBXItemInfo("sf3d2","",4,1),
                new UBXItemInfo("sf3d3","",4,1),
                new UBXItemInfo("sf3d4","",4,1),
                new UBXItemInfo("sf3d5","",4,1),
                new UBXItemInfo("sf3d6","",4,1),
                new UBXItemInfo("sf3d7","",4,1),
              
            };

        }
        static void InitialItemsInfo0101()
        {
            //0 U4 - iTOW ms GPS Millisecond Time of Week
            //4 I4 - ecefX cm ECEF X coordinate
            //8 I4 - ecefY cm ECEF Y coordinate
            //12 I4 - ecefZ cm ECEF Z coordinate
            //16 U4 - pAcc cm Position Accuracy Estimate

            ItemsInfo0101 = new UBXItemInfo[]
            {
                new UBXItemInfo("ID","",2,1),
                new UBXItemInfo("itow","ms",4,1, ItemInfoType.UINT),
                new UBXItemInfo("ecefx","cm",4,1, ItemInfoType.INT),
                new UBXItemInfo("ecefy","cm",4,1, ItemInfoType.INT),
                new UBXItemInfo("ecefz","cm",4,1, ItemInfoType.INT),
                new UBXItemInfo("pacc","cm",4,1, ItemInfoType.UINT)
            };
        }
        public int itow
        {
            get
            {
                return System.Convert.ToInt32(this["itow"] / 1000.0);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (this.GetMessageID() == 0x0220)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (i != 0)
                        sb.Append(" ");
                    string content = (i == 0) ? string.Format("0x{0:X4}", (int)this[i]) : (this[i]).ToString();
                    sb.Append(ItemsInfo[i].Name + ":" + content + " " + ItemsInfo[i].Unit);
                    if (i % 4 == 0)
                        sb.Append("\n");
                    else
                        sb.Append("\t");
                }
                int numsv = this["numsv"] < 100 ? (int)this["numsv"] : 100;
                for (int i = 0; i < numsv * 5; i++)
                {
                    sb.Append(ItemsInfo[5+i].Name + ":" + this[5+i] + " " + ItemsInfo[i].Unit);
                    if (i % 4 == 0)
                        sb.Append("\n");
                    else
                        sb.Append("\t");
                }
                return sb.ToString();
            }
            else if (this.GetMessageID() == 0x0210)
            {
             

                for (int i = 0; i < 5; i++)
                {
                    if (i != 0)
                        sb.Append(" ");
                    string content = (i == 0) ? string.Format("0x{0:X4}", (int)this[i]) : (this[i]).ToString();
                    sb.Append(ItemsInfo[i].Name + ":" + content + " " + ItemsInfo[i].Unit);
                    if (i % 4 == 0)
                        sb.Append("\n");
                    else
                        sb.Append("\t");
                }
                int numsv = this["numsv"] < 32 ? (int)this["numsv"] : 32;
                for (int i = 0; i < numsv * 5; i++)
                {
                    sb.Append(ItemsInfo[5 + i].Name + ":" + this[5+i] + " " + ItemsInfo[i].Unit);
                    if (i % 4 == 0)
                        sb.Append("\n");
                    else
                        sb.Append("\t");
                }
                return sb.ToString();

            }
            else if (this.GetMessageID() == 0x0132)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (i != 0)
                        sb.Append(" ");
                    string content = (i == 0) ? string.Format("0x{0:X4}", (int)this[i]) : (this[i]).ToString();
                    sb.Append(ItemsInfo[i].Name + ":" + content + " " + ItemsInfo[i].Unit);
                    if (i % 4 == 0)
                        sb.Append("\n");
                    else
                        sb.Append("\t");
                }
                int numsv = this["cnt"] < 32 ? (int)this["cnt"] : 32;
                for (int i = 0; i < numsv * 9; i++)
                {
                    sb.Append(ItemsInfo[5 + i].Name + ":" + this[5 + i] + " " + ItemsInfo[i].Unit);
                    if (i % 4 == 0)
                        sb.Append("\n");
                    else
                        sb.Append("\t");
                }
                return sb.ToString();



            }
            else
            {
                for (int i = 0; i < ItemsInfo.Length; i++)
                {
                    if (i != 0)
                        sb.Append(" ");
                    string content = (i == 0) ? string.Format("0x{0:X4}", (int)this[i]) : (this[i]).ToString();
                    sb.Append(ItemsInfo[i].Name + ":" + content + " " + ItemsInfo[i].Unit);
                    if (i % 4 == 0)
                        sb.Append("\n");
                    else
                        sb.Append("\t");
                }

                sb.Append("\nTotal Length:" + GetTotalLength() + "\n");
                return sb.ToString();
            }
         
        }

        public double alm_Eccentricity
        {
            get
            {
                //byte[] d = System.BitConverter.GetBytes((ushort)this["dwrd0"]);
                //byte[] res = new byte[4];//{0xff,0xff,0xff,0xff};
                //System.Array.Copy(d, res, 2);
                //return System.BitConverter.ToSingle(res, 0);
                return ((uint)this["dwrd0"] & 0x0000ffff) * Math.Pow(2, -21);

            }
        }

        public double alm_toa  // time of application
        {
            get
            {
                return (((uint)this["dwrd1"] & 0x00ffffff) >> 16) * Math.Pow(2, 12);
            }
        }


        public double alm_delti
        {
            get
            {

                byte[] temp = System.BitConverter.GetBytes(((uint)this["dwrd1"] & 0x0000ffff));
                //   byte[] data = new byte[2];

                // System.Array.Copy(temp, data, 2);
                //uint raw=((uint)this["dwrd1"] & 0x00ffffff)>>8;
                //data[0]=(byte)(raw % 256);
                //data[1]=(byte)(raw /256);

                return System.BitConverter.ToInt16(temp, 0) * Math.PI * Math.Pow(2, -19) + 0.3 * Math.PI;
            }
        }


        public double alm_sqrtA
        {
            get
            {
                return (((uint)this["dwrd3"] & 0x00ffffff)) * Math.Pow(2, -11);
            }
        }

        public double alm_omegadot
        {
            get
            {
                byte[] temp = System.BitConverter.GetBytes(((uint)this["dwrd2"] & 0x00ffffff) >> 8);
                // byte[] data = new byte[2];
                ////data[3] = ((data[2] & 0x80) != 0) ? (byte)0xff : (byte)0x00;
                //data[0] = temp[0];
                //data[1] = temp[1];
                return System.BitConverter.ToInt16(temp, 0) * Math.Pow(2, -38) * Math.PI;
            }
        }
        public double alm_omega0
        {
            get
            {
                byte[] data = System.BitConverter.GetBytes((uint)this["dwrd4"] & 0x00ffffff);

                data[3] = ((data[2] & 0x80) != 0) ? (byte)0xff : (byte)0x00;

                return System.BitConverter.ToInt32(data, 0) * Math.Pow(2, -23) * Math.PI;
            }
        }

        public double[] eph_xyz(double t)
        {
            if (!this.IsValid)
                return new double[] { 0, 0, 0 };

            double mu = 3986005e8;
            double omgEdot = 7.2921151467e-5;
            double A = eph_sqrtA * eph_sqrtA;
            double n0 = Math.Sqrt(mu / (A * A * A));
            double n = n0 + eph_deltan;
            double tk = t - eph_toe;
            // int iter = 0;
            while (tk > 302400)
            {
                tk -= 604800;
            }

            while (tk < -302400)
            {
                tk += 604800;
            }

            double mk = eph_m0 + n * tk;
            double ek = mk;
            double sep = 1;
            double oldek = ek;
            int iter = 0;
            while (sep > 1e-13)
            {
                ek = mk + eph_e * Math.Sin(ek);
                sep = Math.Abs(ek - oldek);
                oldek = ek;
                iter++;
                if (iter > 10)
                    break;
            }

            double sin_nu_k = ((Math.Sqrt(1 - eph_e * eph_e)) * Math.Sin(ek)) / (1 - eph_e * Math.Cos(ek));
            double cos_nu_k = (Math.Cos(ek) - eph_e) / (1 - eph_e * Math.Cos(ek));
            double nu_k = Math.Atan2(sin_nu_k, cos_nu_k);
            double phik = nu_k + eph_w;
            // 2012.6.11 append
            double deltu = eph_cus * Math.Cos(2 * phik) + eph_cuc * Math.Sin(2 * phik);
            double deltr = eph_crc * Math.Cos(2 * phik) + eph_crs * Math.Sin(2 * phik);
            double delti = eph_cic * Math.Cos(2 * phik) + eph_cis * Math.Sin(2 * phik);
            double uc = phik + deltu;


            double rc = A * (1 - eph_e * Math.Cos(ek))+deltr;

            double inclr = eph_i0 + eph_idot * tk+delti;

            double ip1 = rc * Math.Cos(uc);
            double ip2 = rc * Math.Sin(uc);
            double omgk = eph_w0 + (eph_wdot - omgEdot) * tk - omgEdot * eph_toe;
            double cosomg = Math.Cos(omgk);
            double sinomg = Math.Sin(omgk);
            double cosi = Math.Cos(inclr);
            double sini = Math.Sin(inclr);
            double[] xyz = new double[3];
            xyz[0] = ip1 * cosomg - ip2 * cosi * sinomg;
            xyz[1] = ip1 * sinomg + ip2 * cosi * cosomg;
            xyz[2] = ip2 * sini;

            return xyz;

        }

        public double alm_mean0
        {
            get
            {
                byte[] data = System.BitConverter.GetBytes((uint)this["dwrd6"] & 0x00ffffff);

                data[3] = ((data[2] & 0x80) != 0) ? (byte)0xff : (byte)0x00;

                return System.BitConverter.ToInt32(data, 0) * Math.Pow(2, -23) * Math.PI;
                // return (((uint)this["dwrd4"] & 0x00ffffff)) * Math.Pow(2, -11);
            }

        }

        public double alm_w
        {

            get
            {
                byte[] data = System.BitConverter.GetBytes((uint)this["dwrd5"] & 0x00ffffff);

                data[3] = ((data[2] & 0x80) != 0) ? (byte)0xff : (byte)0x00;

                return System.BitConverter.ToInt32(data, 0) * Math.Pow(2, -23) * Math.PI;
                // return (((uint)this["dwrd4"] & 0x00ffffff)) * Math.Pow(2, -11);
            }

        }

        public double alm_af0
        {
            get
            {
                //byte[] data = new byte[2];

                //ushort temp =(ushort)( System.BitConverter.GetBytes((uint)this["dwrd7"] & 0x00ffffff)[0]<<3);

                //temp |=(ushort)( System.BitConverter.GetBytes(((uint)this["dwrd7"] & 0x00ffffff) >> 19)[0] & 0x07);

                //data = System.BitConverter.GetBytes(temp);


                //if ((data[1] & 0x04) != 0)
                //    data[1]  |= (byte)0xf8;
                //else
                //    data[1]  &=  (byte)0x07;

                //return System.BitConverter.ToInt16(data, 0) * Math.Pow(2, -20);

                uint orgdata = (uint)this["dwrd7"] & 0x00ffffff;

                uint part1 = (orgdata >> 2) & 0x07;
                uint part2 = (orgdata >> 16) << 3;
                byte[] data = System.BitConverter.GetBytes(part1 + part2);
                if ((data[1] & 0x04) != 0)
                    data[1] |= 0xf8;
                else
                    data[1] &= 0x07;

                return System.BitConverter.ToInt16(data, 0) * Math.Pow(2, -20);

            }
        }



        public double alm_af1
        {
            get
            {
                //byte[] data = System.BitConverter.GetBytes(((uint)this["dwrd7"] & 0x00ffffff) >> 8);


                //if ((data[1] & 0x40) != 0)
                //    data[1] |= 0xf8;
                //else
                //    data[1] &= 0x07;


                //return System.BitConverter.ToInt16(data, 0) * Math.Pow(2, -38);

                byte[] data = System.BitConverter.GetBytes((((uint)this["dwrd7"] & 0x00ffffff) >> 5));

                if ((data[1] & 0x04) != 0)
                    data[1] |= 0xf8;
                else
                    data[1] &= 0x07;
                return System.BitConverter.ToInt16(data, 0) * Math.Pow(2, -38);

            }
        }


        public double eph_toc
        {

            get
            {

                //old 
                //byte[] data = System.BitConverter.GetBytes((uint)this["sf1d4"]);
                //return System.BitConverter.ToUInt16(data, 0) * Math.Pow(2, 4);
                byte[] data = System.BitConverter.GetBytes((uint)this["sf1d5"]);
                return System.BitConverter.ToUInt16(data, 0) * Math.Pow(2, 4);
            }

        }

        public double eph_af2
        {

            get
            {
                byte[] data = System.BitConverter.GetBytes((((uint)this["sf1d6"]) >> 16) & 0x000000ff);
                return (sbyte)data[0] * Math.Pow(2, -55);
            }
        }
        public double eph_af1
        {

            get
            {
                byte[] data = System.BitConverter.GetBytes((((uint)this["sf1d6"])) & 0x0000ffff);
                return System.BitConverter.ToInt16(data, 0) * Math.Pow(2, -43);
            }
        }

        public double eph_af0
        {
            get
            {
                byte[] data = System.BitConverter.GetBytes((((uint)this["sf1d7"]) >> 2) & 0x003fffff);
                if ((data[2] & 0x20) != 0)
                {
                    data[3] = 0xff;
                    data[2] |= 0xc0;
                }
                else
                {
                    data[3] = 0;
                    data[2] &= 0x3f;
                }

                return System.BitConverter.ToInt32(data, 0) * Math.Pow(2, -31);
            }
        }


        public double eph_crs
        {

            get
            {
                byte[] data = System.BitConverter.GetBytes((((uint)this["sf2d0"])) & 0x0000ffff);
                return System.BitConverter.ToInt16(data, 0) * Math.Pow(2, -5);
            }


        }

        public double eph_deltan
        {
            get
            {
                byte[] data = System.BitConverter.GetBytes((((uint)this["sf2d1"] >> 8)) & 0x0000ffff);
                return System.BitConverter.ToInt16(data, 0) * Math.Pow(2, -43) * Math.PI;
            }
        }

        public double eph_m0
        {

            get
            {
                byte[] data = System.BitConverter.GetBytes((((uint)this["sf2d2"])));
                data[3] = System.BitConverter.GetBytes((uint)this["sf2d1"])[0];
                return System.BitConverter.ToInt32(data, 0) * Math.Pow(2, -31) * Math.PI;
            }
        }

        public double eph_cuc
        {
            get
            {
                byte[] data = System.BitConverter.GetBytes((((uint)this["sf2d3"] >> 8)) & 0x0000ffff);
                return System.BitConverter.ToInt16(data, 0) * Math.Pow(2, -29);
            }
        }
        public double eph_e
        {
            get
            {
                byte[] data = System.BitConverter.GetBytes((((uint)this["sf2d4"])));
                data[3] = System.BitConverter.GetBytes((uint)this["sf2d3"])[0];
                return System.BitConverter.ToUInt32(data, 0) * Math.Pow(2, -33);
            }
        }

        public double eph_cus
        {
            get
            {
                byte[] data = System.BitConverter.GetBytes((((uint)this["sf2d5"] >> 8)) & 0x0000ffff);
                return System.BitConverter.ToInt16(data, 0) * Math.Pow(2, -29);
            }

        }

        public double eph_sqrtA
        {
            get
            {
                byte[] data = System.BitConverter.GetBytes((((uint)this["sf2d6"])));
                data[3] = System.BitConverter.GetBytes((uint)this["sf2d5"])[0];
                return System.BitConverter.ToUInt32(data, 0) * Math.Pow(2, -19);
            }
        }
        public double eph_toe
        {
            get
            {
                byte[] data = System.BitConverter.GetBytes((((uint)this["sf2d7"]) >> 8));
                return System.BitConverter.ToUInt16(data, 0) * Math.Pow(2, 4);
            }

        }

        public double eph_cic
        {
            get
            {
                byte[] data = System.BitConverter.GetBytes((((uint)this["sf3d0"]) >> 8));
                return System.BitConverter.ToInt16(data, 0) * Math.Pow(2, -29);
            }
        }

        public double eph_w0
        {
            get
            {
                byte[] data = System.BitConverter.GetBytes((((uint)this["sf3d1"])));
                data[3] = System.BitConverter.GetBytes((uint)this["sf3d0"])[0];
                return System.BitConverter.ToInt32(data, 0) * Math.Pow(2, -31) * Math.PI;
            }
        }

        public double eph_cis
        {
            get
            {
                byte[] data = System.BitConverter.GetBytes((((uint)this["sf3d2"]) >> 8));
                return System.BitConverter.ToInt16(data, 0) * Math.Pow(2, -29);
            }
        }

        public double eph_i0
        {
            get
            {
                byte[] data = System.BitConverter.GetBytes((((uint)this["sf3d3"])));
                data[3] = System.BitConverter.GetBytes((uint)this["sf3d2"])[0];
                return System.BitConverter.ToInt32(data, 0) * Math.Pow(2, -31) * Math.PI;
            }
        }

        public double eph_crc
        {
            get
            {
                byte[] data = System.BitConverter.GetBytes((((uint)this["sf3d4"]) >> 8));
                return System.BitConverter.ToInt16(data, 0) * Math.Pow(2, -5);
            }
        }

        public double eph_w
        {
            get
            {
                byte[] data = System.BitConverter.GetBytes((((uint)this["sf3d5"])));
                data[3] = System.BitConverter.GetBytes((uint)this["sf3d4"])[0];
                return System.BitConverter.ToInt32(data, 0) * Math.Pow(2, -31) * Math.PI;
            }

        }

        public double eph_wdot
        {
            get
            {
                byte[] data = System.BitConverter.GetBytes((((uint)this["sf3d6"])));
                if ((data[2] & 0x80) != 0)
                    data[3] = 0xff;
                else
                    data[3] = 0x00;
                return System.BitConverter.ToInt32(data, 0) * Math.Pow(2, -43) * Math.PI;
            }

        }

        public double eph_idot
        {

            get
            {
                byte[] data = System.BitConverter.GetBytes((((uint)this["sf3d7"]) >> 2) & 0x3fff);
                if ((data[1] & 0x20) != 0)
                    data[1] |= 0xc0;
                else
                    data[1] &= 0x3f;

                return System.BitConverter.ToInt16(data, 0) * Math.PI * Math.Pow(2, -43);

            }
        }




    }

   public enum ItemInfoType
   {
       UINT,
       INT,
       DOUBLE
   }

   public class UBXItemInfo
   {
       public string Name, Unit;
       public int Length;
       public double Scale;

       public ItemInfoType ItemType = ItemInfoType.UINT;
       public UBXItemInfo(string name, string unit, int len, double scale)
           
       {
           this.Name = name;
           this.Unit = unit;
           this.Length = len;
           this.Scale = scale;
           this.ItemType = ItemInfoType.UINT;

       }

       public UBXItemInfo(string name, string unit, int len, double scale,ItemInfoType infotype)
       {
           this.Name = name;
           this.Unit = unit;
           this.Length = len;
           this.Scale = scale;
           this.ItemType = infotype;
       }
       //public UBXItemInfo(string name, string unit, int len, double scale, bool isDouble)
       //{
       //    this.Name = name;
       //    this.Unit = unit;
       //    this.Length = len;
       //    this.Scale = scale;
       //    this.ItemType = ItemInfoType.DOUBLE;
       //}



   }
}
