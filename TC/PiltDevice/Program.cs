using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using RemoteInterface.SensorConfig;

namespace TiltDevice
{
    class Program
    {
        static void Main(string[] args)
        {
            new TiltController(ControllerConfigBase.Deserialize(AppDomain.CurrentDomain.BaseDirectory + "config.xml"), GetProperty());
        }

        static TiltPropertyBag GetProperty()
        {
            TiltPropertyBag property;
            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "PropertyBag.xml"))
            {
                System.IO.FileStream fs;
                System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(TiltPropertyBag));
                property = ser.Deserialize(fs = System.IO.File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "PropertyBag.xml")) as TiltPropertyBag;

                fs.Close();
                fs.Dispose();
            }
            else
            {
                property = new TiltPropertyBag();

            }

            property.SetHasLoaded();

            return property;


        }
    }
}
