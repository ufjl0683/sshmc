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
    public class GoogleMap : ESRI.ArcGIS.Client.TiledMapServiceLayer
    {
        public override void Initialize()
        {

            this.FullExtent = new
             Envelope(-20037508.342787,-20037508.342787,20037508.342787,20037508.342787);
            //(-180,-85.0511287798066,180, 85.0511287798066)
            {
                SpatialReference = new  ESRI.ArcGIS.Client.Geometry.SpatialReference(102113);
            };
            this.SpatialReference = new ESRI.ArcGIS.Client.Geometry.SpatialReference(102113);
            //this.InitialExtent = this.FullExtent;
            this.TileInfo = new TileInfo()
            {
                Height = 256,
                Width = 256,

            Origin = new ESRI.ArcGIS.Client.Geometry.MapPoint(-20037508.342787,
            20037508.342787)//Origin = new ESRI.ArcGIS.Geometry.MapPoint(-180, 90)
                {
                    SpatialReference = new ESRI.ArcGIS.Client.Geometry.SpatialReference(102113)
                },
                Lods = new Lod[20]
            };

            double resolution = 156543.033928;
            for (int i = 0; i < TileInfo.Lods.Length; i++)
            {
                TileInfo.Lods[i] = new Lod() { Resolution = resolution };
                resolution /= 2;
            }

            base.Initialize();
        }

        public override string GetTileUrl(int level, int row, int col)
        {
            //google maps map
           // string baseUrl = "http://mt0.google.com/mt/v=ap.92&hl=zh-CN&x=";
            string baseUrl = "http://mt1.google.com/vt/lyrs=m@129&hl=zh-TW&x=";
            string url = baseUrl + col.ToString() + "&y=" + row.ToString() + "&z=" + level.ToString() + "&s=";
            return url;

            ////google maps satallite
           // string baseUrl = "http://khm2.google.com/kh/v=38&hl=zh-TW&x=";
           // string url = baseUrl + col.ToString() + "&y=" + row.ToString() + "&z=" + level.ToString() + "&s=BQIAAAAEF0sXuI47eD6TY7N0nujgRSc1DKDncXZcOmmhWSL8K5Vh4tJFxQfD4xvDrvmuW7OT5aiGNCOfN8mDw";
           // return url;
            //// http://mt1.google.com/vt/lyrs=m@129&hl=en&x=1&y=0&z=1&s=Galileo
        }

        public static void LongitudeLatitude2GoogleTileXT(double longitude, double latitude, int level, out int tilex, out int tiley)
        {
            double sinLatitude = Math.Sin(latitude * Math.PI / 180);

            double pixelX = ((longitude + 180) / 360) * 256 * Math.Pow(2, level);

            double pixelY = (0.5 - Math.Log((1 + sinLatitude) / (1 - sinLatitude)) / (4 * Math.PI)) * 256 * Math.Pow(2, level);

            tilex = (int)(pixelX / 256);
            tiley = (int)(pixelY / 256);

            double relx = (int)pixelX % 256;
            double rely = (int)pixelY % 256; ;


        }


    }
}
