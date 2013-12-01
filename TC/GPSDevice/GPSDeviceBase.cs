using System;
using System.Collections.Generic;
using System.Text;
using GPSDevice.GPSMessage;
using Comm;
using MatrixLibrary;
using Comm.Controller;

namespace GPSDevice
{
    public delegate void ProcessCompletedHandler(GPSDeviceBase sender, GPSData data);
    public abstract  class GPSDeviceBase:SensorBase
    {
        public event ProcessCompletedHandler ProcessCompletedEvent;
        public static readonly int constMinValidDataCnt =51;
        const int constHatchQueueCnt = 50 ;
        protected  int ValidDataCnt = 0;
       protected long LastTimeStamp = 0;
        protected double[,] GpsDataMatrix = new double[33, 50];
        protected System.Collections.Generic.Queue<GPSData> queue = new System.Collections.Generic.Queue<GPSData>();
        public bool IsReference = false;
        public double refx, refy, refz;
        protected ControllerBase controller;
        double constLambda = 2.99792458e8 / 1575420000;

      
       public GPSDeviceBase(int id,ControllerBase controller,  string GPSName, string ComName, int baud, double refx, double refy, double refz) : this(id,controller,GPSName, ComName, baud)
        {
          
            this.IsReference = true;
            this.refx = refx;
            this.refy = refy;
            this.refz = refz;
           

        }
       public GPSDeviceBase(int id, ControllerBase controller, string GPSName, string ComName, int baud)
            : base(id,GPSName, ComName, baud)
        {
          //  this.DeviceName= GPSName;

            this.controller = controller;
            new System.Threading.Thread(ProcessGpsSignal).Start();
          
        }
       public GPSDeviceBase(int id, ControllerBase controller, string GPSName, System.Net.IPEndPoint endpoint, double refx, double refy, double refz)
           : this(id, controller, GPSName, endpoint)
        {
            this.IsReference = true;
            this.refx = refx;
            this.refy = refy;
            this.refz = refz;
        }
       public GPSDeviceBase(int id, ControllerBase controller, string GPSName, System.Net.IPEndPoint endpoint)
            : base(id, GPSName, endpoint)
        {
           // this.DeviceName= GPSName;
           // this.endpoint = endpoint;
          //  com = new System.IO.Ports.SerialPort(ComName, baud, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
          //  com.Open();
            this.controller = controller;
            new System.Threading.Thread(ProcessGpsSignal).Start();
          
         
        }
       public override void sensorDev_OnReceiveText(object sender, TextPackage txtObj)
        {
            throw new NotImplementedException();
        }

        public void NotifyPrecessCompleted(GPSData gpsdata)
        {

            if (this.ProcessCompletedEvent != null)
                ProcessCompletedEvent(this, gpsdata);
        }

         public void Signal()
        {
            lock (queue)
            {
                if (queue.Count > 0)
                    System.Threading.Monitor.Pulse(queue);
            }

        }


        protected double[] Hatch(double[] PrCprDiff, double[] adr, int[] svid, int smint, double[][] prmat)
        {


            if (prmat[0] == null)
            {

                prmat[0] = new double[31];
                //  adrmat[0] = new double[31];

                for (int i = 0; i < svid.Length; i++)
                {
                    prmat[0][svid[i] - 1] = PrCprDiff[i];
                    // adrmat[0][svid[i] - 1] = adr[i];
                }

            }

            else
            {
                for (int i = prmat.GetLength(0) - 1; i >= 1; i--)
                {
                    prmat[i] = prmat[i - 1];
                    // adrmat[i] = adrmat[i - 1];
                }
                prmat[0] = new double[31];
                for (int i = 0; i < svid.Length; i++)
                {

                    prmat[0][svid[i] - 1] = PrCprDiff[i];
                    // adrmat[0][svid[i] - 1] = adr[i];
                }
            }


            double[] prsm = new double[svid.Length];
            for (int i = 0; i < svid.Length; i++)
            {
                int col = svid[i] - 1;
                int cnt = 0;
                double sum = 0;
                for (int row = 0; row < prmat.GetLength(0) && prmat[row] != null; row++)
                {

                    sum += prmat[row][col];
                    if (prmat[row][col] != 0)
                        cnt++;

                }
                prsm[i] = sum / cnt + adr[i];
            }
            //for (int sid = 0; i < svid.Length; sid++)
            //{
            //    prmat
            //}


            return prsm;
        }

