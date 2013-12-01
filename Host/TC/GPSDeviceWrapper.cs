using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Host.TC
{
    public  class GPSDeviceWrapper:DataDeviceBaseWrapper
    {
        public GPSDeviceWrapper(string mfccid, string devicename, string deviceType, string ip, int port, byte[] hw_status)
            : base(mfccid, devicename, deviceType, ip, port, hw_status)
        {
        }
    }
}
