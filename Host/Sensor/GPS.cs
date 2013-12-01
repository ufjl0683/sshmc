using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Host.Sensor
{
    public class GPS:SensorBase
    {

        public GPS(int sensorid, string sensorname, string sensortype,string mfcc_id,int id,string siteid ,int current_degree)
            : base(sensorid, sensorname, sensortype, mfcc_id, id, siteid, current_degree)
        {
        }
        public override int GetValueLength()
        {

            return 3;
           // throw new NotImplementedException();
        }
    }
}
