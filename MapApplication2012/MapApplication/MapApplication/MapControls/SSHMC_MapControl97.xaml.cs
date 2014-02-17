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
//using ESRI.ArcGIS.Client.Toolkit.DataSources;
using System.ServiceModel.DomainServices.Client;
using MapApplication.Web;
using System.Collections.ObjectModel;
using MapApplication.Controls;
using System.Windows.Threading;
using System.Windows.Browser;
using System.Threading.Tasks;



namespace MapApplication.MapControls
{
   //public enum enumDisplayMode
   // {
   //     GLOBAL_VIEW,
   //     SITE_VIEW
   // }

   
    public partial class SSHMC_MapControl97 : UserControl
    {
        
        public event EventHandler OnClick;
        double currentx = 0, currenty = 0;
        enumDisplayMode view_mode = enumDisplayMode.GLOBAL_VIEW;
    
        ObservableCollection<DataService.vwSiteDegree> siteCollection;
        System.Collections.Generic.Dictionary<string,Pin> dictPins = new System.Collections.Generic.Dictionary<string,Pin>();
        System.Collections.Generic.Dictionary<int, Sensor> dictSensors = new System.Collections.Generic.Dictionary<int, Sensor>();
        System.Collections.Generic.Dictionary<string, CCTV> dictCCTVs = new Dictionary<string, CCTV>();
        ObservableCollection<vwSensorDegree> sensorsCollection;
        string CustomerID,UserID;
        int GLOBAL_VIEW_LEVEL=6;
        int SITE_VIEW_LEVEL = 12;
        Envelope INIT_EXTENT = new Envelope(64009.3303799998, 2403270.13928, 365327.76582, 2818099.11252);
        System.Windows.Threading.DispatcherTimer tmrCheckDegree = new DispatcherTimer();
        //  const int constWKID = 102100;
        public SSHMC_MapControl97()
        {
            InitializeComponent();
        
        }
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.htmlhost.SetHostDivVisible(false);

             //  HtmlPage.Document

          
        }

        public void Initial(string CustomerID,string UserID )
        {
            this.CustomerID = CustomerID;
            this.UserID = UserID;
            SetViewMode(enumDisplayMode.GLOBAL_VIEW, CustomerID);
            tmrCheckDegree.Interval = TimeSpan.FromSeconds(20);
           
            this.imgLOGO.Source =  new System.Windows.Media.Imaging.BitmapImage(new Uri("http://192.192.161.4/pic/logo/" + CustomerID + ".jpg"));
            this.tmrCheckDegree.Tick += (s, a) =>
            {
                
                UpdateSiteDegree(CustomerID);
                UpdateSensorDegree();

            };
            tmrCheckDegree.Start();
           
        }

