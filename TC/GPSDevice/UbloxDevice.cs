using System;
using System.Collections.Generic;
using System.Text;
using Comm;
using GPSDevice.GPSMessage;

namespace GPSDevice
{
  public  class UbloxDevice : GPSDeviceBase
    {


      int eph_cnt = 0;
     public    GPSMessage.UBIDBase[] eph_datas = new GPSMessage.UBIDBase[32];
    //  double[,] GpsDataMatrix = new double[33, 50];
      UBIDBase idbase0122, idbase0210, idbase0220,idbase0132,idbase0101;


     // const int constHatchQueueCnt = 100;
     // int ValidDataCnt = 0;
    //  long LastTimeStamp = 0;
    //  System.Collections.Generic.Queue<GPSData> queue = new System.Collections.Generic.Queue<GPSData>();


      public UbloxDevice(int id, UbloxGPSController controller, string GPSName, string ComName, int baud, double refx, double refy, double refz, bool IsRefreence)
          : this(id, controller, GPSName, ComName, baud)
        {

            this.IsReference = true;
            this.refx = refx;
            this.refy = refy;
            this.refz = refz;
           

        }
            public UbloxDevice(int id, UbloxGPSController controller, string GPSName, string ComName, int baud)
            : base(id, controller,GPSName, ComName, baud)
        {
          //  this.DeviceName= GPSName;
          //  this.IsReference = IsReference;
            //this.controller = controller;
            //new System.Threading.Thread(ProcessGpsSignal).Start();

        }
            public UbloxDevice(int id, UbloxGPSController controller, string GPSName, System.Net.IPEndPoint endpoint, double refx, double refy, double refz,bool IsReference)
                : base(id, controller, GPSName, endpoint)
        {
            this.IsReference = IsReference;
            this.refx = refx;
            this.refy = refy;
            this.refz = refz;
        }
        //    public UbloxDevice(int id, UbloxGPSController controller, string GPSName, System.Net.IPEndPoint endpoint)
        //    : base(id,controller, GPSName, endpoint)
        //{
          
        //    //this.controller = controller;
        //    //new System.Threading.Thread(ProcessGpsSignal).Start();
          
         
        //}



