using System;
using System.Collections.Generic;
using System.Text;


namespace GPSDevice.GPSMessage
{
   public  class GPSData:IComparable<GPSData>
    {
      // public UBIDBase[] eph_datas;
      public bool IsReference = false;
      public bool IsTrackFinished = false;
      public int TrackCnt = 0;
      public double TimeStamp;

      public double[,] svxyz; //各個衛星所在的座標
      public double[] initpos;
      public int[] satelliteIds; //各衛星編號
      private double[] _CPrr;  // 接收機 carrier phase pseudo range 
      private double[] _Prr; //GPS 接收機 計算之 Psudo range
      private double[] _Prsm;
      double[] refPrErr;
      double[] refCPrErr;
      double[] refPsmErr;
      public double[] refCprr;
      public double tol;
      public double rec_clock_bias;
      public double rec_clock_drif;
      public  int ValidSateliteCnt = 0;
      public double id2x, id2y, id2z;//GPS 接收機計算之座標
      public double refx, refy, refz;
      public double basex, basey, basez;
      public double[] sv_clock_bais;
      public double[] sv_iono_delay;
      public double[] sv_elv;
      public double[] pr_correct;

      public int MaxElvIndex
      {
          get
          {
              int maxind = 0;
              double maxelv=sv_elv[0];
              for(int i=1;i<sv_elv.Length;i++)
                  if (sv_elv[i] > maxelv)
                  {
                      maxind = i;
                      maxelv = sv_elv[i];
                  }
              return maxind;
          }
      }

      public double[] CPrr  //修正過載波相位
      {

          set
          {
              _CPrr = value;
          }

          get
          {
             // return _CPrr;
              double[] myprr = new double[satelliteIds.Length];

              for (int i = 0; i < myprr.Length; i++)
              {
                  //- GpsDataMatrix[31, 5] * 2.99792458e-1 / 1575420000
              //    myprr[i] = _CPrr[i] -rec_clock_bias * 2.99792458e-1  /*/ 1575420000*/  + sv_clock_bais[i] * 2.99792458e8 /*/ 1575420000*/;

                  myprr[i] = _CPrr[i] * 2.99792458e8 / 1575420000 + (sv_clock_bais[i] - rec_clock_bias*1e-9) * 2.99792458e8 / 1575420000;
              }

              return myprr;

          }
      }

      public double[] Prsm
      {
          set
          {
              this._Prsm = value; 
          }
          get
          {
               double[] prsm = new double[satelliteIds.Length];
              if (IsReference)
              {
                  //for (int i = 0; i < pr.Length; i++)
                  //    pr[i] = Norm(new double[] { refx - svxyz[i, 0], refy - svxyz[i, 1], refz - svxyz[i, 2] });
                return _Prsm; 
              }
              else
              {
                  for (int i = 0; i < prsm.Length; i++)
                  {
                     // Norm(new double[] { id2x - svxyz[i, 0], id2y - svxyz[i, 1], id2z - svxyz[i, 2] });
                      if (refPsmErr != null)
                         prsm[i] = _Prsm[i] + refPsmErr[i];
                      else
                          prsm[i] = _Prsm[i];

                  
                  }

              }
              return prsm;
          }
          
      }


      public double[] Prr
      {

          set
          {
              this._Prr = value;
          }
          get
          {


              double[] myprr = new double[satelliteIds.Length];

              for (int i = 0; i < myprr.Length; i++)
              {

                 // myprr[i] = _Prr[i] + sv_clock_bais[i] * 2.99792458e8 - sv_iono_delay[i]
                 //- (2.312 / Math.Sin(Math.Sqrt(sv_elv[i] * sv_elv[i] + 1.904E-3)) + 0.084 / Math.Sin(Math.Sqrt(sv_elv[i] * sv_elv[i] + 0.6854E-3)))
                 // - rec_clock_bias * 2.99792458e8 * 1e-9;

                  myprr[i] = _Prr[i] + sv_clock_bais[i] * 2.99792458e8 - sv_iono_delay[i] / 100/* + pr_correct[i] / 100 */
                  -(2.312 / Math.Sin(Math.Sqrt(sv_elv[i] * sv_elv[i] + 1.904E-3)) + 0.084 / Math.Sin(Math.Sqrt(sv_elv[i] * sv_elv[i] + 0.6854E-3)))
                    -rec_clock_bias * 2.99792458e8 * 1e-9;
              }

              return myprr;
          //    return _Prr;
          }
          

      }

