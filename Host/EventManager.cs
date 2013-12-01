using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Host ;

namespace Host
{
     public  class EventManager
    {

         public EventManager(Sensor.SensorManager snrmgr)
         {
            // SSHMC01Entities1 db = new SSHMC01Entities1();

             foreach (Sensor.SensorBase snr in snrmgr.getAllDeviceEnum())
             {

                 snr.OnDegreeChanged += new Sensor.OnDegreeChangedHandler(snr_OnDegreeChanged);
             }


         }

         void snr_OnDegreeChanged(Sensor.SensorBase snr, int degree)
         {
             //throw new NotImplementedException();
         }

        

        
    }
}
