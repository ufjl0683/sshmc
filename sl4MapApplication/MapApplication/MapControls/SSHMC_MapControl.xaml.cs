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
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Toolkit.DataSources;
using System.ServiceModel.DomainServices.Client;
using MapApplication.Web;
using System.Collections.ObjectModel;
using MapApplication.Controls;
using System.Windows.Threading;
using System.ComponentModel;

namespace MapApplication.MapControls
{
   public enum enumDisplayMode
    {
        GLOBAL_VIEW,
        SITE_VIEW
    }
    public partial class SSHMC_MapControl : UserControl
    {
        double currentx = 0, currenty = 0;
        enumDisplayMode view_mode = enumDisplayMode.GLOBAL_VIEW;
      //  MapApplication.Web.DbContext db = new Web.DbContext();
        ObservableCollection<vwSiteDegree> siteCollection;
        System.Collections.Generic.Dictionary<string,Pin> dictPins = new System.Collections.Generic.Dictionary<string,Pin>();
        System.Collections.Generic.Dictionary<int, Sensor> dictSensors = new System.Collections.Generic.Dictionary<int, Sensor>();
        System.Collections.Generic.Dictionary<string, CCTV> dictCCTVs = new Dictionary<string, CCTV>();
        ObservableCollection<vwSensorDegree> sensorsCollection;
        
