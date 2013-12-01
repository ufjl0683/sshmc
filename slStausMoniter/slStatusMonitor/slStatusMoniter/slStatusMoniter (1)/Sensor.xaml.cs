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
using slStatusMoniter.Web;


namespace slStatusMoniter
{
    public partial class Sensor : UserControl
    {
        vwSensorStatus status;
        public Sensor()
        {
         
            InitializeComponent();


          //  this.DataContext = this.status = status;

         //   status.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(status_PropertyChanged);
            
        }

        public void SetDataContext(vwSensorStatus status)
        {
            this.DataContext =this.status=status;
            ShowState();
            
        }

        void ShowState()
        {
            switch (status.LEVEL)
            {

                case 0:
                    VisualStateManager.GoToState(this, "lv0", false);
                    break;
                case 1:
                    VisualStateManager.GoToState(this, "lv1", false);
                    break;
                case 2:
                    VisualStateManager.GoToState(this, "lv2", false);
                    break;
                case 3:
                    VisualStateManager.GoToState(this, "lv3", false);
                    break;
                case 4:
                    VisualStateManager.GoToState(this, "lv4", false);
                    break;
            }
        }
        void status_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.PropertyName == "LEVEL")
            {
                ShowState();
            }
        }
    }
}
