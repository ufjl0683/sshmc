 
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
namespace ArcGISLib
{
    public class GaoDeMap: ESRI.ArcGIS.Client.TiledMapServiceLayer
    {


        private const double cornerCoordinate = 20037508.342787;
        public override void Initialize()
        {

          this.FullExtent = new
           ESRI.ArcGIS.Client.Geometry.Envelope(-20037508.342787, -20037508.342787, 20037508.342787, 20037508.342787);
            {
                SpatialReference = new ESRI.ArcGIS.Client.Geometry.SpatialReference(102100);
            };


            this.SpatialReference = new ESRI.ArcGIS.Client.Geometry.SpatialReference(102100);

            this.TileInfo = new TileInfo()
            {
                Height = 256,
                Width = 256,

                Origin = new ESRI.ArcGIS.Client.Geometry.MapPoint(-20037508.342787, 20037508.342787)
                {
                    SpatialReference = new ESRI.ArcGIS.Client.Geometry.SpatialReference(102100)
                },
                Lods = new Lod[20]
            };


            double resolution = 156543.033928;

            for (int i = 0; i < TileInfo.Lods.Length; i++)
            {

                TileInfo.Lods[i] = new Lod() { Resolution = resolution };
                resolution /= 2;
            }

            // Call base initialize to raise the initialization event 
            base.Initialize();
        }

        public override string GetTileUrl(int level, int row, int col)
        {
         //   string baseUrl = "http://webrd0{0}.is.autonavi.com/appmaptile?x={1}&y={2}&z={3}&lang=zh_cn&size=1&scale=1&style=7"; ;
            string baseUrl = "http://webrd02.is.autonavi.com/appmaptile?x={1}&y={2}&z={3}&lang=zh_cn&size=1&scale=1&style=7"; ;

            string quard = GetQuard(col, row, level);

            return string.Format(baseUrl, (object)quard[quard.Length - 1], col, row, level);
        }

        public static string GetQuard(int x, int y, int zoomLevel)
        {
            string str = "";
            while (x > 0 || y > 0)
            {
                str = ((x & 1) << 1 | y & 1).ToString() + str;
                x >>= 1;
                y >>= 1;
            }
            return ((object)str).ToString().PadLeft(zoomLevel, '0');
        }

    }
}
