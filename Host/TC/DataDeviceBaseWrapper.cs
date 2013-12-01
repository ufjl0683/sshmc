using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Host.TC
{
    public delegate void OnDegreeChangedHandler(DataDeviceBaseWrapper tc, int degree);
  public  class DataDeviceBaseWrapper:DeviceBaseWrapper
    {
      public event OnDegreeChangedHandler OnDegreeChanged;
      int _CurrentDegree;
      public DataDeviceBaseWrapper(string mfccid, string devicename, string deviceType, string ip, int port, byte[] hw_status)
      : base(mfccid, devicename, deviceType, ip, port, hw_status)
      {
      }

      public void SetDegree(int degree)
      {
          this.CurrentDegree = degree;
      }

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

    }
}
