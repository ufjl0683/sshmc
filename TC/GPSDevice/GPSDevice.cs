using System;
using System.Collections.Generic;
using System.Text;
using GPSDevice.GPSMessage;
using MatrixLibrary;
using Comm;


namespace GPSDevice  //Sirf GPS Device
{
    //
 
    public   class GPSDevice:GPSDeviceBase
    {
      
     //   public event ProcessCompletedHandler ProcessCompletedEvent;
     //    public static  readonly int constMinValidDataCnt=100;
        const int constHatchQueueCnt = 100;
        int ValidDataCnt = 0;
        long LastTimeStamp = 0;
        System.Collections.Generic.Queue<GPSData> queue = new System.Collections.Generic.Queue<GPSData>();
        
      //  System.IO.Ports.SerialPort com;
      //  Comm.SirfDLE delDev;
        //public string GPSName;
        double [,]  GpsDataMatrix=new double[33,50];
        System.Collections.Generic.Dictionary<double, GPSMessage.IDBase> Dict28 = new Dictionary<double, GPSMessage.IDBase>();
        System.Collections.Generic.Dictionary<double, GPSMessage.IDBase> Dict30 = new Dictionary<double, GPSMessage.IDBase>();
        IDBase ID4,ID2,ID7,ID29;
      //  public bool IsReference = false;
      //  public double refx, refy, refz;

     
        public GPSDevice(int id,SirfGPSController controller,  string GPSName, string ComName, int baud, double refx, double refy, double refz) : this(id,controller,GPSName, ComName, baud)
        {

            this.IsReference = true;
            this.refx = refx;
            this.refy = refy;
            this.refz = refz;
           

        }
        public GPSDevice(int id,SirfGPSController controller, string GPSName, string ComName, int baud)
            : base(id, controller,GPSName, ComName, baud)
        {
          //  this.DeviceName= GPSName;

            //this.controller = controller;
            //new System.Threading.Thread(ProcessGpsSignal).Start();
          
        }
        public GPSDevice(int id,SirfGPSController controller,string GPSName, System.Net.IPEndPoint endpoint, double refx, double refy, double refz):this(id,controller, GPSName,endpoint)
        {
            this.IsReference = true;
            this.refx = refx;
            this.refy = refy;
            this.refz = refz;
        }
        public GPSDevice(int id, SirfGPSController controller, string GPSName, System.Net.IPEndPoint endpoint)
            : base(id,controller, GPSName, endpoint)
        {
          
            //this.controller = controller;
            //new System.Threading.Thread(ProcessGpsSignal).Start();
          
         
        }

    
     

        //public void Signal()
        //{
        //    lock (queue)
        //    {
        //        if(queue.Count>0)
        //                 System.Threading.Monitor.Pulse(queue);
        //    }

        //}

       
        //double[] Hatch( double[] PrCprDiff,double[]adr,int[] svid, int smint, double[][] prmat)
        //{

           
        //    if (prmat[0] == null)
        //    {
               
        //        prmat[0] = new double[31];
        //      //  adrmat[0] = new double[31];

        //        for (int i = 0; i < svid.Length; i++)
        //        {
        //            prmat[0][svid[i] - 1] = PrCprDiff[i];
        //           // adrmat[0][svid[i] - 1] = adr[i];
        //        }

        //    }

        //    else
        //    {
        //        for (int i = prmat.GetLength(0) - 1; i >= 1; i--)
        //        {
        //            prmat[i] = prmat[i - 1];
        //           // adrmat[i] = adrmat[i - 1];
        //        }
        //        prmat[0] = new double[31];
        //        for (int i = 0; i < svid.Length; i++)
        //        {
                  
        //            prmat[0][svid[i] - 1] = PrCprDiff[i];
        //            // adrmat[0][svid[i] - 1] = adr[i];
        //        }
        //    }
         

        //    double[] prsm = new double[svid.Length];
        //    for(int i=0;i<svid.Length;i++)
        //    {
        //        int col=svid[i]-1;
        //        int cnt = 0;
        //        double sum = 0;
        //        for (int row = 0; row < prmat.GetLength(0)  && prmat[row]!=null; row++)
        //        {