        protected bool IsSVIDEqual(int[] svid1, int[] svid2)
        {
            if (svid1.Length != svid2.Length)
                return false;
            else
            {
                System.Array.Sort(svid1);
                System.Array.Sort(svid2);
                for (int i = 0; i < svid1.Length; i++)
                {
                    if (svid1[i] != svid2[i])
                        return false;
                }
            }

            return true;

        }
        Matrix position_cp=new Matrix(0,0);
        Matrix position_smpr = new Matrix(0, 0);
        double[] smest_NL;
        double[] last_dd_comadr;
        Matrix last_position_cp=new Matrix(0,0);
        Matrix ref_position_cp=new Matrix(0,0);
        protected  void ProcessGpsSignal()
        {

            double[] xhat = null;
            double[] yn = null;
            double[,] px = null;
            double[,] r = Matrix.ScalarMultiply(50, Matrix.Identity(8));
            double[][] prmat = new double[constHatchQueueCnt][], adrmat = new double[constHatchQueueCnt][];
            int[] lastsvid = null; ;
            GPSData data = null;
           
            while (true)
            {
                try
                {
                    lock (queue)
                    {
                        if (queue.Count == 0)
                        {

                            //do
                            //{
                            System.Threading.Monitor.Wait(queue);
                            // } while (queue.Peek().TimeStamp > controller.refGpsDataQueue.ToArray()[0].TimeStamp);

                        }
                        else
                        {
                            if (!IsReference)
                            {
                                do
                                {
                                    GPSData[] refdatas = ((UbloxGPSController)controller).refGpsDataQueue.ToArray();
                                    System.Array.Sort(refdatas);

                                    if (refdatas.Length == 0 || queue.Peek().TimeStamp > refdatas[refdatas.Length - 1].TimeStamp)
                                        System.Threading.Monitor.Wait(queue);
                                    else
                                        break;

                                } while (true);
                            }
                        }
                        data = queue.Dequeue();

                    }
                    if (!IsReference)
                    {

                        if (!CorrectGpsData(data))
                        {
                            this.ValidDataCnt = 0;
                            data.TrackCnt = 0;
                            continue;
                        }

                    }
                    if (lastsvid == null)
                        lastsvid = data.satelliteIds;
                    else
                    {
                        if (!IsSVIDEqual(lastsvid, data.satelliteIds))
                        {
                            Console.WriteLine(this.SensorName+"===========SVID not Equal!===============");
                            this.ValidDataCnt = 0;
                            
                            lastsvid = data.satelliteIds;
                           // continue;
                        }
                    }
                    //if (!this.IsReference)
                    //{
                    //    for (int i = 0; i < data.Pr.Length; i++)
                    //        data.Pr[i] = Norm(new double[] { refx - data.svxyx[i, 0], refy - data.svxyx[i, 1], refz - data.svxyx[i, 2] });

                    //}
                    //else
                    //{
                    //   // double virtualPseudoRange = Norm(new double[] { id2posx - GpsDataMatrix[i, 24], id2posy - GpsDataMatrix[i, 25], id2posz - GpsDataMatrix[i, 26] });
                    //    // id2 修正 Pseudo Range 2011.7.11
                    // //   GpsDataMatrix[i, 1] = virtualPseudoRange;
                    //}

                    double[] position;
                    
                    

           double[] prsm = Hatch(data.PrrCpDifference, data.CPrr, data.satelliteIds, constHatchQueueCnt, prmat);

                    data.Prsm = prsm;
                    if (this.IsReference)
                    
                        position = CalcPosition(data.Pr/*data.Pr*/, data.svxyz, data.initpos, data.tol);

                    else
                       position = CalcPosition(data.Pr, data.svxyz, data.initpos, data.tol);
                    //data.refx,y,z 主站座標
                    
                   

                    double[] basePosition ;

                 

                    double[] trpref = new double[data.satelliteIds.Length];
                    double[,]comadr=new double[data.satelliteIds.Length,2];
                    double[] sd_comadr = new double[data.satelliteIds.Length];
                    double[] dd_comadr = new double[data.satelliteIds.Length-1];
                    double[,] compr = new double[data.satelliteIds.Length, 2];
                    double[] sd_compr = new double[data.satelliteIds.Length];
                    double[] dd_compr = new double[data.satelliteIds.Length-1];
                    double[] td_adr = new double[data.satelliteIds.Length - 1];
                   
                    double[,] unit_vec = new double[data.satelliteIds.Length, 3];
                    double[,] H = new double[data.satelliteIds.Length - 1,3];

                    if (!this.IsReference && data.TrackCnt >= constMinValidDataCnt)
                    {
                        if (position_cp.NoRows == 0)
                            basePosition = new double[] { (position[0] + data.basex) / 2, (position[1] + data.basey) / 2, (position[2] + data.basez) / 2 };
                        else
                        {
                            //basePosition = new double[] { (position_smpr[0, 0] + data.refx) / 2, (position_smpr[1, 0] + data.refy) / 2, (position_smpr[2, 0] + data.refz) / 2 };
                            basePosition = new double[] { (position_cp[0, 0] + data.basex) / 2, (position_cp[1, 0] + data.basey) / 2, (position_cp[2, 0] + data.basez) / 2 };
                            //  basePosition = new double[] { (position[0] + data.refx) / 2, (position[1] + data.refy) / 2, (position[2] + data.refz) / 2 };
                        }



                        for (int i = 0; i < trpref.Length; i++)
                        {
                            trpref[i] = Norm(new double[] { data.svxyz[i, 0] - data.basex, data.svxyz[i, 1] - data.basey, data.svxyz[i, 2] - data.basez });
                            comadr[i, 0] = data.CPrr[i];
                            comadr[i, 1] = data.refCprr[i];
                            compr[i, 0] = trpref[i];

                            compr[i, 1] = data.Pr[i];//data.Pr[i];// data.Prsm[i];
                            //sd_comadr[i] = comadr[i,1] - comadr[i, 0];
                            //sd_compr[i] = compr[i, 1] - compr[i,0];

                            //double[] vec = new double[3];
                            //vec[0] = data.svxyz[i, 0] - basePosition[0];
                            //vec[1] = data.svxyz[i, 1] - basePosition[1];
                            //vec[2] = data.svxyz[i, 2] - basePosition[2];
                            //double norm=Norm(vec);
                            //unit_vec[i, 0] = vec[0] / norm;
                            //unit_vec[i, 1] = vec[1] / norm;
                            //unit_vec[i, 2] = vec[2] / norm;
                        }
                        int maxelvsvinx = data.MaxElvIndex;

                        #region TestProcedure

                        //basePosition = new double[] { -3026891.0304, 4928955.0748, 2678906.0368 };
                        //compr = new double[,] { 
                        //        {21871437.7818,21796147.342},
                        //        {21887501.493,21812224.6426},
                        //        {24254321.1366,24179037.7336},
                        //        {20498328.3772,20423063.67},
                        //        {24376262.6299,24301010.9362},
                        //        {20161982.62178,20086712.56577},
                        //        {22047083.7739,21971797.7195}
                        //    };
                        //comadr = new double[,]{
                        //        {-10565223.0407,-62197.7856},
                        //        {-8681472.0157,-937820.3205},
                        //        {-3816781.3606,-1690424.558},
                        //        {-7625225.857,-3888117.6702},
                        //        {-2595602.5742,-2268866.4089},
                        //        {-8435759.6664,-4757404.5004},
                        //        {-1922044.1608,-1726811.9773}

                        //    };

                        //data.svxyz = new double[,] { 
                        //    {-22535023.4917,13227286.9544,-2699738.3306},
                        //    {-17923710.9538,6059500.4886,18674818.2003},
                        //    {-18170283.0538,-4369469.1133,19186143.1744},
                        //    {-5038380.6595,24023240.8373,9858305.5867},
                        //    {10420441.7522,12362433.0103,21602842.4992},
                        //    {-12306288.884,14264355.9134,17951385.6614},
                        //    {-16102048.5857,18657263.4999,-8574865.8383}


                        //    };
                        //maxelvsvinx = 3;
                        //sd_comadr = new double[compr.GetLength(0)];
                        //dd_comadr = new double[compr.GetLength(0) - 1];
                        //sd_compr = new double[compr.GetLength(0)];
                        //dd_compr = new double[compr.GetLength(0) - 1];
                        //unit_vec = new double[compr.GetLength(0), 3];
                        //H = new double[compr.GetLength(0) - 1, 3];
                        #endregion

                        for (int i = 0; i < compr.GetLength(0); i++)
                        {
                            sd_comadr[i] = comadr[i, 1] - comadr[i, 0];
                            sd_compr[i] = compr[i, 1] - compr[i, 0];

                            double[] vec = new double[3];
                            vec[0] = data.svxyz[i, 0] - basePosition[0];
                            vec[1] = data.svxyz[i, 1] - basePosition[1];
                            vec[2] = data.svxyz[i, 2] - basePosition[2];
                            double norm = Norm(vec);
                            unit_vec[i, 0] = vec[0] / norm;
                            unit_vec[i, 1] = vec[1] / norm;
                            unit_vec[i, 2] = vec[2] / norm;
                        }

                        //compr = new double[,] {{2,7 },{3,11}};
                        //comadr = new double[,] { { 1,7 },{9,5} };
                        //unit_vec = new double[,] { {1,3,5},{11,13,17} };
                        //sd_comadr = new double[] { 6, -4 };
                        //sd_compr = new double[] { 5,8};




                        //  maxelvsvinx = 1;
                        int fillinx = 0;
                        for (int i = 0; i < compr.GetLength(0); i++)
                        {
                            if (i != maxelvsvinx)
                            {
                                dd_comadr[fillinx] = sd_comadr[maxelvsvinx] - sd_comadr[i];
                                dd_compr[fillinx] = sd_compr[maxelvsvinx] - sd_compr[i];
                                for (int k = 0; k < 3; k++)
                                    H[fillinx, k] = unit_vec[maxelvsvinx, k] - unit_vec[i, k];
                                fillinx++;
                            }
                        }

                     //   bool IsFirstComAdr = false ;
                        if (last_dd_comadr != null)
                        {
                            if (last_dd_comadr.Length != dd_comadr.Length)
                            {
                                last_dd_comadr = dd_comadr;
                                throw new Exception("last_dd_comadr!=dd_comadr");
                            }
                            for (int i = 0; i < dd_comadr.Length; i++)
                            {
 
                                    td_adr[i] = dd_comadr[i] - last_dd_comadr[i];
                                
                                
                            }
                        }
                        //else
                        //    IsFirstComAdr = true;

                        last_dd_comadr = dd_comadr;
                        //double [,]Q,R;
                        //Matrix.QR(H,out Q,out R);
                        //double[,] QT = Matrix.Transpose(Q);
                        //double[,] Q_beta = new double[3, H.GetLength(0)];
                        //double[,] R_beta = new double[3,3];
                        //for (int row = 0; row < 3; row++)
                        //{
                        //    for (int col = 0; col < H.GetLength(0); col++)
                        //        Q_beta[row, col] = QT[row, col];
                        //    for (int col = 0; col < 3; col++)
                        //        R_beta[row, col] = R[row, col];
                        //}

                        //Matrix base_est_smpr = new Matrix(Matrix.Inverse(R_beta))
                        //    * new Matrix(Q_beta) * new Matrix(Matrix.OneD_2_TwoD(dd_compr));

                        //position_smpr = new Matrix(Matrix.OneD_2_TwoD(new double[] { data.refx, data.refy, data.refz })) - base_est_smpr;

                      //  double[] 

                        if (data.TrackCnt == constMinValidDataCnt && smest_NL==null)
                        {
                            smest_NL = new double[dd_comadr.Length];
                            for (int i = 0; i < dd_comadr.Length; i++)
                            {
                                double smest_N = Math.Round(1 / constLambda * (dd_comadr[i] - dd_compr[i]));
                                smest_NL[i] = smest_N * constLambda;

                            }
                            Console.WriteLine("==============new lamda================track cnt="+data.TrackCnt);
                        }
                            Matrix base_est_cp = Matrix.PINV(new Matrix(H)) *
                                (new Matrix(Matrix.OneD_2_TwoD(dd_comadr)) - new Matrix(Matrix.OneD_2_TwoD(smest_NL)));

                         Matrix   position_dd = new Matrix(Matrix.OneD_2_TwoD(new double[] { data.basex, data.basey, data.basez })) - base_est_cp;

                            if (ref_position_cp.NoRows==0)
                                ref_position_cp = new Matrix(new double[,]{{data.refx},{data.refy},{data.refz}});


                            position_cp = ref_position_cp - (Matrix.PINV(new Matrix(H)) * new Matrix(Matrix.OneD_2_TwoD(td_adr)));
                            
                             

                    }

                    if (data.TrackCnt < constMinValidDataCnt)
                    {
                        smest_NL = null;

                        Console.WriteLine(this.SensorName + ": trackcnt=" + data.TrackCnt);
                    }
                    else if (!data.IsReference && data.TrackCnt>=constMinValidDataCnt)
                    {
                        this.SetDataToQueue(new double[] { position_cp[0,0], position_cp[1,0], position_cp[2,0] });
                        Console.WriteLine(this.SensorName + "x:{0:0.0000}/{5:0.0000},y:{1:0.0000}/{6:0.0000},z:{2:0.0000}/{7:0.0000} timestamp:{3}   TrackCnt={4}",position_cp[0,0], position_cp[1,0], position_cp[2,0], data.TimeStamp, data.TrackCnt, data.id2x, data.id2y, data.id2z);
#if DEBUG
                        if (data.TrackCnt == constMinValidDataCnt)
                            System.IO.File.AppendAllText(SensorName + "_xyz.log",
                                 string.Format("{0},{1},{2},{3},{4},{5}\r\n", DateTime.Now, data.TimeStamp, position_cp[0, 0], position_cp[1, 0], position_cp[2, 0], data.TrackCnt));
#endif
                    }
                    if (ProcessCompletedEvent != null)
                        ProcessCompletedEvent(this, data);

                    #region old algorithm


                    //if (/*data.TrackCnt == 1 ||*/ xhat == null)
                    //{
                    //    xhat = new double[12] { position[0], position[1], position[2], 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    //    px = Matrix.ScalarMultiply(50.0, Matrix.Identity(12));

                    //}

                    //yn = new double[] { position[0], position[1], position[2], data.rec_clock_bias * 1e-9, 0, 0, 0, data.rec_clock_drif };


//                    KFLSA(ref xhat, yn, ref px, r, 1);
//#if DEBUG
//                    if (data.TrackCnt == constMinValidDataCnt)
//                        System.IO.File.AppendAllText(SensorName + "_xyz.log",
//                            string.Format("{0},{1},{2},{3},{4},{5}\r\n", DateTime.Now, data.TimeStamp, xhat[0], xhat[1], xhat[2], data.TrackCnt));
//#endif
//                    if (data.TrackCnt < constMinValidDataCnt)
//                        Console.WriteLine(this.SensorName + ": trackcnt=" + data.TrackCnt);
//                    else if (!data.IsReference)
//                    {
//                        this.SetDataToQueue(new double[] { xhat[0], xhat[1], xhat[2] });
//                        Console.WriteLine(this.SensorName + "x:{0:0.0000}/{5:0.0000},y:{1:0.0000}/{6:0.0000},z:{2:0.0000}/{7:0.0000} timestamp:{3}   TrackCnt={4}", xhat[0], xhat[1], xhat[2], data.TimeStamp, data.TrackCnt, data.id2x, data.id2y, data.id2z);
//                    }

//                    if (ProcessCompletedEvent != null)
//                        ProcessCompletedEvent(this, data);
//                    double[] llh = xyz2llh(new double[] { xhat[0], xhat[1], xhat[2] });
                    //                    //  Console.WriteLine("GPSID:{0} longitude:{1:0.000000} latitude:{2:0.000000}  height:{3:0.00} Timestamp:{4} trackCnt:{5}",this.SensorName, llh[0], llh[1], llh[2],data.TimeStamp,data.TrackCnt);
#endregion
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message + "," + ex.StackTrace);
                }
            }


        }