        protected override void PrepareGPSDataMatrix()
        {
            if (eph_cnt != 32)
            {
                Console.WriteLine("Waiting Eph!");
                return;
            }
            if ((idbase0122.itow & idbase0220.itow & idbase0210.itow & idbase0132.itow & idbase0101.itow   ) != idbase0220.itow  )  
            {
                Console.WriteLine("Time base error");
                return;
            }

            for (int row = 0; row < 33; row++)
                for (int col = 0; col < 50; col++)
                    GpsDataMatrix[row, col] = 0;

            for (int svid = 1; svid < 32; svid++)
            {
                if (eph_datas[svid - 1].IsValid)
                {
                    GpsDataMatrix[svid - 1, 30] =eph_datas[svid - 1].eph_af0;// +eph_datas[svid - 1].eph_af1 * (idbase0220.itow - eph_datas[svid - 1].eph_toc) + eph_datas[svid - 1].eph_af2 * (idbase0220.itow - eph_datas[svid - 1].eph_toc) * (idbase0220.itow - eph_datas[svid - 1].eph_toc);
                    GpsDataMatrix[svid - 1, 31] = eph_datas[svid - 1].eph_af1;
                    GpsDataMatrix[svid - 1, 49]++;  //有效記數
                }
            }
                // 0x01 0x22
                //clkB (ns) =main(31,5)
                //clkD (ns/s) =main(31,4)
 
                GpsDataMatrix[31, 5] = idbase0122["clkb"];
                GpsDataMatrix[31, 4] = idbase0122["clkd"];

                GpsDataMatrix[32, 0] = idbase0101["ecefx"]/100;
                GpsDataMatrix[32, 1] = idbase0101["ecefy"]/100;
                GpsDataMatrix[32, 2] = idbase0101["ecefz"]/100;

                //RXM-SVSI (0x02 0x20)
                //week=main(31,1)
                //iTOW (ms)=main(31,2)
                //azim=main(1:ID,36)
                //elev=main(1:ID,37)
                GpsDataMatrix[31, 1] = idbase0220["week"];
                GpsDataMatrix[31, 2] = idbase0220.itow * 1000;
                for (int i = 0; i < idbase0220["numsv"]; i++)
                {
                    int svid = (int)idbase0220["svid_" + i];
                    if (svid < 1 || svid > 31)
                        continue;

                    GpsDataMatrix[svid - 1,49]++;

                    GpsDataMatrix[svid-1,36]=idbase0220["azim_"+i];
                    GpsDataMatrix[svid - 1, 37] = idbase0220["elev_" + i];

                }
            // (0x02 0x10)
            //iTOW (ms)=main(1:ID,0)
            //cpMes =main(1:ID,3)
            //prMes (m)=main(1:ID,1)
            //domes (HZ)=main(1:ID,2)
                for (int i = 0; i < idbase0210["numsv"]; i++)
                {
                    int svid = (int)idbase0210["sv_" + i];

                    if (svid < 1 || svid > 31  || idbase0210["mesQI_"+i] <7   )
                        continue;
                    GpsDataMatrix[svid - 1, 47] = idbase0210["mesQI_" + i];
                    GpsDataMatrix[svid - 1, 0] = idbase0210.itow*1000;
                    GpsDataMatrix[svid - 1, 3] = idbase0210["cpmes_"+i];
                    GpsDataMatrix[svid - 1, 1] = idbase0210["prmes_" + i];
                    GpsDataMatrix[svid - 1, 2] = idbase0210["domes_" + i];
                    GpsDataMatrix[svid - 1, 49]++;
                }

                //NAV-SBAS (0x01 0x32)
                //ic=main(1:ID,35)
                for (int i = 0; i < idbase0132["cnt"]; i++)
                {
                    int svid =(int) idbase0132["svid_" + i];
                    if (svid < 1 || svid > 31)
                        continue;

                    GpsDataMatrix[svid - 1, 35] = idbase0132["ic_" + i];
                    GpsDataMatrix[svid - 1, 48] = idbase0132["prc_" + i];
                    GpsDataMatrix[svid - 1, 49]++;
                }

                //SVPos X=main1(1:ID,24)
                //SVPos Y=main1(1:ID,25)
                //SVPos Z=main1(1:ID,26)

                for (int i = 0; i < 31; i++)
                {
                    double[] xyz;
                    if (!eph_datas[i].IsValid)
                        continue;

                    xyz =eph_datas[i].eph_xyz(idbase0220.itow);
                    GpsDataMatrix[i, 24] = xyz[0];
                    GpsDataMatrix[i, 25] = xyz[1];
                    GpsDataMatrix[i, 26] = xyz[2];

                    GpsDataMatrix[i, 49]++;
                }




                for (int i = 0; i < 31; i++)
                {
                    if (GpsDataMatrix[i, 49] != 5)
                        GpsDataMatrix[i, 0] = 0;
                }


                double id2posx = GpsDataMatrix[32, 0], id2posy = GpsDataMatrix[32, 1], id2posz = GpsDataMatrix[32, 2];



                double virtualPseudoRange;
                for (int i = 0; i < 31; i++)
                {
                    if (GpsDataMatrix[i, 0] == 0)
                        continue;
                    if (this.IsReference)
                    {
                        virtualPseudoRange = Norm(new double[] { refx - GpsDataMatrix[i, 24], refy - GpsDataMatrix[i, 25], refz - GpsDataMatrix[i, 26] });
                    }
                    else
                    {
                        virtualPseudoRange = Norm(new double[] { id2posx - GpsDataMatrix[i, 24], id2posy - GpsDataMatrix[i, 25], id2posz - GpsDataMatrix[i, 26] });
                        // id2 修正 Pseudo Range 2011.7.11
                        //  GpsDataMatrix[i, 1] = virtualPseudoRange;

                    }
                    // 2011.9.23
                    //  if (!(GpsDataMatrix[i, 1] > 3e5 && GpsDataMatrix[i, 1] <  4e7 /*3.5e7*/))  // check pseudo range
                    if (!(virtualPseudoRange > 3e5 && virtualPseudoRange < 4e7))
                        GpsDataMatrix[i, 0] = 0;

                    if (GpsDataMatrix[i, 37] < 15) // check elev
                        GpsDataMatrix[i, 0] = 0;
                }


                //for (int row = 0; row < 33; row++)
                //{
                //    Console.Write("[");
                //    for (int col = 0; col < 50; col++)
                //        Console.Write(GpsDataMatrix[row, col] + "\t");
                //    Console.Write("]");
                //    Console.WriteLine();
                //}
#if DEBUG
            //  string path=AppDomain.CurrentDomain.BaseDirectory+"logs";
            // if(!System.IO.Directory.Exists(path))
            //     System.IO.Directory.CreateDirectory(path);
            //DateTime dt=System.DateTime.Now;
            // string filename = string.Format("{0:00}{1:00}{2:00}{3:00}{4:00}_{5:00}.txt",dt.Year%100,dt.Month,dt.Day,dt.Hour,(dt.Minute/20)*20,this.SensorName);


            //    System.IO.File.AppendAllText(path+"\\"+filename, ToMathLabArray(GpsDataMatrix) + "\r\n");
#endif

                System.Collections.ArrayList ArySatelliteIds = new System.Collections.ArrayList();

                for (int row = 0; row < 31; row++)
                {
                    if (GpsDataMatrix[row, 0] != 0)
                        ArySatelliteIds.Add(row);
                }

                if (ArySatelliteIds.Count < 5) //至少5顆 101.10.1
                {
                    ValidDataCnt = 0;
                    Console.WriteLine(SensorName + "not enough satellite number," + ArySatelliteIds.Count);
                    return;//有效衛星數不足

                }

                ValidDataCnt++;
                if (ValidDataCnt > constMinValidDataCnt)
                    ValidDataCnt = constMinValidDataCnt;

                double[,] svxyx = new double[ArySatelliteIds.Count, 3];
                int[] satelliteIds = new int[ArySatelliteIds.Count];
                double[] Pr = new double[ArySatelliteIds.Count];
                double[] initpos = new double[] { 0, 0, 0, 0 };
                double[] carrier_phase = new double[ArySatelliteIds.Count];
                double[] sv_clock_bais = new double[ArySatelliteIds.Count];
                double[] sv_iono_delay = new double[ArySatelliteIds.Count];
                double[] pr_correct = new double[ArySatelliteIds.Count];
                double[] sv_elv = new double[ArySatelliteIds.Count];

                int rowinx = 0;
                foreach (int matrixrow in ArySatelliteIds)
                {
                    svxyx[rowinx, 0] = GpsDataMatrix[matrixrow, 24];
                    svxyx[rowinx, 1] = GpsDataMatrix[matrixrow, 25];
                    svxyx[rowinx, 2] = GpsDataMatrix[matrixrow, 26];

                    Pr[rowinx] = GpsDataMatrix[matrixrow, 1];
                    satelliteIds[rowinx] = matrixrow + 1;
                    carrier_phase[rowinx] = GpsDataMatrix[matrixrow, 3];// -GpsDataMatrix[31, 5] * 2.99792458e-1 / 1575420000;
                    sv_clock_bais[rowinx] = GpsDataMatrix[matrixrow, 30];
                    sv_iono_delay[rowinx] = GpsDataMatrix[matrixrow, 35];
                    pr_correct[rowinx] = GpsDataMatrix[matrixrow, 48];
                    sv_elv[rowinx] = GpsDataMatrix[matrixrow, 37] / 180.0 * Math.PI;
                    rowinx++;

                }
                long currentTimeStamp = (int)GpsDataMatrix[31, 2];
                //  if (currentTimeStamp - LastTimeStamp != 1   && LastTimeStamp !=0 )
                if (currentTimeStamp == LastTimeStamp && LastTimeStamp != 0)
                {
                    ValidDataCnt = 0;
                    LastTimeStamp = currentTimeStamp;
                    return;
                }

                LastTimeStamp = currentTimeStamp;
                lock (queue)
                {
                    queue.Enqueue(new GPSData()
                    {
                        initpos = initpos,
                        Prr = Pr,
                        CPrr = carrier_phase,
                        svxyz = svxyx,
                        tol = 1e-3,
                        IsTrackFinished = (ValidDataCnt < constMinValidDataCnt) ? false : true,
                        TrackCnt = ValidDataCnt,
                        TimeStamp = currentTimeStamp,
                        rec_clock_bias = GpsDataMatrix[31, 5],
                        rec_clock_drif = GpsDataMatrix[31, 4],
                        satelliteIds = satelliteIds,
                        id2x = id2posx,
                        id2y = id2posy,
                        id2z = id2posz,
                        IsReference = this.IsReference,
                        refx = this.refx,
                        refy = this.refy,
                        refz = this.refz,
                        sv_clock_bais = sv_clock_bais,
                        sv_iono_delay = sv_iono_delay,
                        sv_elv = sv_elv,
                        pr_correct=pr_correct
                     //   eph_datas=this.eph_datas

                    });

                    if (this.IsReference)
                        System.Threading.Monitor.Pulse(queue);
                    else
                    {
                        if (queue.Count > constMinValidDataCnt)
                        {//clear time out data exceed 10 sec

                            GPSData data = queue.Dequeue();
                            Console.WriteLine(this.SensorName + ": clear  {0} data exceeed {1} sec", data.TimeStamp, constMinValidDataCnt);
                            ValidDataCnt = 0;
                        }
                    }
                }



        }





