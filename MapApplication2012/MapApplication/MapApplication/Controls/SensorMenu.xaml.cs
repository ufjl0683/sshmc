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
using System.ComponentModel;

namespace MapApplication.Controls
{
    public partial class SensorMenu : UserControl
    {
        public SensorMenu()
        {
            InitializeComponent();
        }

        

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            INotifyPropertyChanged inotifier = this.DataContext as INotifyPropertyChanged;
            inotifier.PropertyChanged += new PropertyChangedEventHandler(inotifier_PropertyChanged);
        }

        void inotifier_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.PropertyName == "VALUE0")
                this.stbValue0Change.Begin();
            else if (e.PropertyName == "VALUE1")
                this.stbValue1Change.Begin();
            else if (e.PropertyName == "VALUE2")
                this.stbValue2Change.Begin();
                
        }
    }
}
