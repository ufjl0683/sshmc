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
    public class TYFDMap : ESRI.ArcGIS.Client.TiledMapServiceLayer
    {
        public override void Initialize()
        {

            this.FullExtent = new
             Envelope(53345.036, 2411683.347, 490618.274, 2813866.758);
         //  Envelope(53345.036, 2411683.347+40, 490618.274, 2813905.52+40);
            {
                SpatialReference = new ESRI.ArcGIS.Client.Geometry.SpatialReference(104137);
            };
            this.SpatialReference = new ESRI.ArcGIS.Client.Geometry.SpatialReference(104137);
            //this.InitialExtent = this.FullExtent;
            this.TileInfo = new TileInfo()
            {
                Height = 256,
                Width = 256,

                Origin = new ESRI.ArcGIS.Client.Geometry.MapPoint(53345.036,
           2813866.758)//Origin = new ESRI.ArcGIS.Geometry.MapPoint(-180, 90)
                {
                    SpatialReference = new ESRI.ArcGIS.Client.Geometry.SpatialReference(104137)
                },
                Lods = new Lod[10]
            };

      //<zoom_level level="0" name="level0" description="" scale="358672.0" tile_width="24294.05013333333" tile_height="24294.05013333333"/>
      //<zoom_level level="1" name="level1" description="" scale="141873.0" tile_width="9609.5312" tile_height="9609.5312"/>
      //<zoom_level level="2" name="level2" description="" scale="63949.0" tile_width="4331.478933333333" tile_height="4331.478933333333"/>
      //<zoom_level level="3" name="level3" description="" scale="35993.0" tile_width="2437.9258666666665" tile_height="2437.9258666666665"/>
      //<zoom_level level="4" name="level4" description="" scale="20258.0" tile_width="1372.1418666666666" tile_height="1372.1418666666666"/>
      //<zoom_level level="5" name="level5" description="" scale="11402.0" tile_width="772.2954666666666" tile_height="772.2954666666666"/>
      //<zoom_level level="6" name="level6" description="" scale="6417.0" tile_width="434.6448" tile_height="434.6448"/>
      //<zoom_level level="7" name="level7" description="" scale="3611.0" tile_width="244.58506666666665" tile_height="244.58506666666665"/>
      //<zoom_level level="8" name="level8" description="" scale="1144.0" tile_width="77.48693333333333" tile_height="77.48693333333333"/>
      //<zoom_level level="9" name="level9" description="" scale="644.0" tile_width="43.620266666666666" tile_height="43.620266666666666"/>
            double resolution =94.8984375;
            TileInfo.Lods[0] =new Lod(){  Resolution= 24294.05013333333 / 256};
            TileInfo.Lods[1] = new Lod() { Resolution = 9609.5312 / 256 };
            TileInfo.Lods[2] = new Lod() { Resolution = 4331.478933333333 / 256 };
            TileInfo.Lods[3] = new Lod() { Resolution = 2437.9258666666665 / 256 };
            TileInfo.Lods[4] = new Lod() { Resolution = 1372.1418666666666 / 256 };
            TileInfo.Lods[5] = new Lod() { Resolution = 772.2954666666666 / 256 };
            TileInfo.Lods[6] = new Lod() { Resolution = 434.6448 / 256 };
            TileInfo.Lods[7] = new Lod() { Resolution = 244.58506666666665 / 256 };
            TileInfo.Lods[8] = new Lod() { Resolution = 77.48693333333333 / 256 };
            TileInfo.Lods[9] = new Lod() { Resolution = 43.620266666666666 / 256 };
           
            //for (int i = 0; i < TileInfo.Lods.Length; i++)
            //{
            //    TileInfo.Lods[i] = new Lod() { Resolution = resolution };
            //    resolution /= 2;
            //}

            base.Initialize();
            
          
        }

        public override string GetTileUrl(int level, int row, int col)
        {
            //google maps map
            //TYFD  http://10.11.3.104/mapviewer/mcserver?request=gettitle&format=PNG&zoomlevel=9&mapcache=MAP103.MAP103&mx=4881&my=8022
             double fullheight= this.FullExtent.YMax - this.FullExtent.YMin;
             int ymaxindex =(int)( fullheight /( TileInfo.Lods[level].Resolution*TileInfo.Height));
             
#if DEBUG
            string baseurl = "http://localhost:8080/MapService.svc/GetImage?level=" + level + "&col=" + col + "&row=" + row ;
#else
             string baseurl = "http://210.241.86.158/TYFBMap/MapService.svc/GetImage?level=" + level + "&col=" + col + "&row=" + row;

#endif
            return baseurl;
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
