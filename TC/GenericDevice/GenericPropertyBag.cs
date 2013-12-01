using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comm;

namespace GenericDevice
{
 public    class GenericPropertyBag:PropertyBag
    {
        protected override Type GetPropertyType()
        {
            return typeof(GenericPropertyBag);
        }
    }
}