        //            sum += prmat[row][col];
        //            if (prmat[row][col] != 0)
        //                cnt++;
                    
        //        }
        //        prsm[i] = sum/cnt+adr[i];
        //    }
        //    //for (int sid = 0; i < svid.Length; sid++)
        //    //{
        //    //    prmat
        //    //}


        //    return prsm;
        //}

        //bool IsSVIDEqual(int[] svid1, int[] svid2)
        //{
        //    if (svid1.Length != svid2.Length)
        //        return false;
        //    else
        //    {
        //        System.Array.Sort(svid1);
        //            System.Array.Sort(svid2);
        //            for (int i = 0; i < svid1.Length; i++)
        //            {
        //                if (svid1[i] != svid2[i])
        //                    return false;
        //            }
        //    }

        //    return true;

        //}
        //void ProcessGpsSignal()
        //{

        //      double[] xhat=null;
        //      double[] yn=null;
        //      double[,] px=null;
        //      double[,] r=Matrix.ScalarMultiply(50,Matrix.Identity(8));
        //      double[][] prmat = new double[constHatchQueueCnt][], adrmat = new double[constHatchQueueCnt][];
        //      int[] lastsvid = null; ;
        //    GPSData data=null;
        //    while (true)
        //    {
        //        try
        //        {
        //            lock (queue)
        //            {
        //                if (queue.Count == 0)
        //                {

        //                    //do
        //                    //{
        //                    System.Threading.Monitor.Wait(queue);
        //                    // } while (queue.Peek().TimeStamp > controller.refGpsDataQueue.ToArray()[0].TimeStamp);

        //                }
        //                else
        //                {
        //                    if (!IsReference)
        //                    {
        //                        do
        //                        {
        //                            GPSData[] refdatas = controller.refGpsDataQueue.ToArray();
        //                            System.Array.Sort(refdatas);

        //                            if (refdatas.Length == 0 || queue.Peek().TimeStamp > refdatas[refdatas.Length - 1].TimeStamp)
        //                                System.Threading.Monitor.Wait(queue);
        //                            else
        //                                break;

        //                        } while (true);
        //                    }
        //                }
        //                data = queue.Dequeue();

        //            }
        //            if (!IsReference)
        //            {
                        
        //                if (!CorrectGpsData(data))
        //                {
        //                    this.ValidDataCnt = 0;
        //                    continue;
        //                }

        //            }
        //            if (lastsvid == null)
        //                lastsvid = data.satelliteIds;
        //            else
        //            {
        //                if (!IsSVIDEqual(lastsvid, data.satelliteIds))
        //                {
        //                    Console.WriteLine("===========SVID not Equal!===============");
        //                    this.ValidDataCnt = 0;
        //                    lastsvid = data.satelliteIds;
        //                    continue;
        //                }
        //            }
        //            //if (!this.IsReference)
        //            //{
        //            //    for (int i = 0; i < data.Pr.Length; i++)
        //            //        data.Pr[i] = Norm(new double[] { refx - data.svxyx[i, 0], refy - data.svxyx[i, 1], refz - data.svxyx[i, 2] });

        //            //}
        //            //else
        //            //{
        //            //   // double virtualPseudoRange = Norm(new double[] { id2posx - GpsDataMatrix[i, 24], id2posy - GpsDataMatrix[i, 25], id2posz - GpsDataMatrix[i, 26] });
        //            //    // id2 修正 Pseudo Range 2011.7.11
        //            // //   GpsDataMatrix[i, 1] = virtualPseudoRange;
        //            //}

        //            double[] position;

        //            //   position = CalcPosition(data.Pr, data.svxyz, data.initpos, data.tol);
        //            // if (IsReference)
        //            // Pr==>Prc
        //            //   position = CalcPosition(data.Pr, data.svxyz, data.initpos, data.tol);

        //            double[] prsm = Hatch(data.PrrCpDifference, data.CPrr, data.satelliteIds, constHatchQueueCnt, prmat);

        //            data.Prsm = prsm;
        //            if(this.IsReference)
        //                position = CalcPosition(data.Pr/*data.Pr*/, data.svxyz, data.initpos, data.tol);