        int GLOBAL_VIEW_LEVEL=13;
        int SITE_VIEW_LEVEL = 19;
        Envelope INIT_EXTENT =  new Envelope(13361742, 2595102, 13692221, 2906181);
        System.Windows.Threading.DispatcherTimer tmrCheckDegree = new DispatcherTimer();
        //  const int constWKID = 102100;
        public SSHMC_MapControl()
        {
            InitializeComponent();
        }
        public bool IsDesignTime()
        {
            return DesignerProperties.GetIsInDesignMode(Application.Current.RootVisual);
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {


            if (!IsDesignTime())
            {
                SetViewMode(enumDisplayMode.GLOBAL_VIEW, "");
                tmrCheckDegree.Interval = TimeSpan.FromSeconds(20);
                this.tmrCheckDegree.Tick += (s, a) =>
                {

                    UpdateSiteDegree("");
                    UpdateSensorDegree();

                };
                tmrCheckDegree.Start();
            }
        }
        
        public void SetViewMode(enumDisplayMode mode,string site_id)
        {
            if (mode == enumDisplayMode.GLOBAL_VIEW)
            {
                Load_Site(site_id);
                this.btmGoback.Visibility = System.Windows.Visibility.Collapsed;
                (this.map1.Layers["siteLyr"] as Layer).Visible = true;
               
                     
                SetCCTVGridExpand(false, System.Windows.Visibility.Collapsed);
                this.largeCCTV.DisMiss();
                this.largeCCTVPanel.Visibility = Visibility.Collapsed;
             //   this.largeCCTVPanel.Opacity = 0;
            }
            else
            {
                this.btmGoback.Visibility = System.Windows.Visibility.Visible;
                vwSiteDegree site= (from n in siteCollection where n.SITE_ID==site_id  select n).FirstOrDefault();
                if (site == null)
                    return;

                loadSiteSensor(site );
                loadSiteCCTV(site);
                (this.map1.Layers["siteLyr"] as Layer).Visible = false;
                ZoomToLevel(SITE_VIEW_LEVEL, ConvertMapPointTo102100(new MapPoint((double)site.X, (double)site.Y)));
                this.map1.ExtentChanged+=new EventHandler<ExtentEventArgs>(map1_CustomExtentChanged);
                 
               // this.largeCCTVPanel.Opacity = 0;
              //  this.largeCCTV.Visibility = System.Windows.Visibility.Visible;
             //   this.largeCCTV.DataContext = null;  
            }

        
            view_mode = mode;
        }

        void map1_CustomExtentChanged(object sender, EventArgs arg)
        {
            SetCCTVGridExpand(true, Visibility.Visible);
            this.map1.ExtentChanged -= map1_CustomExtentChanged;
        }

        public void UpdateSiteDegree(string user_id)
        {
            MapApplication.Web.DbContext db = new Web.DbContext();
           EntityQuery<vwSiteDegree> qry= from n in db.GetVwSiteDegreeQuery() select n;
           LoadOperation<vwSiteDegree> lo = db.Load<vwSiteDegree>(qry);
           lo.Completed += (s, a) =>
               {
                   if (lo.Error != null)
                       return;
                   foreach (vwSiteDegree siteinfo in lo.Entities)
                   {
                       if (!dictPins.ContainsKey(siteinfo.SITE_ID))
                           continue;

                       (dictPins[siteinfo.SITE_ID].DataContext as vwSiteDegree).CURRENT_DEGREE = siteinfo.CURRENT_DEGREE;

                   }
               };

        }
        public void UpdateSensorDegree( )
        {
            MapApplication.Web.DbContext db = new Web.DbContext();
            EntityQuery<vwSensorDegree> qry = from n in db.GetVwSensorDegreeQuery()   select n;
            LoadOperation<vwSensorDegree> lo = db.Load<vwSensorDegree>(qry);
            lo.Completed += (s, a) =>
            {
                if (lo.Error != null)
                    return;
                foreach (vwSensorDegree sensorinfo in lo.Entities)
                {
                    if (!dictSensors.ContainsKey(sensorinfo.SENSOR_ID))
                        continue;

                  (dictSensors[sensorinfo.SENSOR_ID].DataContext as vwSensorDegree).CURRENT_DEGREE = sensorinfo.CURRENT_DEGREE;
                  (dictSensors[sensorinfo.SENSOR_ID].DataContext as vwSensorDegree).VALUE0 = sensorinfo.VALUE0;
                  (dictSensors[sensorinfo.SENSOR_ID].DataContext as vwSensorDegree).VALUE1 = sensorinfo.VALUE1;
                  (dictSensors[sensorinfo.SENSOR_ID].DataContext as vwSensorDegree).VALUE2 = sensorinfo.VALUE2;
                   // dictSensors[sensorinfo.SENSOR_ID].DataContext = sensorinfo;
                  dictSensors[sensorinfo.SENSOR_ID].PlayAlarm();
                }
            };

        }

        

        private void loadSiteCCTV(vwSiteDegree site)
        {
           // throw new NotImplementedException();
            MapApplication.Web.DbContext db = new Web.DbContext();
            string site_id = site.SITE_ID;
            EntityQuery<tblCCTV> qry = from n in db.GetTblCCTVQuery() where n.SITE_ID == site_id select n;
            LoadOperation<tblCCTV> lo = db.Load<tblCCTV>(qry);
           

            lo.Completed += (s, a) =>
               {
                   if (lo.Error != null)
                   {
                       MessageBox.Show(lo.Error.Message);
                       return;

                   }
                   dictCCTVs.Clear();
                   this.lstCCTV.ItemsSource = lo.Entities;
                   foreach (tblCCTV cctvInfo  in lo.Entities   )
                   {
                       ElementLayer lyr = this.map1.Layers["sensorLyr"] as ElementLayer;
                       CCTV sensor = new CCTV();

                       MapPoint mp = ConvertMapPointTo102100(new MapPoint((double)cctvInfo.X, (double)cctvInfo.Y));
                       sensor.DataContext = cctvInfo;

                       ElementLayer.SetEnvelope(sensor, new Envelope(mp, mp));
                       dictCCTVs.Add(cctvInfo.CCTV_ID, sensor);
                       lyr.Children.Add(sensor);
                   }

               };
        }

        void loadSiteSensor(vwSiteDegree site)
        {
            MapApplication.Web.DbContext db = new Web.DbContext();
                
            string site_id = site.SITE_ID;
            if (sensorsCollection != null)
                sensorsCollection.Clear();
            
                EntityQuery<vwSensorDegree> qry = from n in db.GetVwSensorDegreeQuery() where n.SITE_ID == site_id select n;
                LoadOperation<vwSensorDegree> lo = db.Load<vwSensorDegree>(qry);
                lo.Completed += (s, a) =>
                    {
                        if (lo.Error != null)
                        {
                            MessageBox.Show(lo.Error.Message);
                            return;

                        }
                        dictSensors.Clear();
                        sensorsCollection = new ObservableCollection<vwSensorDegree>(lo.Entities);
                        this.lstMenu.ItemTemplate = this.Resources["SITE_VIEW_TEMPLATE"] as DataTemplate;
                        this.lstMenu.ItemsSource = sensorsCollection;
                        foreach (vwSensorDegree sensorInfo in sensorsCollection)
                        {
                            ElementLayer lyr = this.map1.Layers["sensorLyr"] as ElementLayer;
                            Sensor sensor = new Sensor();

                            MapPoint mp = ConvertMapPointTo102100(new MapPoint((double)sensorInfo.X, (double)sensorInfo.Y));
                            sensor.DataContext = sensorInfo;
                            
                            ElementLayer.SetEnvelope(sensor, new Envelope(mp, mp));
                            dictSensors.Add(sensorInfo.SENSOR_ID, sensor);
                            lyr.Children.Add(sensor);



                        }



                    };
             
            

        }

     

        void Load_Site(string user_id)
        {
            MapApplication.Web.DbContext db = new Web.DbContext();
            if (this.siteCollection == null)
            {
                EntityQuery<vwSiteDegree> q = db.GetVwSiteDegreeQuery();
                LoadOperation<vwSiteDegree> lo = db.Load<vwSiteDegree>(q);
                lo.Completed += (s, a) =>
                    {
                        if (lo.Error != null)
                        {
                            MessageBox.Show(lo.Error.Message);
                            return;
                        }

                        siteCollection = new ObservableCollection<vwSiteDegree>(lo.Entities);
                        DisplayGlobalAndBindingSite();
                        BindingMenu();

                        return;
                    };

            }

            if(siteCollection!=null)
             BindingMenu();

           
        }

        private void BindingMenu()
        {
           // if (this.view_mode == enumDisplayMode.GLOBAL_VIEW)
                lstMenu.ItemTemplate = this.Resources["GLOBAL_VIEW_TEMPLATE"] as DataTemplate;
            lstMenu.ItemsSource = siteCollection;
 
              //throw new NotImplementedException();
        }
        void DisplayGlobalAndBindingSite( )
        {
            ESRI.ArcGIS.Client.ElementLayer lyr = this.map1.Layers["siteLyr"] as ESRI.ArcGIS.Client.ElementLayer;
            dictPins.Clear();
            foreach (vwSiteDegree site in siteCollection)
            {

                Pin pin = new Pin();

                MapPoint mp = new MapPoint() { X = (double)site.X, Y = (double)site.Y };


                pin.SetValue(ESRI.ArcGIS.Client.ElementLayer.EnvelopeProperty,
                     ConvertPointToEnvelop(mp ));

                pin.DataContext = site;
                
                lyr.Children.Add(pin);
                dictPins.Add(site.SITE_ID, pin);
            }
            view_mode = enumDisplayMode.GLOBAL_VIEW;
            this.map1.Extent = new Envelope(13361742, 2595102, 13692221, 2906181);
        }

        Envelope ConvertPointToEnvelop(MapPoint point )
        {
            
           
            ESRI.ArcGIS.Client.Projection.WebMercator wm = new ESRI.ArcGIS.Client.Projection.WebMercator();
            //return wm.FromGeographic(new ESRI.ArcGIS.Client.Geometry.Envelope(point, point) { SpatialReference = new SpatialReference(org_wkid) }).Extent;
            return wm.FromGeographic(point).Extent;
        }
        MapPoint ConvertMapPointTo102100(MapPoint mp)
        {
            ESRI.ArcGIS.Client.Projection.WebMercator wm = new ESRI.ArcGIS.Client.Projection.WebMercator();
           return  wm.FromGeographic(mp) as MapPoint;
        }

        void ZoomToLevel(int level)
        {

            double resolution = (this.map1.Layers["BaseMap"] as TiledLayer ).TileInfo.Lods[level].Resolution;
            this.map1.ZoomToResolution(resolution);
        }

        void ZoomToLevel(int level, MapPoint point)
        {
            bool zoomentry = false;
            double resolution;
            if (level == -1)
                resolution = map1.Resolution;
            else
                resolution = (this.map1.Layers["BaseMap"] as TiledLayer).TileInfo.Lods[level].Resolution;


            if (Math.Abs(map1.Resolution - resolution) < 0.05)
            {
                this.map1.PanTo(point);
                return;
            }
            zoomentry = false;
            this.map1.ZoomToResolution(resolution);

            map1.ExtentChanged += (s, a) =>
            {
                if (!zoomentry)
                    this.map1.PanTo(point);

                zoomentry = true;

                //   SwitchLayerVisibility();
            };



        }

        int GetCurrentLevel()
        {

            Lod[] Lods = (this.map1.Layers["BaseMap"] as TiledLayer).TileInfo.Lods;
            for (int i = 0; i < Lods.Length; i++)
            {
                if (Math.Abs(this.map1.Resolution - Lods[i].Resolution) < 1 || this.map1.Resolution >= Lods[i].Resolution)
                {
                    return i;
                }

            }
            return 0;
        }
        void SwitchLayerVisibility()
        {
            //int currentLevel = this.GetCurrentLevel();

            //if (currentLevel > 0)
            //    (this.map1.Layers["DefenseBaselyr"] as GraphicsLayer).Visible = true;
            //else
            //    (this.map1.Layers["DefenseBaselyr"] as GraphicsLayer).Visible = false;


            //if (currentLevel > 2)
            //    (this.map1.Layers["DefenseBaseNamelyr"] as GraphicsLayer).Visible = true;
            //else
            //    (this.map1.Layers["DefenseBaseNamelyr"] as GraphicsLayer).Visible = false;

            //if (currentLevel >= 0 && currentLevel <= 5)
            //    (this.map1.Layers["DefenseCirclelyr"] as GraphicsLayer).Visible = true;
            //else
            //    (this.map1.Layers["DefenseCirclelyr"] as GraphicsLayer).Visible = false;



        }
        void LocateTo(int level, double x, double y)
        {

            ESRI.ArcGIS.Client.Geometry.MapPoint mp = new ESRI.ArcGIS.Client.Geometry.MapPoint(x, y, new SpatialReference(104137));
            if (currentx != x || currenty != y)
            {
                currentx = x;
                currenty = y;
                //load other spactial data here






                //(this.map1.Layers["elementlyr"] as ESRI.ArcGIS.Client.ElementLayer).Children.Clear();

                //Alarm alarm = new Alarm() { Width = 40, Height = 40 };
                //alarm.SetValue(ESRI.ArcGIS.Client.ElementLayer.EnvelopeProperty,
                //   new ESRI.ArcGIS.Client.Geometry.Envelope(mp, mp));
                //(this.map1.Layers["elementlyr"] as ESRI.ArcGIS.Client.ElementLayer).Children.Add(alarm);
            }

            this.ZoomToLevel(level, mp);
            //this.map1.ExtentChanged+=(s,a)=>
            //    {
            //        double width = this.map1.Extent.XMax - this.map1.Extent.XMin;
            //        double height = this.map1.Extent.YMax - this.map1.Extent.YMin;
            //        this.map1.ZoomTo(new ESRI.ArcGIS.Client.Geometry.Envelope()
            //            {
            //                XMin = mp.X - width / 2,
            //                XMax = mp.X + width / 2,
            //                YMin = mp.Y - height / 2,
            //                YMax = mp.Y + height / 2,
            //                SpatialReference = new SpatialReference(104137)
            //            }
            //            );
            //    };

            //double width=this.map1.Extent.XMax-this.map1.Extent.XMin;
            //double height=this.map1.Extent.YMax-this.map1.Extent.YMin;
            //this.map1.ZoomTo(new ESRI.ArcGIS.Client.Geometry.Envelope()
            //    {
            //       XMin=mp.X-width/2,
            //       XMax=mp.X+width/2,
            //       YMin=mp.Y-height/2,
            //       YMax=mp.Y+height/2,
            //       SpatialReference = new SpatialReference(104137)
            //    }
            //    );
        }

        private void map1_ExtentChanged(object sender, ExtentEventArgs e)
        {
            this.txtLevel.Text = "Level:" + this.GetCurrentLevel();
        }

        private void SiteMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            SiteMenu menu = sender as Controls.SiteMenu;
            MapApplication.Web.vwSiteDegree sitedata = menu.DataContext as MapApplication.Web.vwSiteDegree;
           MapPoint mp = new MapPoint((double)sitedata.X, (double)sitedata.Y);
           this.ZoomToLevel(GLOBAL_VIEW_LEVEL, ConvertMapPointTo102100(mp));

           dictPins[sitedata.SITE_ID].SetBlind();

        }