      public PrErr[] CPrOffset
      {
          get
          {
              PrErr[] err = new PrErr[satelliteIds.Length];
              double[] Pr = this.Pr;

              for (int i = 0; i < err.Length; i++)
              {
                  err[i] = new PrErr()
                  {
                      SatelliteID = satelliteIds[i],
                      Error = CPr[i] - CPrr[i],
                      timestamp = this.TimeStamp
                  };
              }

              return err;
          }

      }

       public PrErr[] PrsmOffset
       {
           get
           {
               PrErr[] err = new PrErr[satelliteIds.Length];
               for (int i = 0; i < err.Length; i++)
               {
                   err[i] = new PrErr()
                   {
                       SatelliteID = satelliteIds[i],
                       Error = Pr[i]- Prsm[i] ,
                       timestamp = this.TimeStamp
                   };
               }

               return err;
           }


       }
      public PrErr[] PrOffset
      {
          get
          {
              PrErr[] err = new PrErr[satelliteIds.Length];
              for (int i = 0; i < err.Length; i++)
              {
                  err[i] = new PrErr()
                  {
                       SatelliteID=satelliteIds[i],
                       Error=Pr[i]-Prr[i],
                       timestamp=this.TimeStamp
                  };
              }

              return err;
          }
      }

      //public double[] PrModify
      //{
      //    get
      //    {
      //        double[] myprr = new double[satelliteIds.Length];

      //        for (int i = 0; i < myprr.Length; i++)
      //        {
      //           // myprr[i] = Prr[i] - rec_clock_bias * 2.99792458e8*1e-9;
      //            myprr[i] = Prr[i] + sv_clock_bais[i] * 2.99792458e8 - sv_iono_delay[i]-
      //                2.312 / Math.Sin(Math.Sqrt(sv_elv[i] * sv_elv[i] + 1.904E-3)) + 0.084 / Math.Sin(Math.Sqrt(sv_elv[i] * sv_elv[i] + 0.6854E-3));

      //            //TropoDelay = 2.312 / sin(sqrt(El * El + 1.904E-3)) + 0.084 / sin(sqrt(El * El + 0.6854E-3))

      //        }

      //        return myprr;
      //    }
      //}

