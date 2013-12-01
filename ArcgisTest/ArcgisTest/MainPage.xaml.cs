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

namespace ArcgisTest
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //foreach (ESRI.ArcGIS.Client.LayerInfo info in (this.map1.Layers["dyn1"] as ESRI.ArcGIS.Client.ArcGISDynamicMapServiceLayer).Layers)
            //{
            //   // info.
            //}
            (this.map1.Layers["dyn1"] as ESRI.ArcGIS.Client.ArcGISDynamicMapServiceLayer).VisibleLayers = new int[]{2};
        }
    }
}
