using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteInterface.SensorConfig;

namespace GenericDevice
{
    class Program
    {
        static void Main(string[] args)
        {

            new GenericController(ControllerConfigBase.Deserialize(AppDomain.CurrentDomain.BaseDirectory + "config.xml"), GetProperty());
        }

        static GenericPropertyBag GetProperty()
        {
            GenericPropertyBag property;
            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "PropertyBag.xml"))
            {
                System.IO.FileStream fs;
                System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(GenericPropertyBag));
                property = ser.Deserialize(fs = System.IO.File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "PropertyBag.xml")) as GenericPropertyBag;

                fs.Close();
                fs.Dispose();
            }
            else
            {
                property = new GenericPropertyBag();

            }

            property.SetHasLoaded();

            return property;


        }
    }
}