      public void SetPrsmOffset(GPSData refdata,PrErr[] prerr,PrErr[] cprerr)
      {
          PrErr[] prsmerr=refdata.PrsmOffset;
          System.Collections.Generic.Dictionary<int, int> SatelliteInxAry = new System.Collections.Generic.Dictionary<int, int>();
         

          for (int i = 0; i < Prr.Length; i++)
          {
              // bool match=false;
              for (int j = 0; j < prsmerr.Length; j++)
              {
                  if (satelliteIds[i] == prsmerr[j].SatelliteID)
                  {
                      SatelliteInxAry.Add(i, j);
                  }
              }
          }

          if (prsmerr.Length != SatelliteInxAry.Count)
          {
              Console.WriteLine("check point!");

          }

          int[] _satelliteIds = new int[SatelliteInxAry.Count];
          double[] _Prr = new double[SatelliteInxAry.Count];
          double[] _CPrr = new double[SatelliteInxAry.Count];
          double[] _refPsmErr = new double[SatelliteInxAry.Count];
          double[] _refCprr = new double[SatelliteInxAry.Count];
          double[] _refPrErr = new double[SatelliteInxAry.Count];
          double[] _refCPrErr = new double[SatelliteInxAry.Count];
         
          double[,] _svxyz = new double[SatelliteInxAry.Count, 3];
          double[] _sv_clock_bais = new double[SatelliteInxAry.Count];
          double[] _sv_iono_delay = new double[SatelliteInxAry.Count];
          double[] _sv_elv = new double[SatelliteInxAry.Count];
          int inx = 0;
          for (int i = 0; i < satelliteIds.Length; i++)
          {
              if (SatelliteInxAry.ContainsKey(i))
              {
                  _satelliteIds[inx] = satelliteIds[i];
                  _Prr[inx] = this._Prr[i];
                  _CPrr[inx] = this._CPrr[i];
                  _refPsmErr[inx] = prsmerr[SatelliteInxAry[i]].Error;
         
                  //2012.7.16 改用  refgps xyz
                  //_svxyz[inx, 0] = refdata.svxyz[SatelliteInxAry[i], 0];
                  //_svxyz[inx, 1] = refdata.svxyz[SatelliteInxAry[i], 1];
                  //_svxyz[inx, 2] = refdata.svxyz[SatelliteInxAry[i], 2];
                  _svxyz[inx, 0] = svxyz[i, 0];
                  _svxyz[inx, 1] = svxyz[i, 1];
                  _svxyz[inx, 2] = svxyz[i, 2];

                  ////2012.7.16 改用  refgps sv_clock_bais
                  //_sv_clock_bais[inx] = refdata.sv_clock_bais[SatelliteInxAry[i]];
                  _sv_clock_bais[inx] = sv_clock_bais[i];
                  _sv_iono_delay[inx] = sv_iono_delay[i];
                  _sv_elv[inx] = sv_elv[i];
                  _refCprr[inx]=refdata.CPrr[SatelliteInxAry[i]];
                  _refPrErr[inx] = prerr[SatelliteInxAry[i]].Error;
                  _refCPrErr[inx] = cprerr[SatelliteInxAry[i]].Error;
                  inx++;
              }
          }

          if (inx == 0)
              return;

          satelliteIds = _satelliteIds;
          this._Prr = _Prr;
          this._CPrr = _CPrr;
       //   refPrErr = _refPrErr;
        //  refCPrErr = _refCPrErr;
          refPsmErr = _refPsmErr;
          svxyz = _svxyz;
          sv_elv = _sv_elv;
          sv_iono_delay = _sv_iono_delay;
          sv_clock_bais = _sv_clock_bais;

          //basex = refx;
          //basey = refy;

          basex = refdata.refx;
          basey = refdata.refy;
          basez = refdata.refz;
          this.refCprr = _refCprr;
          refPrErr = _refPrErr;
          refCPrErr = _refCPrErr;

      }


