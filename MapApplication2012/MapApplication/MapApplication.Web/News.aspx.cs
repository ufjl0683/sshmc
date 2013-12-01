using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapApplication.Web
{
    public partial class News : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           DataService.SSHMCDataServiceClient client = new DataService.SSHMCDataServiceClient();

           DataService.tblPre_disasterNotified[] res= client.GetDisasterInfo();
           string htmlstr="";
           foreach (DataService.tblPre_disasterNotified info in res)
           {
               string classtype = "side";

               if (info.PRE_ADMONISH_CLASS.Contains("雨"))
                   classtype = "main2";
               else if (info.PRE_ADMONISH_CLASS.Contains("地震") || info.PRE_ADMONISH_CLASS.Contains("颱風"))
                   classtype = "side";

               htmlstr+= GenerateDivInformation(  info.TITLE, ((DateTime)info.TIMESTAMP).ToString("yyyy/MM/dd HH:mm:ss"), info.CONTENT, classtype  );
               
           }
           this.Literal1.Text = htmlstr;
        }

        string GenerateDivInformation(string header,string datetime,string content,string type)
        {
           return "<div class=\"" + type + "\">" +
                 "<h2>" + header + "</h2>" +
                 "<h6>" + datetime + "</h6>" +
                 "<p>" + content + "</p></div>";

        }
    }
}