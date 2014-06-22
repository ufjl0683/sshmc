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

namespace MapApplication.DataService
{
    public partial class vwSiteDegree
    {
        public string SurveyImageFilePath
        {
            get
            {
                 return "http://192.192.161.4/mobile/FileUploads/"+"S"+this.NID+".jpg";
            }
        }

    }
}