      public void SetPrrOffset(PrErr[] prerr,PrErr[] cprerr)
      {
          System.Collections.Generic.Dictionary<int, int> SatelliteInxAry = new System.Collections.Generic.Dictionary <int, int>();
          if (prerr.Length != satelliteIds.Length)
          {
              Console.WriteLine("check point!");
              
          }
         
          for (int i = 0; i < Prr.Length; i++)
          {
             // bool match=false;
              for (int j = 0; j < prerr.Length; j++)
              {
                  if (satelliteIds[i] == prerr[j].SatelliteID)
                  {
                      SatelliteInxAry.Add(i,j);
                  }
              }
          }

          int[] _satelliteIds = new int[SatelliteInxAry.Count];
          double[] _Prr = new double[SatelliteInxAry.Count];
          double[] _CPrr = new double[SatelliteInxAry.Count];
           double[] _refPrErr = new double[SatelliteInxAry.Count];
           double[] _refCPrErr = new double[SatelliteInxAry.Count];
           double[,] _svxyz = new double[SatelliteInxAry.Count, 3];
           double[] _sv_clock_bais=new double[SatelliteInxAry.Count];
           double[] _sv_iono_delay=new double[SatelliteInxAry.Count];
           double[] _sv_elv=new double[SatelliteInxAry.Count];
         int inx=0;
         for (int i = 0; i < satelliteIds.Length; i++)
         {
             if (SatelliteInxAry.ContainsKey(i))
             {
                 _satelliteIds[inx] = satelliteIds[i];
                 _Prr[inx] = this._Prr[i];
                 _CPrr[inx] = this._CPrr[i];
                 _refPrErr[inx] = prerr[SatelliteInxAry[i]].Error;
                 _refCPrErr[inx] = cprerr[SatelliteInxAry[i]].Error;
                 _svxyz[inx, 0] = svxyz[i, 0];
                 _svxyz[inx, 1] = svxyz[i, 1];
                 _svxyz[inx, 2] = svxyz[i, 2];
                 _sv_clock_bais[inx] = sv_clock_bais[i];
                 _sv_iono_delay[inx] = sv_iono_delay[i];
                 _sv_elv[inx] = sv_elv[i];
                 inx++;
             }
         }

         if (inx == 0)
             return;

         satelliteIds = _satelliteIds;
         this._Prr = _Prr;
         this._CPrr = _CPrr;
         refPrErr = _refPrErr;
         refCPrErr = _refCPrErr;
         svxyz = _svxyz;
         sv_elv = _sv_elv;
         sv_iono_delay = _sv_iono_delay;
         sv_clock_bais = _sv_clock_bais;

      }


      public double[] CPr
      {
          get
          {
              double[] cpr = new double[satelliteIds.Length];
              if (IsReference)
              {
                  for (int i = 0; i < cpr.Length; i++)
                      cpr[i] = Norm(new double[] { refx - svxyz[i, 0], refy - svxyz[i, 1], refz - svxyz[i, 2] });

              }
              else
              {
                  for (int i = 0; i < cpr.Length; i++)
                  {
                      // Norm(new double[] { id2x - svxyz[i, 0], id2y - svxyz[i, 1], id2z - svxyz[i, 2] });
                      cpr[i] = CPrr[i] + refCPrErr[i];

                  }

              }
              return cpr;
          }
      }

      public double[] PrrCpDifference
      {
          get
          {
              double[] err = new double[satelliteIds.Length];
              for (int i = 0; i < err.Length; i++)
                  err[i] = this.Prr[i] - this.CPrr[i];

              return err;

          }
      }
      public double[] Pr //實際 Pseudo
      {

          get
          {
              double[] pr = new double[satelliteIds.Length];
              if (IsReference)
              {
                  for (int i = 0; i < pr.Length; i++)
                      pr[i] = Norm(new double[] { refx - svxyz[i, 0], refy - svxyz[i, 1], refz - svxyz[i, 2] });
                 
              }
              else
              {
                  for (int i = 0; i < pr.Length; i++)
                  {
                     // Norm(new double[] { id2x - svxyz[i, 0], id2y - svxyz[i, 1], id2z - svxyz[i, 2] });
                      if (refPrErr != null)
                          pr[i] = Prr[i] + refPrErr[i];
                      else
                          pr[i] = Prr[i];

                  
                  }

              }
              return pr;
          }
      }

      
      double Norm(double[] vect)
      {


          double ret = 0;
          for (int i = 0; i < vect.Length; i++)
          {
              ret += Math.Pow(vect[i], 2);
          }
          return Math.Sqrt(ret);

      }

      public override string ToString()
      {
          return this.TimeStamp.ToString();
      }


      public int CompareTo(GPSData other)
      {
        //  throw new NotImplementedException();

          return (int)this.TimeStamp - (int)other.TimeStamp;
      }
    }

   public struct PrErr
   {
       public int SatelliteID;
       public double Error;
       public double timestamp;
       public override string  ToString()
        {
            return string.Format("ID:{0} err:{1}", SatelliteID, Error);
        }
   }
}