        //            else
        //                position = CalcPosition(data.Prsm/*data.Pr*/, data.svxyz, data.initpos, data.tol);

        //            //    else
        //            //  position = CalcPosition(data.Pr, data.svxyz, data.initpos, data.tol);
        //            //      ;





        //            if (/*data.TrackCnt == 1 ||*/ xhat == null)
        //            {
        //                xhat = new double[12] { position[0], position[1], position[2], 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //                px = Matrix.ScalarMultiply(50.0, Matrix.Identity(12));

        //            }

        //            yn = new double[] { position[0], position[1], position[2], data.rec_clock_bias * 1e-9, 0, 0, 0, data.rec_clock_drif };


        //            KFLSA(ref xhat, yn, ref px, r, 1);

        //            if (data.TrackCnt == constMinValidDataCnt)
        //            System.IO.File.AppendAllText(SensorName + "_xyz.log",
        //                string.Format("{0},{1},{2},{3},{4},{5}\r\n", DateTime.Now, data.TimeStamp, xhat[0], xhat[1], xhat[2], data.TrackCnt));
                   
        //            Console.WriteLine(this.SensorName + "x:{0:0.0000}/{5:0.0000},y:{1:0.0000}/{6:0.0000},z:{2:0.0000}/{7:0.0000} timestamp:{3}   TrackCnt={4}", xhat[0], xhat[1], xhat[2], data.TimeStamp, data.TrackCnt, data.id2x, data.id2y, data.id2z);
                   
        //          //  Console.WriteLine(this.SensorName + "x:{0:0.0000}/{5:0.0000},y:{1:0.0000}/{6:0.0000},z:{2:0.0000}/{7:0.0000} timestamp:{3}   TrackCnt={4}", position[0], position[1], position[2], data.TimeStamp, data.TrackCnt, data.id2x, data.id2y, data.id2z);
        //        //if (ProcessCompletedEvent != null)
        //        //        ProcessCompletedEvent(this, data);
        //            NotifyPrecessCompleted(data);
        //          double[] llh = xyz2llh(new double[] { xhat[0], xhat[1], xhat[2] });
        //            // Console.WriteLine("GPSID:{0} longitude:{1:0.000000} latitude:{2:0.000000}  height:{3:0.00} Timestamp:{4} trackCnt:{5}",this.GPSName, llh[0], llh[1], llh[2],data.TimeStamp,data.TrackCnt);

        //        }
        //        catch(Exception ex){
        //            Console.WriteLine(ex.Message + "," + ex.StackTrace);
        //        }
        //    }


        //}
      


        //private bool CorrectGpsData(GPSData data)
        //{

        //    GPSData[] refgpsdatas;

        //    System.Collections.ArrayList list = new System.Collections.ArrayList();
        //    lock (controller.refGpsDataQueue)
        //    {
        //        refgpsdatas = controller.refGpsDataQueue.ToArray();
        //    }

        //    //if (data.TimeStamp > refgpsdatas[0].TimeStamp)
        //    //{
        //    //    lock (refgpsdatas)
        //    //    {
        //    //        System.Threading.Monitor.Wait(refgpsdatas);
        //    //    }
        //    //}
        //    for (int i = 0; i < refgpsdatas.Length; i++)
        //    {
        //        if(refgpsdatas[i].TimeStamp!=null && data.TimeStamp==refgpsdatas[i].TimeStamp)
        //        {

        //         //   data.SetPrrOffset(refgpsdatas[i].PrOffset,refgpsdatas[i].CPrOffset);
        //            data.SetPrsmOffset(refgpsdatas[i].PrsmOffset);
        //            if (data.satelliteIds.Length < 4)
        //                return false;

        //            return true;
        //        }
                
        //    }

        //    return false;

        //}
        //public static double[] xyz2llh(double[] xyz)
        //{

        //  //  double[] xyz = new double[3];

        //    //xyz[0] = -3.026910817843361e6;
        //    //xyz[1] = 4.928894479527888e6;
        //    //xyz[2] = 2.679014313202421e6;

        //    double x = xyz[0];
        //    double y = xyz[1];
        //    double z = xyz[2];
        //    double x2 = Math.Pow(x, 2);
        //    double y2 = Math.Pow(y, 2);
        //    double z2 = Math.Pow(z, 2);