        protected bool CorrectGpsData(GPSData data)
        {

            GPSData[] refgpsdatas;

            System.Collections.ArrayList list = new System.Collections.ArrayList();
            lock (((UbloxGPSController)controller).refGpsDataQueue)
            {
                refgpsdatas = ((UbloxGPSController)controller).refGpsDataQueue.ToArray();
            }

            //if (data.TimeStamp > refgpsdatas[0].TimeStamp)
            //{
            //    lock (refgpsdatas)
            //    {
            //        System.Threading.Monitor.Wait(refgpsdatas);
            //    }
            //}
            for (int i = 0; i < refgpsdatas.Length; i++)
            {
                if (refgpsdatas[i].TimeStamp != null && data.TimeStamp == refgpsdatas[i].TimeStamp)
                {

                    //   data.SetPrrOffset(refgpsdatas[i].PrOffset,refgpsdatas[i].CPrOffset);
                    data.SetPrsmOffset(refgpsdatas[i], refgpsdatas[i].PrOffset, refgpsdatas[i].CPrOffset);
                    if (data.satelliteIds.Length < 5) //有效衛星數至少5顆
                        return false;

                    return true;
                }

            }

            return false;

        }
        public static double[] xyz2llh(double[] xyz)
        {

            //  double[] xyz = new double[3];

            //xyz[0] = -3.026910817843361e6;
            //xyz[1] = 4.928894479527888e6;
            //xyz[2] = 2.679014313202421e6;

            double x = xyz[0];
            double y = xyz[1];
            double z = xyz[2];
            double x2 = Math.Pow(x, 2);
            double y2 = Math.Pow(y, 2);
            double z2 = Math.Pow(z, 2);

            //function llh = xyz2llh(xyz)
            //%XYZ2LLH	Convert from ECEF cartesian coordinates to 
            //%               latitude, longitude and height.  WGS-84
            //%
            //%	llh = XYZ2LLH(xyz)	
            //%
            //%    INPUTS
            //%	xyz(1) = ECEF x-coordinate in meters
            //%	xyz(2) = ECEF y-coordinate in meters
            //%	xyz(3) = ECEF z-coordinate in meters
            //%
            //%    OUTPUTS
            //%	llh(1) = latitude in radians
            //%	llh(2) = longitude in radians
            //%	llh(3) = height above ellipsoid in meters

            //    x = xyz(1);
            //    y = xyz(2);
            //    z = xyz(3);
            //    x2 = x^2;
            //    y2 = y^2;
            //    z2 = z^2;

            double a = 6378137.0000;
            double b = 6356752.3142;
            double e = Math.Sqrt(1 - Math.Pow((b / a), 2));
            double b2 = b * b;
            double e2 = Math.Pow(e, 2);
            double ep = e * (a / b);
            double r = Math.Sqrt(x2 + y2);
            double r2 = r * r;
            double E2 = Math.Pow(a, 2) - Math.Pow(b, 2);
            double F = 54 * b2 * z2;
            double G = r2 + (1 - e2) * z2 - e2 * E2;
            double c = (e2 * e2 * F * r2) / (G * G * G);
            double s = Math.Pow((1 + c + Math.Sqrt(c * c + 2 * c)), 1.0 / 3.0);
            double P = F / (3 * Math.Pow((s + 1 / s + 1), 2) * G * G);
            double Q = Math.Sqrt(1 + 2 * e2 * e2 * P);
            double ro = -(P * e2 * r) / (1 + Q) + Math.Sqrt((a * a / 2) * (1 + 1 / Q) - (P * (1 - e2) * z2) / (Q * (1 + Q)) - P * r2 / 2);
            double tmp = Math.Pow((r - e2 * ro), 2);
            double U = Math.Sqrt(tmp + z2);
            double V = Math.Sqrt(tmp + (1 - e2) * z2);
            double zo = (b2 * z) / (a * V);

            //a = 6378137.0000;	% earth radius in meters
            //b = 6356752.3142;	% earth semiminor in meters	
            //e = sqrt (1-(b/a).^2);
            //b2 = b*b;
            //e2 = e^2;
            //ep = e*(a/b);
            //r = sqrt(x2+y2);
            //r2 = r*r;
            //E2 = a^2 - b^2;
            //F = 54*b2*z2;
            //G = r2 + (1-e2)*z2 - e2*E2;
            //c = (e2*e2*F*r2)/(G*G*G);
            //s = ( 1 + c + sqrt(c*c + 2*c) )^(1/3);
            //P = F / (3 * (s+1/s+1)^2 * G*G);
            //Q = sqrt(1+2*e2*e2*P);
            //ro = -(P*e2*r)/(1+Q) + sqrt((a*a/2)*(1+1/Q) ...
            //                            - (P*(1-e2)*z2)/(Q*(1+Q)) - P*r2/2);
            //tmp = (r - e2*ro)^2;
            //U = sqrt( tmp + z2 );
            //V = sqrt( tmp + (1-e2)*z2 );
            //zo = (b2*z)/(a*V);

            double height = U * (1 - b2 / (a * V));

            //height = U*( 1 - b2/(a*V) );
            double lat = Math.Atan((z + ep * ep * zo) / r);
            //lat = atan( (z + ep*ep*zo)/r );
            double temp = Math.Atan(y / x);
            //temp = atan(y/x);
            double Long;
            if (x >= 0)
                Long = temp;
            else if ((x < 0) && (y >= 0))
                Long = Math.PI + temp;
            else
                Long = temp - Math.PI;


            //if x >=0	
            //    long = temp;
            //elseif (x < 0) & (y >= 0)
            //    long = pi + temp;
            //else
            //    long = temp - pi;
            //end

            //double[] llh = new double[3];

            //llh[0] = lat;
            //llh[1] = Long;
            //llh[2] = height;

            //llh(1) = lat;
            //llh(2) = long;
            //llh(3) = height;

            return new double[] { Long * 180.0 / Math.PI, lat * 180.0 / Math.PI, height };//                       (llh[1].ToString()+"/n"+llh[2]+"/n"+llh[0]);

        }

