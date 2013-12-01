using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Host.Sensor
{
   public  class SensorManager
    {
       System.Collections.Generic.Dictionary<int, SensorBase> dictSensors = new Dictionary<int, SensorBase>();


       public SensorBase this[int snrid]
       {
           get
           {
               if (!this.dictSensors.ContainsKey(snrid))
                   throw new Exception("sensorid:"+snrid+" not found!");

               return dictSensors[snrid];

           }
       }

       public System.Collections.IEnumerable getAllDeviceEnum()
       {

           System.Collections.IEnumerator ie = this.dictSensors.GetEnumerator();
           while (ie.MoveNext())
           {
               // if (!(((System.Collections.DictionaryEntry)ie.Current).Value is TC.OutPutDeviceBase))
               yield return ((System.Collections.DictionaryEntry)ie.Current).Value;
           }

       }

       public void SetSensorCurrentDegreeValue(int snrid, double value0, double value1, double value2, int degree)
       {
           lock (this.dictSensors)
           {
               if (!dictSensors.ContainsKey(snrid))
                   throw new Exception(snrid + "not found!");
               
               dictSensors[snrid].CurrentDegree = degree;
               dictSensors[snrid].Value0=value0;
               dictSensors[snrid].Value1 = value1;
               dictSensors[snrid].Value2 = value2;
               

           }
       }
       public SensorManager()
       {
           try
           {
               LoadSensors();
               Console.WriteLine("SensorManager Loading Completed!");
           }
           catch (Exception ex)
           {
               Console.WriteLine(ex.Message + "," + ex.StackTrace);
               System.Environment.Exit(-1);
           }

       }

       void LoadSensors()
       {
           SSHMC01Entities1 db = new SSHMC01Entities1();
           foreach (vwSensorDegree sensor in db.vwSensorDegree)
           {
               SensorBase snr;
               if (!dictSensors.ContainsKey(sensor.SENSOR_ID))
               {
                   try
                   {

                       if (sensor.SENSOR_TYPE == "TILT")
                       {
                           snr = new Tilt(sensor.SENSOR_ID, sensor.SENSOR_NAME, sensor.SENSOR_TYPE, sensor.MFCC_ID, (int)sensor.ID, sensor.SITE_ID,(int)sensor.CURRENT_DEGREE);
                           // dictSensors.Add(sensor.SENSOR_ID, new Tilt(sensor.SENSOR_ID, sensor.SENSOR_NAME, sensor.SENSOR_TYPE,sensor.MFCC_ID,sensor.ID,sensor.SITE_ID));
                       }
                       else if (sensor.SENSOR_TYPE == "GPS")
                           snr = new GPS(sensor.SENSOR_ID, sensor.SENSOR_NAME, sensor.SENSOR_TYPE, sensor.MFCC_ID, (int)sensor.ID, sensor.SITE_ID,(int)sensor.CURRENT_DEGREE);
                       //  dictSensors.Add(sensor.SENSOR_ID, new GPS(sensor.SENSOR_ID, sensor.SENSOR_NAME, sensor.SENSOR_TYPE,sensor.MFCC_ID,sensor.ID,sensor.SITE_ID));
                       else
                       {
                           Console.WriteLine("unnknow sensor type " + sensor.SENSOR_TYPE);
                           continue;
                       }

                       snr.Value0 = (sensor.VALUE0==null )?0 : (double)sensor.VALUE0;
                       snr.Value1 = (sensor.VALUE1 == null) ? 0 : (double)sensor.VALUE1;
                       snr.Value2 = (sensor.VALUE2 == null) ? 0 : (double)sensor.VALUE2;
                       snr.IsConnected = (sensor.ISCONNECTED.Trim() == "Y") ? true : false;
                       snr.IsValid = (sensor.ISVALID == "Y") ? true : false;
                       dictSensors.Add(snr.SensorID, snr);
                   }
                   catch (Exception ex) { Console.WriteLine(ex.Message + "" + ex.StackTrace); }
               }

             


           }
           
       }

    }
}
