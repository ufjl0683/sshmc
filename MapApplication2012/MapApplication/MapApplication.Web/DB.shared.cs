using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;

namespace MapApplication.Web
{
    public partial class vwSiteDegree
    {
        public bool HasSurveyData
        {

            get
            {
                
                return this.DESCRIPTION!=null || this.PHOTO_PATH!=null;
            }
        }
    }
}