        protected double[] CalcPosition(double[] Pr, double[,] GpsPos, double[] initpos, double tol)
        {
            double[] beta = new double[] { 1e9, 1e9, 1e9, 1e9 };
            double[] Y = new double[GpsPos.GetLength(0)];
            double[] estuser = new double[initpos.Length];
            System.Array.Copy(initpos, estuser, initpos.Length);
            double maxiter = 10;
            int iter = 0;

            while (iter < maxiter && Norm(beta) > tol)
            {

                for (int row = 0; row < GpsPos.GetLength(0); row++)
                {
                    double pr0 = 0;
                    double[] temp = new double[3];
                    for (int col = 0; col < 3; col++)
                        temp[col] = GpsPos[row, col] - estuser[col];


                    pr0 = Norm(temp);

                    Y[row] = Pr[row] - pr0 - estuser[3];


                }

                double[,] h = Hmat(GpsPos, estuser);
                // Matrix my=new Matrix(Matrix.OneD_2_TwoD(Y));
                beta = Matrix.TwoD_2_OneD((Matrix.PINV(new Matrix(h)) * new Matrix(Matrix.OneD_2_TwoD(Y))).toArray);
                for (int col = 0; col < estuser.Length; col++)
                    estuser[col] += beta[col];

                iter++;

            }

            return estuser;
        }