        private void SiteMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
              vwSiteDegree sitedata = (sender as SiteMenu).DataContext as vwSiteDegree;
              SetViewMode(enumDisplayMode.SITE_VIEW, sitedata.SITE_ID);
          
            ;
           
        
        }

        private void btmGoback_Click(object sender, RoutedEventArgs e)
        {
           // map1.ZoomDuration = TimeSpan.FromMilliseconds(500);
            DispatcherTimer tmr = new DispatcherTimer();
            tmr.Interval = TimeSpan.FromMilliseconds(1000);
            tmr.Tick += (s, a) =>
                {
                    if (GetCurrentLevel() > GLOBAL_VIEW_LEVEL)
                        ZoomToLevel(GetCurrentLevel() - 2);
                    else
                    {
                        map1.ZoomTo(INIT_EXTENT);
                        tmr.Stop();
                    }
                };

            tmr.Start();

            ElementLayer lyer = map1.Layers["sensorLyr"] as ElementLayer;
            lyer.Children.Clear();
            //foreach (Sensor snr in lyer.Children)
            //    lyer.Children.Remove(snr);
               
           // map1.Extent = INIT_EXTENT;
            this.lstCCTV.ItemsSource = null;
            SetViewMode(enumDisplayMode.GLOBAL_VIEW,"");
           
         //   this.btmGoback.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void SensorMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            SensorMenu menu = sender as SensorMenu;
            vwSensorDegree sensorInfo = menu.DataContext as vwSensorDegree;
            dictSensors[sensorInfo.SENSOR_ID].SetBlind();
            this.ZoomToLevel(SITE_VIEW_LEVEL,ConvertMapPointTo102100(new MapPoint((double)sensorInfo.X,(double)sensorInfo.Y)));
        }