        //    //function llh = xyz2llh(xyz)
        //    //%XYZ2LLH	Convert from ECEF cartesian coordinates to 
        //    //%               latitude, longitude and height.  WGS-84
        //    //%
        //    //%	llh = XYZ2LLH(xyz)	
        //    //%
        //    //%    INPUTS
        //    //%	xyz(1) = ECEF x-coordinate in meters
        //    //%	xyz(2) = ECEF y-coordinate in meters
        //    //%	xyz(3) = ECEF z-coordinate in meters
        //    //%
        //    //%    OUTPUTS
        //    //%	llh(1) = latitude in radians
        //    //%	llh(2) = longitude in radians
        //    //%	llh(3) = height above ellipsoid in meters

        //    //    x = xyz(1);
        //    //    y = xyz(2);
        //    //    z = xyz(3);
        //    //    x2 = x^2;
        //    //    y2 = y^2;
        //    //    z2 = z^2;

        //    double a = 6378137.0000;
        //    double b = 6356752.3142;
        //    double e = Math.Sqrt(1 - Math.Pow((b / a), 2));
        //    double b2 = b * b;
        //    double e2 = Math.Pow(e, 2);
        //    double ep = e * (a / b);
        //    double r = Math.Sqrt(x2 + y2);
        //    double r2 = r * r;
        //    double E2 = Math.Pow(a, 2) - Math.Pow(b, 2);
        //    double F = 54 * b2 * z2;
        //    double G = r2 + (1 - e2) * z2 - e2 * E2;
        //    double c = (e2 * e2 * F * r2) / (G * G * G);
        //    double s = Math.Pow((1 + c + Math.Sqrt(c * c + 2 * c)), 1.0 / 3.0);
        //    double P = F / (3 * Math.Pow((s + 1 / s + 1), 2) * G * G);
        //    double Q = Math.Sqrt(1 + 2 * e2 * e2 * P);
        //    double ro = -(P * e2 * r) / (1 + Q) + Math.Sqrt((a * a / 2) * (1 + 1 / Q) - (P * (1 - e2) * z2) / (Q * (1 + Q)) - P * r2 / 2);
        //    double tmp = Math.Pow((r - e2 * ro), 2);
        //    double U = Math.Sqrt(tmp + z2);
        //    double V = Math.Sqrt(tmp + (1 - e2) * z2);
        //    double zo = (b2 * z) / (a * V);

        //    //a = 6378137.0000;	% earth radius in meters
        //    //b = 6356752.3142;	% earth semiminor in meters	
        //    //e = sqrt (1-(b/a).^2);
        //    //b2 = b*b;
        //    //e2 = e^2;
        //    //ep = e*(a/b);
        //    //r = sqrt(x2+y2);
        //    //r2 = r*r;
        //    //E2 = a^2 - b^2;
        //    //F = 54*b2*z2;
        //    //G = r2 + (1-e2)*z2 - e2*E2;
        //    //c = (e2*e2*F*r2)/(G*G*G);
        //    //s = ( 1 + c + sqrt(c*c + 2*c) )^(1/3);
        //    //P = F / (3 * (s+1/s+1)^2 * G*G);
        //    //Q = sqrt(1+2*e2*e2*P);
        //    //ro = -(P*e2*r)/(1+Q) + sqrt((a*a/2)*(1+1/Q) ...
        //    //                            - (P*(1-e2)*z2)/(Q*(1+Q)) - P*r2/2);
        //    //tmp = (r - e2*ro)^2;
        //    //U = sqrt( tmp + z2 );
        //    //V = sqrt( tmp + (1-e2)*z2 );
        //    //zo = (b2*z)/(a*V);

        //    double height = U * (1 - b2 / (a * V));

        //    //height = U*( 1 - b2/(a*V) );
        //    double lat = Math.Atan((z + ep * ep * zo) / r);
        //    //lat = atan( (z + ep*ep*zo)/r );
        //    double temp = Math.Atan(y / x);
        //    //temp = atan(y/x);
        //    double Long;
        //    if (x >= 0)
        //        Long = temp;
        //    else if ((x < 0) && (y >= 0))
        //        Long = Math.PI + temp;
        //    else
        //        Long = temp - Math.PI;