        protected double[,] Hmat(double[,] gpspos, double[] usrpos)
        {
            //  double[] temppos = new double[usrpos.Length];
            //   System.Array.Copy(usrpos, temppos, 4);
            //  double b = Norm(temppos);
            double[,] ret = new double[gpspos.GetLength(0), 4];
            for (int row = 0; row < gpspos.GetLength(0); row++)
            {
                double[] temp = new double[3];

                for (int col = 0; col < 3; col++)
                {
                    temp[col] = usrpos[col] - gpspos[row, col];
                }

                double normdata = Norm(temp);
                for (int col = 0; col < 3; col++)
                {
                    ret[row, col] = (usrpos[col] - gpspos[row, col]) / normdata;

                }
                ret[row, 3] = 1;


            }
            return ret;

        }


        protected double Norm(double[] vect)
        {


            double ret = 0;
            for (int i = 0; i < vect.Length; i++)
            {
                ret += Math.Pow(vect[i], 2);
            }
            return Math.Sqrt(ret);

        }

        protected string ToMathLabArray(double[,] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int row = 0; row < data.GetLength(0); row++)
            {
                for (int col = 0; col < data.GetLength(1); col++)
                {
                    sb.Append(data[row, col]);
                    if (col != data.GetLength(1) - 1)
                        sb.Append(",");
                }

                if (row != data.GetLength(0) - 1)
                    sb.Append(";");
            }