        public override void sensorDev_OnReceiveText(object sender, TextPackage txtObj)
        {
            if (txtObj.Text[0] == 0x02 && txtObj.Text[1] == 0x31)  //eph data
            {

                UBIDBase idbase = new UBIDBase(txtObj.Text);
                if (++eph_cnt >= 32)
                    eph_cnt = 32;
               
                if (idbase == null)
                    return;
                eph_datas[idbase.svid - 1] = idbase;
                Console.WriteLine(this.SensorName+" svid:" + idbase.svid+" "+ idbase.IsValid);
                //if (idbase.IsValid)
                //{
                //    eph_datas[idbase.svid - 1] = idbase;
                //    // double[] xyz=idbase.eph_xyz(0);
                //    //   Console.WriteLine("svid={0} x:{1} y:{2} z:{3} at t=0", idbase.svid,xyz[0], xyz[1], xyz[2]);
                //}
                //else
                //    eph_datas[idbase.svid - 1] = null;


            }
            else if (txtObj.Text[0] == 0x01 && txtObj.Text[1] == 0x22)
            {
                //Console.WriteLine("01 22");

                UBIDBase idbase = new UBIDBase(txtObj.Text);
            //    Console.WriteLine("0x01 0x22, itow=" + System.Convert.ToInt32(idbase["itow"] / 1000.0));
                idbase0122 = idbase;
                //   Console.WriteLine(idbase0122.ToString());
            }
            else if (txtObj.Text[0] == 0x02 && txtObj.Text[1] == 0x20)
            {
                UBIDBase idbase = new UBIDBase(txtObj.Text);
           //     Console.WriteLine("0x02 0x20, itow=" + System.Convert.ToInt32(idbase["itow"] / 1000.0));
                idbase0220 = idbase;
                PrepareGPSDataMatrix();
                // Console.WriteLine(idbase0220.ToString());
            }
            else if (txtObj.Text[0] == 0x02 && txtObj.Text[1] == 0x10)
            {
                UBIDBase idbase = new UBIDBase(txtObj.Text);
                idbase0210 = idbase;
               // Console.WriteLine("0x02 0x10, itow=" + System.Convert.ToInt32(idbase["itow"] / 1000.0));
             //   Console.WriteLine(idbase0210.ToString());
            }
            else if (txtObj.Text[0] == 0x01 && txtObj.Text[1] == 0x32)
            {
                UBIDBase idbase = new UBIDBase(txtObj.Text);
                idbase0132 = idbase;
               // Console.WriteLine("0x01 0x32, itow=" + idbase.itow);
             //   Console.WriteLine(idbase0132.ToString());
            }
            else if (txtObj.Text[0] == 0x01 && txtObj.Text[1] == 0x01)
            {
                UBIDBase idbase = new UBIDBase(txtObj.Text);
                idbase0101 = idbase;
              //  Console.WriteLine("0x01 0x01, itow=" + idbase.itow);


            }
            else if (txtObj.Text[0] == 0x02 && txtObj.Text[1] == 0x30)
            {
                // dummy
            }
            else
            {
                UBIDBase idbase = new UBIDBase(txtObj.Text);
                Console.WriteLine("unknow id {0:X4}", idbase.GetMessageID());
            }

        }

        public override void SensorDev_OnCommError(object sender)
        {
            this.ValidDataCnt = 0;
           // throw new NotImplementedException();
        }
    }
}