        private void SensorMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            SensorMenu menu = sender as SensorMenu;
            vwSensorDegree sensorInfo = menu.DataContext as vwSensorDegree;
            dictSensors[sensorInfo.SENSOR_ID].StopBlind();
          //  this.ZoomToLevel(SITE_VIEW_LEVEL, ConvertMapPointTo102100(new MapPoint((double)sensorInfo.X, (double)sensorInfo.Y)));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.btnCollapse.Tag.ToString() == "1")
            {
                this.btnCollapse.Tag = "0";
                SetCCTVGridExpand(false, System.Windows.Visibility.Visible);
            }
            else
            {
                this.btnCollapse.Tag = "1";
                SetCCTVGridExpand(true, System.Windows.Visibility.Visible);
            }

        }

        void SetCCTVGridExpand(bool IsExpand, Visibility visible)
        {
            this.gridCCTV.Visibility = visible;
            if (IsExpand)
            {
                this.stbCCTVExpand.Begin();

            }
            else
            {
                dkfExpand.Value = -(gridCCTV.ActualWidth - 40);
                this.stbCCTVCollapse.Begin();
            }
         
            
        }

        private void CCTV_MouseEnter(object sender, MouseEventArgs e)
        {
            Controls.CCTV cctv = sender as Controls.CCTV;
            tblCCTV cctvinfo = cctv.DataContext as tblCCTV;
            dictCCTVs[cctvinfo.CCTV_ID].SetBlind();
            this.ZoomToLevel(SITE_VIEW_LEVEL,
                this.ConvertMapPointTo102100( new MapPoint((double)cctvinfo.X,(double)cctvinfo.Y)));
        }

        private void lstCCTV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCCTV.SelectedItem == null)
                return;
            this.largeCCTV.DataContext = lstCCTV.SelectedItem;

           this.largeCCTVPanel.Visibility = System.Windows.Visibility.Visible;
           // this.largeCCTVPanel.Opacity = 1;
            this.largeCCTV.SwitchCCTV();

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
               // this.largeCCTVPanel.Opacity = 0;
                
              //  this.largeCCTVPanel.Visibility = Visibility.Collapsed;
              
                lstCCTV.SelectedItem = null;
                this.largeCCTV.DisMiss();
                this.largeCCTVPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void SensorMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SensorMenu menu = sender as SensorMenu;
            vwSensorDegree sensorInfo = menu.DataContext as vwSensorDegree;
            dictSensors[sensorInfo.SENSOR_ID].StopBlind();
            chldChart chld = new chldChart(((sender as SensorMenu).DataContext as vwSensorDegree).SENSOR_ID);
            chld.Show();
        }

    }
}