            return "[" + sb.ToString() + "]";
        }
        protected abstract void PrepareGPSDataMatrix();

        protected void KFLSA(ref  double[] xhat, double[] yn, ref double[,] px, double[,] r, double sampletime)
        {

            Matrix q = 3 * Matrix.MergeMatrix(
                  new Matrix[,] { 
                      {0.05*Math.Pow(sampletime,5)* new Matrix(Matrix.Identity(4)),1.0/8.0 * Math.Pow(sampletime,4)*new Matrix(Matrix.Identity(4)),1.0/6.0 * Math.Pow(sampletime,3)*new Matrix(Matrix.Identity(4)) },
                      {1.0/8.0*Math.Pow(sampletime,4) * new Matrix(Matrix.Identity(4)),1.0/3.0 * Math.Pow(sampletime,3)*new Matrix(Matrix.Identity(4)),1.0/2.0 * Math.Pow(sampletime,2)*new Matrix(Matrix.Identity(4)) } , 
                      {1.0/6.0*Math.Pow(sampletime,3)*new Matrix(Matrix.Identity(4)),1.0/2.0 * Math.Pow(sampletime,2)*new Matrix(Matrix.Identity(4)),1.0/1.0 * Math.Pow(sampletime,1)*new Matrix(Matrix.Identity(4)) }  
                  }
                );

            Matrix an = Matrix.MergeMatrix(
                    new Matrix[,]{ 
                        { new Matrix(Matrix.Identity(4)), sampletime * new Matrix(Matrix.Identity(4)),0.5 * Math.Pow(sampletime,2)*new Matrix(Matrix.Identity(4))},
                        { new Matrix(4,4),  new Matrix(Matrix.Identity(4)), sampletime*new Matrix(Matrix.Identity(4))},
                        { new Matrix(4,4),  new Matrix(4,4),new Matrix(Matrix.Identity(4))}
                    }

                );

            Matrix h = Matrix.MergeMatrix(
                   new Matrix[,]{

                      { new Matrix(Matrix.Identity(4)),new Matrix(4,4),new Matrix(4,4)},
                      { new Matrix(4,4),new Matrix(Matrix.Identity(4)),new Matrix(4,4)}

                   }

                );

            Matrix pxminus = an * new Matrix(px) * Matrix.Transpose(an) + q;


            Matrix xhatminus = an * new Matrix(Matrix.OneD_2_TwoD(xhat));

            Matrix kx = pxminus * Matrix.Transpose(h) * Matrix.Inverse(h * pxminus * Matrix.Transpose(h) + new Matrix(r));
            px = ((new Matrix(Matrix.Identity(12)) - kx * h) * pxminus).toArray;
            Matrix tmp = xhatminus + kx * (new Matrix(Matrix.OneD_2_TwoD(yn)) - h * xhatminus);
            xhat = Matrix.TwoD_2_OneD(tmp.toArray);
            //Console.WriteLine(Matrix.PrintMat(tmp));
            //Console.WriteLine(Matrix.PrintMat(px));

        }

        protected bool HasErr = false;
    }
}