        string CURRENT_SITE_ID;
        public void SetViewMode(enumDisplayMode mode,string cust_site_id)
        {
            if (mode == enumDisplayMode.GLOBAL_VIEW)
            {
                Load_Site(cust_site_id);
                this.btmGoback.Visibility = System.Windows.Visibility.Collapsed;
                btnBim.Visibility = System.Windows.Visibility.Collapsed;
                btnBim.IsChecked = false;
                (this.map1.Layers["siteLyr"] as Layer).Visible = true;
               
                     
                SetCCTVGridExpand(false, System.Windows.Visibility.Collapsed);
                this.largeCCTV.DisMiss();
                this.largeCCTVPanel.Visibility = Visibility.Collapsed;
             //   this.largeCCTVPanel.Opacity = 0;
            }
            else
            {
                CURRENT_SITE_ID = cust_site_id;
                this.btmGoback.Visibility = System.Windows.Visibility.Visible;
                 DataService.vwSiteDegree site = (from n in siteCollection where (n.SITE_ID == cust_site_id) select n).FirstOrDefault();
                if (site == null)
                    return;

                if (site.ISBIM)
                    btnBim.Visibility = System.Windows.Visibility.Visible;
                else
                    btnBim.Visibility = System.Windows.Visibility.Collapsed;

                btnBim.IsChecked = false; ;
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

        public void UpdateSiteDegree(string customerid)
        {
           // MapApplication.Web.DbContext db = new Web.DbContext();
           //EntityQuery<vwSiteDegree> qry;
           // if(customerid=="1")
           //      qry= from n in db.GetVwSiteDegreeQuery() select n; 
           // else
           //     qry= from n in db.GetVwSiteDegreeQuery() where n.CUSTOMER_ID==System.Convert.ToInt32(customerid)  select n;
           //LoadOperation<vwSiteDegree> lo = db.Load<vwSiteDegree>(qry);
           //lo.Completed += (s, a) =>
           //    {
           //        if (lo.Error != null)
           //            return;
           //        foreach (vwSiteDegree siteinfo in lo.Entities)
           //        {
           //            if (!dictPins.ContainsKey(siteinfo.SITE_ID))
           //                continue;

           //            (dictPins[siteinfo.SITE_ID].DataContext as vwSiteDegree).CURRENT_DEGREE = siteinfo.CURRENT_DEGREE;

           //        }
           //    };
            DataService.SSHMCDataServiceClient client = new DataService.SSHMCDataServiceClient();
            client.GetSiteInfoCompleted+=(s,a)=>
                {
                    if (a.Error != null)
                        return;
                     foreach (DataService.vwSiteDegree siteinfo in a.Result)
                     {
                         if (!dictPins.ContainsKey(siteinfo.SITE_ID))
                             continue;

                         (dictPins[siteinfo.SITE_ID].DataContext as DataService.vwSiteDegree).CURRENT_DEGREE = siteinfo.CURRENT_DEGREE;

                     }

                };
            client.GetSiteInfoAsync(UserID);
            
        }
        public void UpdateSensorDegree( )
        {
            if (this.view_mode != enumDisplayMode.SITE_VIEW)
                return;
            MapApplication.Web.DbContext db = new Web.DbContext();
            EntityQuery<vwSensorDegree> qry = from n in db.GetVwSensorDegreeQuery() where n.SITE_ID==CURRENT_SITE_ID  select n;
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


            EntityQuery<tblTC> qBA = from n in db.GetTblTCQuery() where n.SITE_ID == this.CURRENT_SITE_ID && n.DEVICE_TYPE == "BA" select n;

            LoadOperation<tblTC> loBA = db.Load<tblTC>(qBA);
            loBA.Completed += (s, a) =>
            {
                if (loBA.Error != null)
                {
                    MessageBox.Show(lo.Error.Message);
                    return;
                }
                if (loBA.Entities.Count() == 0)
                    txtBA.Visibility = System.Windows.Visibility.Collapsed;
                else
                {
                    txtBA.Visibility = System.Windows.Visibility.Visible;
                    if (loBA.Entities.FirstOrDefault().ISCONNECTED == "N")
                        txtBA.Foreground = new SolidColorBrush(Colors.Gray);
                    else
                        txtBA.Foreground = new SolidColorBrush(Colors.Green);
                }


            };

        }

        

        private void loadSiteCCTV(DataService.vwSiteDegree site)
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

        void loadSiteSensor(DataService.vwSiteDegree site)
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
                EntityQuery<tblTC> qBA = from n in db.GetTblTCQuery() where n.SITE_ID == this.CURRENT_SITE_ID && n.DEVICE_TYPE == "BA" select n;
                LoadOperation<tblTC> loBA = db.Load<tblTC>(qBA);
                loBA.Completed += (s, a) =>
                {
                    if (loBA.Error != null)
                    {
                        MessageBox.Show(lo.Error.Message);
                        return;
                    }
                    if (loBA.Entities.Count() == 0)
                        txtBA.Visibility = System.Windows.Visibility.Collapsed;
                    else
                    {
                        txtBA.Visibility = System.Windows.Visibility.Visible;
                        if (loBA.Entities.FirstOrDefault().ISCONNECTED == "N")
                            txtBA.Foreground = new SolidColorBrush(Colors.Gray);
                        else
                            txtBA.Foreground = new SolidColorBrush(Colors.Green);
                    }


                };


        }

     

        void Load_Site(string customerid)
        {
            #region old ria service
            //MapApplication.Web.DbContext db = new Web.DbContext();
            //if (this.siteCollection == null)
            //{
            //    EntityQuery<vwSiteDegree> q ;
            //    if (customerid == "1")
            //    {
            //        q = db.GetVwSiteDegreeQuery();
            //        (this.map1.Layers["buildingPointLyr"] as FeatureLayer).Visible = false;
            //      //  (this.map1.Layers["buildingPointLyr"] as FeatureLayer).Refresh();
            //    }
            //    else
            //    {
            //        q = db.GetVwSiteDegreeQuery().Where(n => n.CUSTOMER_ID == System.Convert.ToInt32(customerid) || n.CUSTOMER_ID == 1);

            //        (this.map1.Layers["buildingPointLyr"] as FeatureLayer).Where = "CUSTOMER_ID<>1 and CUSTOMER_ID<>" + customerid;
            //        (this.map1.Layers["buildingPointLyr"] as FeatureLayer).Refresh();
            //    }
                
            //    LoadOperation<vwSiteDegree> lo = db.Load<vwSiteDegree>(q);
            //    lo.Completed += (s, a) =>
            //        {
            //            if (lo.Error != null)
            //            {
            //                MessageBox.Show(lo.Error.Message);
            //                return;
            //            }

            //            siteCollection = new ObservableCollection<vwSiteDegree>(lo.Entities);
            //            DisplayGlobalAndBindingSite();
            //            BindingMenu();

            //            return;
            //        };

            //}
            #endregion

            DataService.SSHMCDataServiceClient client = new DataService.SSHMCDataServiceClient();
            client.GetSiteInfoCompleted  += (s, a) =>
                {
                    if (a.Error != null)
                    {
                        MessageBox.Show(a.Error.Message);
                        return;
                    }
                    siteCollection = new ObservableCollection<DataService.vwSiteDegree>(a.Result);
                               DisplayGlobalAndBindingSite();
                               BindingMenu();

                                return;
                };
            client.GetSiteInfoAsync(UserID);
            if (siteCollection!=null)
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
            foreach (DataService.vwSiteDegree site in siteCollection)
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
            this.map1.Extent = INIT_EXTENT;
        }

        Envelope ConvertPointToEnvelop(MapPoint point )
        {

            MapPoint mp = ConvertMapPointTo102100(point);
            return new Envelope(mp, mp);
           
            //ESRI.ArcGIS.Client.Projection.WebMercator wm = new ESRI.ArcGIS.Client.Projection.WebMercator();
            ////return wm.FromGeographic(new ESRI.ArcGIS.Client.Geometry.Envelope(point, point) { SpatialReference = new SpatialReference(org_wkid) }).Extent;
            //return wm.FromGeographic(point).Extent;
        }
        MapPoint ConvertMapPointTo102100(MapPoint mp)
        {
           // ESRI.ArcGIS.Client.Projection.WebMercator wm = new ESRI.ArcGIS.Client.Projection.WebMercator();
           //MapPoint tmp= wm.FromGeographic(mp) as MapPoint;
           double x, y;
           ArcGISLib.Wgs84Tw97Transform.lonlat_To_twd97(mp.X, mp.Y,out x,out y);
           return new MapPoint(x, y,new SpatialReference(102443));

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

            try
            {
                Lod[] Lods = (this.map1.Layers["BaseMap"] as TiledLayer).TileInfo.Lods;
                for (int i = 0; i < Lods.Length; i++)
                {
                    if (Math.Abs(this.map1.Resolution - Lods[i].Resolution) < 1 || this.map1.Resolution >= Lods[i].Resolution)
                    {
                        return i;
                    }

                }
            }
            catch { ;}
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

            MapApplication.DataService.vwSiteDegree sitedata = menu.DataContext as MapApplication.DataService.vwSiteDegree;
           MapPoint mp = new MapPoint((double)sitedata.X, (double)sitedata.Y);
           this.ZoomToLevel(GLOBAL_VIEW_LEVEL, ConvertMapPointTo102100(mp));

           dictPins[sitedata.SITE_ID].SetBlind();

        }
        private void SiteMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            
            SiteMenu menu = sender as Controls.SiteMenu;
            MapApplication.DataService.vwSiteDegree sitedata = menu.DataContext as MapApplication.DataService.vwSiteDegree;
           // MapPoint mp = new MapPoint((double)sitedata.X, (double)sitedata.Y);
           // this.ZoomToLevel(GLOBAL_VIEW_LEVEL, ConvertMapPointTo102100(mp));

            dictPins[sitedata.SITE_ID].StopBlind();
          
        }