        //    //if x >=0	
        //    //    long = temp;
        //    //elseif (x < 0) & (y >= 0)
        //    //    long = pi + temp;
        //    //else
        //    //    long = temp - pi;
        //    //end

        //    //double[] llh = new double[3];

        //    //llh[0] = lat;
        //    //llh[1] = Long;
        //    //llh[2] = height;

        //    //llh(1) = lat;
        //    //llh(2) = long;
        //    //llh(3) = height;

        //    return new double[] { Long * 180.0 / Math.PI, lat * 180.0 / Math.PI, height };//                       (llh[1].ToString()+"/n"+llh[2]+"/n"+llh[0]);
           
        //}

        //public double[] CalcPosition(double[] Pr, double[,] GpsPos, double[] initpos, double tol)
        //{
        //    double [] beta=new double[]{1e9,1e9,1e9,1e9};
        //    double[] Y = new double[GpsPos.GetLength(0)];
        //    double[] estuser = new double[initpos.Length];
        //    System.Array.Copy(initpos, estuser, initpos.Length);
        //    double maxiter=10;
        //    int iter = 0;

        //    while (iter < maxiter && Norm(beta) > tol)
        //    {

        //        for (int row = 0; row < GpsPos.GetLength(0); row++)
        //        {
        //            double pr0 = 0;
        //            double[] temp = new double[3];
        //            for (int col = 0; col < 3; col++)
        //                temp[col] = GpsPos[row, col] - estuser[col];


        //            pr0 = Norm(temp);

        //            Y[row] = Pr[row] - pr0 - estuser[3];
                  

        //        }

        //        double[,] h = Hmat(GpsPos, estuser);
        //        // Matrix my=new Matrix(Matrix.OneD_2_TwoD(Y));
        //        beta = Matrix.TwoD_2_OneD((Matrix.PINV(new Matrix(h)) * new Matrix(Matrix.OneD_2_TwoD(Y))).toArray);
        //        for (int col = 0; col < estuser.Length; col++)
        //            estuser[col] += beta[col];

        //        iter++;

        //    }

        //    return estuser;
        //}

        //double[,] Hmat(double [,] gpspos,double[] usrpos)
        //{
        //  //  double[] temppos = new double[usrpos.Length];
        // //   System.Array.Copy(usrpos, temppos, 4);
        //  //  double b = Norm(temppos);
        //    double[,] ret = new double[gpspos.GetLength(0), 4];
        //    for (int row = 0; row < gpspos.GetLength(0); row++)
        //    {
        //        double[] temp=new double[3];

        //        for(int col=0;col<3;col++)
        //        {
        //          temp[col]=usrpos[col]-gpspos[row,col];
        //        }

        //        double normdata=Norm(temp);
        //        for (int col = 0; col < 3; col++)
        //        {
        //            ret[row, col] = (usrpos[col] - gpspos[row, col]) / normdata;

        //        }
        //        ret[row, 3] = 1;


        //    }
        //    return ret;

        //}


        //double Norm(double[] vect)
        //{
          

        //    double ret=0;
        //    for (int i = 0; i < vect.Length; i++)
        //    {
        //        ret += Math.Pow(vect[i],2);
        //    }
        //    return Math.Sqrt(ret);

        //}

        //string ToMathLabArray(double[,] data)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    for(int row=0;row <data.GetLength(0);row++)
        //    {
        //        for (int col = 0; col < data.GetLength(1); col++)
        //        {
        //            sb.Append(data[row, col]);
        //            if(col!=data.GetLength(1)-1)
        //                sb.Append(",");
        //        }

        //        if(row!=data.GetLength(0)-1)
        //              sb.Append(";");
        //    }

