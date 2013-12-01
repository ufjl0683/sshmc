using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace TiltDevice
{
    public class TiltPropertyBag : Comm.PropertyBag
    {
        protected override Type GetPropertyType()
        {
            return typeof(TiltPropertyBag);
        }
    }
}