        private void SiteMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
              DataService.vwSiteDegree sitedata = (sender as SiteMenu).DataContext as DataService.vwSiteDegree;
              SetViewMode(enumDisplayMode.SITE_VIEW, sitedata.SITE_ID);

             //SiteMenu menu = sender as Controls.SiteMenu;
             // MapApplication.Web.vwSiteDegree sitedata = menu.DataContext as MapApplication.Web.vwSiteDegree;
              // MapPoint mp = new MapPoint((double)sitedata.X, (double)sitedata.Y);
              // this.ZoomToLevel(GLOBAL_VIEW_LEVEL, ConvertMapPointTo102100(mp));

              dictPins[sitedata.SITE_ID].StopBlind();
             
        
        }

        private void btmGoback_Click(object sender, RoutedEventArgs e)
        {
           // map1.ZoomDuration = TimeSpan.FromMilliseconds(500);
             htmlhost.SetHostDivVisible(false);
             HtmlPage.Window.Invoke("closeBim"); 
            DispatcherTimer tmr = new DispatcherTimer();
            tmr.Interval = TimeSpan.FromMilliseconds(1000);
            tmr.Tick += (s, a) =>
                {
                    if (GetCurrentLevel() > GLOBAL_VIEW_LEVEL)
                        ZoomToLevel(GetCurrentLevel() - 2);
                    else
                    {
                        map1.ZoomTo(INIT_EXTENT);
                        SetViewMode(enumDisplayMode.GLOBAL_VIEW, "");
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
       //    
            this.txtBA.Visibility = System.Windows.Visibility.Collapsed;
         //   this.btmGoback.Visibility = System.Windows.Visibility.Collapsed;
        }

        private    void SensorMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            SensorMenu menu = sender as SensorMenu;
          //  menu.CaptureMouse();
            vwSensorDegree sensorInfo = menu.DataContext as vwSensorDegree;
            dictSensors[sensorInfo.SENSOR_ID].SetBlind();
            this.ZoomToLevel(SITE_VIEW_LEVEL,ConvertMapPointTo102100(new MapPoint((double)sensorInfo.X,(double)sensorInfo.Y)));
            if (btnBim.IsChecked==true)
            {
                   HtmlInvokeAsync("SelectObject", string.Format("sensor{0:000000}", sensorInfo.SENSOR_ID));
            }
        
        }

        private void SensorMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            SensorMenu menu = sender as SensorMenu;
            vwSensorDegree sensorInfo = menu.DataContext as vwSensorDegree;
            dictSensors[sensorInfo.SENSOR_ID].StopBlind();
          //  this.ZoomToLevel(SITE_VIEW_LEVEL, ConvertMapPointTo102100(new MapPoint((double)sensorInfo.X, (double)sensorInfo.Y)));
          //  menu.ReleaseMouseCapture();
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
            if (btnBim.IsChecked == true)
                return;
            SensorMenu menu = sender as SensorMenu;
            vwSensorDegree sensorInfo = menu.DataContext as vwSensorDegree;
            dictSensors[sensorInfo.SENSOR_ID].StopBlind();
            chldChart chld = new chldChart(((sender as SensorMenu).DataContext as vwSensorDegree).SENSOR_ID);
            chld.Show();
        }

        private void chkMudy_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                map1.Layers["MudyLyr"].Visible = true;
            }
            catch { ;}
        }

        private void chkMudy_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                map1.Layers["MudyLyr"].Visible = false;
            }
            catch { ;}
        }

        private void chkDangerZone_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                map1.Layers["DangerZoneLyr"].Visible = true;
            }
            catch { ;}
        }

        private void chkDangerZone_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                map1.Layers["DangerZoneLyr"].Visible = false;
            }
            catch
            { ;}
        }

        private void chkFlood_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                map1.Layers["FloodGroupLyr"].Visible = true;
            }
            catch { ;}
        }

        private void chkFlood_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                map1.Layers["FloodGroupLyr"].Visible = false;
            }
            catch { ;}
        }

        private void checkBox2_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkShelter_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                map1.Layers["ShelterLyr"].Visible = true;
            }
            catch { ;}
        }

        private void chkShelter_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                map1.Layers["ShelterLyr"].Visible = false;
            }
            catch { ;}
        }

        private void chkDisasterPark_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                map1.Layers["SaveParkLyr"].Visible = true;
            }
            catch { ;}
        }

        private void chkDisasterPark_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                map1.Layers["SaveParkLyr"].Visible = false;
            }
            catch { ;}

        }

        private void chkFireDept_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                map1.Layers["FireDeptLyr"].Visible = true;
            }
            catch { ;}
        }

        private void chkFireDept_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                map1.Layers["FireDeptLyr"].Visible = false;
            }
            catch { ;}
        }

        private void chkHospital_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                map1.Layers["HospitalLyr"].Visible = true;
            }
            catch { ;}
        }

        private void chkHospital_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                map1.Layers["HospitalLyr"].Visible = false;
            }
            catch { ;}
        }

        private void chkRspCenter_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                map1.Layers["RspCenterLyr"].Visible = true;
            }
            catch { ;}
        }

        private void chkRspCenter_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                map1.Layers["RspCenterLyr"].Visible = false;
            }
            catch { ;}
        }

        private void lstMenu_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (wrapP != null)
            {
                wrapP.Width = (sender as ListBox).ActualWidth;
            }
        }

        WrapPanel wrapP;
        private void WrapPanel_Loaded(object sender, RoutedEventArgs e)
        {
            this.wrapP = sender as WrapPanel;
            lstMenu_SizeChanged(lstMenu, null);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (btnBim.IsChecked == true)
                return;
            News ctlNews = new News();
            ctlNews.Margin = new Thickness(0);
            Grid.SetRow(ctlNews, 0);
            Grid.SetColumn(ctlNews, 0);
            Grid.SetColumnSpan(ctlNews, 2);
            ctlNews.Unloaded += (s, a) =>
                {
                    htmlhost.SetReSize();
                };
            this.LayoutRoot.Children.Add(ctlNews);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (btnBim.IsChecked == true)
                return;
            if (this.OnClick != null)
                this.OnClick(this, e);


        }

        private void chkPoliceDept_Checked(object sender, RoutedEventArgs e)
        {

            try
            {
                map1.Layers["PoliceLyr"].Visible = true;
            }
            catch { ;}
        }

        private void chkPoliceDept_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                map1.Layers["PoliceLyr"].Visible = false;
            }
            catch { ;}
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
   }

        private   async void btnBim_Checked(object sender, RoutedEventArgs e)
        {
            
            this.htmlhost.SetHostDivVisible(true);
          // System.Threading.Tasks.TaskCompletionSource<  HtmlPage.Window.Invoke("result", new string[] { string.Format("http://192.192.161.4/mobile/Resources/{0}.dwf",CURRENT_SITE_ID) }); 

            await HtmlInvokeAsync("result", string.Format("http://192.192.161.4/mobile/Resources/{0}.dwf", CURRENT_SITE_ID));
           // MessageBox.Show("ok");
        }

        Task<object> HtmlInvokeAsync(string result,params string[] param)
        {
            System.Threading.Tasks.TaskCompletionSource<object> source=new TaskCompletionSource<object>();
            HtmlPage.Window.Invoke(result, param );
              source.TrySetResult(new object());
              return source.Task;
        }
        private void btnBim_UnChecked(object sender, RoutedEventArgs e)
        {
            
            this.htmlhost.SetHostDivVisible(false);
            HtmlPage.Window.Invoke("closeBim"); 
        }

      

    }
}
