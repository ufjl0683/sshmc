using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace GPSDevice
{
   public  class GPSPropertyBag:Comm.PropertyBag
    {
     
       public GPSPropertyBag() 
       {
          
       }
       //public override void Serialize()
       //{
         
       ////    if (IsLoad)
       ////        return;
       ////    System.Xml.Serialization.XmlSerializer ser = new XmlSerializer(typeof(GPSPropertyBag));
       ////    System.IO.FileStream fs=System.IO.File.Create("PropertyBag.xml");
       ////    ser.Serialize(fs, this);
       ////    fs.Close();
       ////    fs.Dispose();
       //    //base.Serialize();
       //}

       protected override Type GetPropertyType()
       {
           return typeof(GPSPropertyBag);
       }
    }
}