        //    return "[" + sb.ToString() + "]";
        //}
        protected override void PrepareGPSDataMatrix()
        {
            //  GpsDataMatrix = new double[33, 50];
            if (Dict28.Count != Dict30.Count)
            {
                Console.WriteLine(this.SensorName + "==>28cnt:{0}/ 30cnt:{1}", Dict28.Count, Dict30.Count);
                ValidDataCnt = 0;
                return;
            }
            if (Dict30.Count == 0)
            {
                ValidDataCnt = 0;
                return;
            }
            for (int row = 0; row < 33; row++)
                for (int col = 0; col < 50; col++)
                    GpsDataMatrix[row, col] = 0;

            foreach (IDBase data30 in Dict30.Values)
            {
                IDBase data28;
                data28 = Dict28[data30[1]];// satellite id  
                for (int col = 0; col <= 20; col++)  //col 0~20
                    GpsDataMatrix[(int)data28[3] - 1, col] = data28[4 + col];

                for (int col = 23; col <= 35; col++)  //id30  col 23~35
                    GpsDataMatrix[(int)data30[1] - 1, col] = data30[2 + col - 23];
            }

            //id4
            for (int filedinxbegin = 4; filedinxbegin < 4 + 14 * 12; filedinxbegin += 14)  //col 36 ~ 48
            {
                //if (ID4 == null)
                //    return;
                int salteliteid = (int)ID4[filedinxbegin];
                if (salteliteid <= 0)
                    continue;
                for (int i = 0; i < 13; i++)
                {

                    GpsDataMatrix[salteliteid - 1, 36 + i] = ID4[filedinxbegin + i + 1];
                }
            }

            //id 7
            for (int col = 0; col < ID7.GetItemsCount(); col++)
            {
                GpsDataMatrix[31, col] = ID7[col];
            }

            //id2
            for (int col = 1; col <= 6; col++)
            {
                GpsDataMatrix[32, col - 1] = ID2[col];
            }

#if DEBUG
            System.IO.File.AppendAllText(SensorName + ".log",ToMathLabArray(GpsDataMatrix)+"\r\n");
#endif



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

            System.Collections.ArrayList ArySatelliteIds = new System.Collections.ArrayList();

            for (int row = 0; row < 31; row++)
            {
                if (GpsDataMatrix[row, 0] != 0)
                    ArySatelliteIds.Add(row);
            }

            if (ArySatelliteIds.Count < 4)
            {
                ValidDataCnt = 0;
                Console.WriteLine(SensorName + "有效衛星數不足," + ArySatelliteIds.Count);
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
                    sv_elv = sv_elv

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


            //double[] position = CalcPosition(Pr, svxyx, initpos, 1e-3);





            //Console.WriteLine("x:{0},y:{1},z:{2}", position[0], position[1], position[2]);

            //double []llh=xyz2llh(position);
            //Console.WriteLine("longitude:{0} latitude:{1}  height:{2}", llh[0], llh[1], llh[2]);

        }
        //public  void KFLSA(ref  double[] xhat, double[] yn,ref double[,] px, double[,] r, double sampletime)
        //{

        //    Matrix q =3* Matrix.MergeMatrix( 
        //          new  Matrix[,] { 
        //              {0.05*Math.Pow(sampletime,5)* new Matrix(Matrix.Identity(4)),1.0/8.0 * Math.Pow(sampletime,4)*new Matrix(Matrix.Identity(4)),1.0/6.0 * Math.Pow(sampletime,3)*new Matrix(Matrix.Identity(4)) },
        //              {1.0/8.0*Math.Pow(sampletime,4) * new Matrix(Matrix.Identity(4)),1.0/3.0 * Math.Pow(sampletime,3)*new Matrix(Matrix.Identity(4)),1.0/2.0 * Math.Pow(sampletime,2)*new Matrix(Matrix.Identity(4)) } , 
        //              {1.0/6.0*Math.Pow(sampletime,3)*new Matrix(Matrix.Identity(4)),1.0/2.0 * Math.Pow(sampletime,2)*new Matrix(Matrix.Identity(4)),1.0/1.0 * Math.Pow(sampletime,1)*new Matrix(Matrix.Identity(4)) }  
        //          }
        //        );

        //    Matrix an = Matrix.MergeMatrix(
        //            new Matrix[,]{ 
        //                { new Matrix(Matrix.Identity(4)), sampletime * new Matrix(Matrix.Identity(4)),0.5 * Math.Pow(sampletime,2)*new Matrix(Matrix.Identity(4))},
        //                { new Matrix(4,4),  new Matrix(Matrix.Identity(4)), sampletime*new Matrix(Matrix.Identity(4))},
        //                { new Matrix(4,4),  new Matrix(4,4),new Matrix(Matrix.Identity(4))}
        //            }

        //        );

        //    Matrix h = Matrix.MergeMatrix(
        //           new Matrix[,]{

        //              { new Matrix(Matrix.Identity(4)),new Matrix(4,4),new Matrix(4,4)},
        //              { new Matrix(4,4),new Matrix(Matrix.Identity(4)),new Matrix(4,4)}

        //           }

        //        );

        //    Matrix pxminus = an * new Matrix(px) * Matrix.Transpose(an) + q;


        //    Matrix xhatminus = an * new Matrix(Matrix.OneD_2_TwoD(xhat));

        //    Matrix kx = pxminus * Matrix.Transpose(h) * Matrix.Inverse(h * pxminus * Matrix.Transpose(h) + new Matrix(r));
        //    px = ((new Matrix(Matrix.Identity(12)) - kx * h) * pxminus).toArray;
        //    Matrix tmp = xhatminus + kx * (new Matrix(Matrix.OneD_2_TwoD(yn)) - h * xhatminus);
        //    xhat = Matrix.TwoD_2_OneD(tmp.toArray);
        //    //Console.WriteLine(Matrix.PrintMat(tmp));
        //    //Console.WriteLine(Matrix.PrintMat(px));
          
        //}

        //bool HasErr = false;
        //public override void delDev_OnReceiveText(object sender, Comm.TextPackage txtObj)
        //{
        //    try
        //    {
        //        IDBase gpsdata = new IDBase(txtObj.Text);
        //        int MsgId = gpsdata.GetMessageID();
        //        double satelliteid;
        //        if (gpsdata.GetTotalLength() != txtObj.Text.Length)
        //        {
        //            Console.WriteLine("Message{0} Length err!", MsgId);
        //            return;
        //        }

        //        switch (MsgId)
        //        {
        //            case 7:
        //                //Console.WriteLine("7");
        //                if (ID7 != null)
        //                {

        //                    HasErr = true;
        //                    Console.WriteLine("ID 7 repeated!");

        //                }

        //                ID7 = gpsdata;

        //                //  Console.WriteLine(gpsdata.ToString());
        //                //  Console.WriteLine(ID2.ToString());

        //                //   Console.WriteLine("=============30 28 match count :{0}======Svs:{1}==========", Dict30.Count, gpsdata[3]);
        //                //  foreach (GPSMessage.IDBase data in Dict30.Values)
        //                //      Console.WriteLine("\tsatellite id:{0} GPS_time:{1}  PseudoRange:{2}  ", data[1], data[2],Dict28[data[1]][5]);
        //                try
        //                {
        //                    if (!HasErr)
        //                        PrepareGPSDataMatrix();
        //                    else
        //                        ValidDataCnt = 0;
        //                    HasErr = false;
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(SensorName + ":" + ex.Message + "," + ex.StackTrace);
        //                    this.ValidDataCnt = 0;
        //                }
        //                Dict28.Clear();
        //                Dict30.Clear();
        //                ID2 = ID4 = ID7 = ID29 = null;
        //                break;
        //            case 29:
        //                //  Console.WriteLine("29");
        //                if (ID29 != null)
        //                {
        //                    HasErr = true;
        //                    Console.WriteLine("ID 29 repeated!");
        //                }
        //                ID29 = gpsdata;
        //                //  Console.WriteLine(ID29);
        //                break;
        //            case 28:
        //                //  Console.WriteLine("28");
        //                satelliteid = gpsdata["Satellite_ID"];

        //                if (!Dict28.ContainsKey(satelliteid))
        //                {

        //                    Dict28.Add(satelliteid, gpsdata);
        //                }
        //                else
        //                {
        //                    Console.WriteLine(SensorName + ":ID28 repeat!");
        //                    HasErr = true;
        //                }
        //                break;
        //            case 30:
        //                //  Console.WriteLine("30");
        //                satelliteid = gpsdata["Satellite_ID"];

        //                if (Dict28.ContainsKey(satelliteid))
        //                {
        //                    if (!Dict30.ContainsKey(satelliteid))
        //                        Dict30.Add(satelliteid, gpsdata);
        //                    else
        //                    {
        //                        HasErr = false;
        //                        foreach (IDBase id30 in Dict30.Values)
        //                        {
        //                            if (id30["GPS_Time"] == gpsdata["GPS_Time"])
        //                            {
        //                                HasErr = true;
        //                                Console.WriteLine(SensorName + ":ID 30 repeat!");
        //                                break;
        //                            }
        //                        }

        //                        //  Console.WriteLine(SensorName + ":ID 30 repeat!");
        //                    }
        //                }

        //                break;
        //            case 2:
        //                // Console.WriteLine("2");
        //                if (ID2 != null)
        //                {
        //                    HasErr = true;
        //                    Console.WriteLine("ID 2 repeated!");
        //                }
        //                ID2 = gpsdata;

        //                break;
        //            case 4:
        //                //Console.WriteLine("4");
        //                if (ID2 != null)
        //                {
        //                    HasErr = true;
        //                    Console.WriteLine("ID 4 repeated!");
        //                }
        //                ID4 = gpsdata;
        //                break;
        //            default:
        //                break;


        //        }
        //        /*
        //        if (gpsdata.GetMessageID() == 7)
        //        {
        //            ID7 = gpsdata;
        //            Console.WriteLine(gpsdata.ToString());
        //            //System.Array.Clear
        //            Console.WriteLine("=============30 28 match count :{0}======Svs:{1}==========", Dict30.Count, gpsdata[3]);
        //            foreach (GPSMessage.IDBase data in Dict30.Values)
        //                Console.WriteLine("\tsatellite id:{0} GPS_time:{1}", data[1], data[2]);
        //            PrepareGPSDataMatrix();
        //            Dict28.Clear();
        //            Dict30.Clear();
        //        }
        //        else if (gpsdata.GetMessageID() == 28)
        //        {
        //            double satelliteid = gpsdata["Satellite_ID"];
        //            //  Console.WriteLine(gpsdata.ToString());
        //            if (!Dict28.ContainsKey(satelliteid))
        //            {

        //                Dict28.Add(satelliteid, gpsdata);
        //            }
        //        }
        //        else if (gpsdata.GetMessageID() == 30)
        //        {
        //            double satelliteid = gpsdata["Satellite_ID"];
        //            // Console.WriteLine(gpsdata.ToString());
        //            if (Dict28.ContainsKey(satelliteid))
        //            {
        //                Dict30.Add(satelliteid, gpsdata);
        //            }

        //        }
        //        else if (gpsdata.GetMessageID() == 2)
        //        {
        //            ID2 = gpsdata;
        //        }
        //        else if (gpsdata.GetMessageID() == 4)
        //            ID4 = gpsdata; */



        //        /* if (txtObj.Text[0] == 2)
        //         {
        //             Console.WriteLine(new GPSMessage.ID2(txtObj.Text).ToString());
        //         }
        //         else if (txtObj.Text[0] == 4)
        //         {
        //             Console.WriteLine(new GPSMessage.ID4(txtObj.Text).ToString());
        //         }
        //         else if (txtObj.Text[0] == 7)
        //         {
        //             Console.WriteLine(new GPSMessage.ID7(txtObj.Text).ToString());
        //         }
        //         else if (txtObj.Text[0] == 28)
        //         {
                   
        //             Console.WriteLine(new GPSMessage.ID28(txtObj.Text).ToString());
        //         }
        //         else if (txtObj.Text[0] == 30)
        //         {
        //             Console.WriteLine(new GPSMessage.ID30(txtObj.Text).ToString());
        //         }
        //         else
        //             Console.WriteLine(string.Format("cmd:{0},Length:{1}", txtObj.Text[0], txtObj.Text.Length));
        //         * */
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message + "," + ex.StackTrace);
        //    }
        //}



        //public override I_DLE CreateDLEDevice(string SensorName, System.IO.Stream stream)
        //{
        //    return new SirfDLE(SensorName, stream);
        //}

        public override void SensorDev_OnCommError(object sender)
        {
            throw new NotImplementedException();
        }
    }
}
