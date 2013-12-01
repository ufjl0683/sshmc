using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Host.Sensor
{
    public delegate void OnDegreeChangedHandler(SensorBase snr, int degree);
      
    public  abstract  class SensorBase
    {
        public event OnDegreeChangedHandler OnDegreeChanged;
      //  int _CurrentDegree;
        private int _SensorID;
        private string _SensorName;
        private string _SensorType;
        private double[] _Values = new double[3];
        private int _CurrentDegree;
        
        public  SensorBase(int sensor_id,string SensorName,string sensortype,string MFCC_ID,int ID, string Site_ID,int CurrentDegree)
        {
            this._SensorID = sensor_id;
            this._SensorName = SensorName;
            this._SensorType = sensortype;
            this.MFCC_ID = MFCC_ID;
            this.Site_ID = Site_ID;
            this._CurrentDegree = CurrentDegree;
        }

        public int SensorID { get { return _SensorID;   } }
        public string SensorName { get { return _SensorName; } }
        public string SensotType { get { return _SensorType; } }
        public abstract int GetValueLength();
        public double Value0
        {
            get
            {
                return _Values[0];
            }
            set
            {
                _Values[0] = value;
            }
        }
        public double Value1
        {
            get
            {
                return _Values[1];
            }
            set
            {
                _Values[1] = value;
            }
        }
        public double Value2
        {
            get
            {
                return _Values[2];
            }
            set
            {
                _Values[2] = value;
            }
        }

        public bool IsConnected { get; set; }
        public int CurrentDegree
        {
            get
            {
                return _CurrentDegree;
            }
            set
            {
                if (value != _CurrentDegree)
                {
                    _CurrentDegree = value;
                    if (this.OnDegreeChanged != null)
                        this.OnDegreeChanged(this, value);

                }
            }
        }

        public string MFCC_ID { get; set; }
        public int ID { get; set; }
        public string Site_ID { get; set; }
        public bool IsValid { get; set; }
    }
}
