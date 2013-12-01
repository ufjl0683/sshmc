using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ESRI.ArcGIS.Client.Geometry;

namespace ArgisGoogle
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            this.map1.MouseMove += new MouseEventHandler(map1_MouseMove);

         
          //MapPoint minp=  ESRI.ArcGIS.Client.Bing.Transform.WebMercatorToGeographic(
          //    new MapPoint(-20037508.342787,-20037508.342787));
          //MapPoint maxp = ESRI.ArcGIS.Client.Bing.Transform.WebMercatorToGeographic(
          // new MapPoint(20037508.342787, 20037508.342787));


            Graphic_Add();
        }


        void Graphic_Add()
        {
            ESRI.ArcGIS.Client.Tasks.QueryTask task = new ESRI.ArcGIS.Client.Tasks.QueryTask(
                "http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/Demographics/ESRI_Census_USA/MapServer/5");
            ESRI.ArcGIS.Client.Tasks.Query qry = new ESRI.ArcGIS.Client.Tasks.Query()
            {
                OutSpatialReference = map1.SpatialReference,
//                XMin: -125.192864569718
//YMin: 19.4163766870772
//XMax: -66.1058240510347
//YMax: 54.3182805228683

                Geometry = new Envelope()
                {
                     XMin=-90,YMin=27 ,XMax=-74,YMax=51
                }

            };
            qry.OutFields.Add("*");
            qry.ReturnGeometry = true;
            task.ExecuteCompleted += (s, a) =>
            {
                if (a.FeatureSet == null || a.FeatureSet.Count() == 0)
                    return;
               
                ESRI.ArcGIS.Client.GraphicsLayer gl = map1.Layers["mygraphic"] as ESRI.ArcGIS.Client.GraphicsLayer;
                foreach (ESRI.ArcGIS.Client.Graphic g in a.FeatureSet.Features)
                {
                    g.Symbol = new ESRI.ArcGIS.Client.Symbols.SimpleFillSymbol() { BorderThickness = 1, Fill = new SolidColorBrush(Colors.Red) };
                        //this.Resources["sample"] as ESRI.ArcGIS.Client.Symbols.Symbol;

                    gl.Graphics.Add(g);
                }
            };
            task.Failed += (s, a) =>
                {
                    MessageBox.Show(a.Error.Message);
                };

            task.ExecuteAsync(qry);


        }

        void map1_MouseMove(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();

            //System.Windows.Point p = e.GetPosition(this.map1);
            //ESRI.ArcGIS.Client.Geometry.MapPoint mp = map1.ScreenToMap(p);
         
            //try
            //{
            //    mp = ESRI.ArcGIS.Client.Bing.Transform.WebMercatorToGeographic(mp);
            //}
            //catch { ;}
            //if(mp!=null)
            //textBlock1.Text = mp.ToString();
           
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
         //   System.Random r=new Random();
            //for (int i = 0; i < 3000; i++)
            //{


         //   ESRI.ArcGIS.Client.Geometry.MapPoint mp = ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(

          //   new ESRI.ArcGIS.Client.Geometry.MapPoint(121.55437847681263, 24.99750117341647)); 

            //   new ESRI.ArcGIS.Client.Geometry.MapPoint(121.299405, 25.000298));
               // MapPoint mp = new MapPoint(r.Next(-20037508, 20037508), r.Next(-20037508, 20037508));
            ESRI.ArcGIS.Client.Geometry.MapPoint mp = new ESRI.ArcGIS.Client.Geometry.MapPoint(280221.955, 2765843.943, new SpatialReference(104137));

                ESRI.ArcGIS.Client.Geometry.MapPoint p1, p2;
                p1 = new ESRI.ArcGIS.Client.Geometry.MapPoint(mp.X - 150, mp.Y - 150, new SpatialReference(104137));
                p2 = new ESRI.ArcGIS.Client.Geometry.MapPoint(mp.X + 150, mp.Y + 150, new SpatialReference(104137));
              //  this.map1.ZoomTo(new ESRI.ArcGIS.Client.Geometry.Envelope(p1, p2) );

                if ((this.map1.Layers["elementlyr"] as ESRI.ArcGIS.Client.ElementLayer).Children.Count > 0)
                    return;
                Alarm alarm = new Alarm() { Width = 40, Height = 40 };
                alarm.SetValue(ESRI.ArcGIS.Client.ElementLayer.EnvelopeProperty,
                   new ESRI.ArcGIS.Client.Geometry.Envelope(mp, mp));
                (this.map1.Layers["elementlyr"] as ESRI.ArcGIS.Client.ElementLayer).Children.Add(alarm);
            //}
           
            
           

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

       
    }
}
