using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Reflection;
//using System.Windows.Forms;
using System.Net;
using System.Data;
using System.Data.SqlClient;

namespace MapApplication.Web
{
    public partial class DownLoadForm : System.Web.UI.Page
    {

        string filename ;
        string selectday, selectYestaday, Istilt, Zaxis, startime, endtime, IsValue, title0_down, title1_down, title2_down;
        protected void Page_Load(object sender, EventArgs e)
        {
            //WriteXls();
            string sensorID = Request["id"].ToString(), selectDate ="";
            
            selectDate = Request["date"].ToString();
            if (selectDate != "")
            {
                string[] spitstr = selectDate.Split(',');
                startime = spitstr[0].Trim();
                endtime = spitstr[1].Trim();
            }
            //Istilt = Request["Istilt"].ToString();
            title0_down = Request["title0"].ToString().Trim('-');
            title1_down = Request["title1"].ToString().Trim('-');
            title2_down = Request["title2"].ToString().Trim('-');

            if(Request["IsValue"].ToString() != null)
            IsValue = Request["IsValue"].ToString();
            Response.Clear();
            Export_CVS_Funtion(sensorID,selectDate);


        }


        //private System.Data.DataSet dataSet;
        private void Export_CVS_Funtion(string sensorID,string selectDate)
        {

            SqlConnection conn = new SqlConnection("server=192.192.161.2;database=SSHMC01;uid=david;pwd=ufjl0683@");
            
            conn.Open();
            
            SqlCommand s_com = new SqlCommand();



            //if (Istilt == "1")
            //    Zaxis = "溫度";
            //else
            //    Zaxis = "Z軸";
            

            //DateTime DT = Convert.ToDateTime(selectDate);
            //selectDate = string.Format("{0:G}", DT);//2005-11-5 14:23:23
            if (IsValue != "N")
            {
                if (selectDate != " ")//other days
                {

                    DateTime ConverterStarTime = Convert.ToDateTime(startime);
                    startime = ConverterStarTime.ToString("yyyy") + "/" + ConverterStarTime.ToString("MM") + "/" + ConverterStarTime.ToString("dd") + " " + ConverterStarTime.ToString("HH:mm:ss");

                    DateTime ConverterEndTime = Convert.ToDateTime(endtime);
                    ConverterEndTime = ConverterEndTime.AddDays(1);
                    endtime = ConverterEndTime.ToString("yyyy") + "/" + ConverterEndTime.ToString("MM") + "/" + ConverterEndTime.ToString("dd") + " " + ConverterEndTime.ToString("HH:mm:ss");

                    s_com.CommandText = "SELECT    TIMESTAMP As '時間', VALUE0 As " + title0_down + ", VALUE1 As " + title1_down + ", VALUE2 As " + title2_down + "  FROM          tblTC10MinDataLog WHERE      (TIMESTAMP >= '" + startime + "') AND   (TIMESTAMP < '" + endtime + "') AND (SENSOR_ID = '" + sensorID + "') AND (ISVALID = 'Y') ORDER BY TIMESTAMP";
                }
                else//today
                {
                    s_com.CommandText = "SELECT    TIMESTAMP As '時間', VALUE0 As " + title0_down + ", VALUE1 As " + title1_down + ", VALUE2 As " + title2_down + "  FROM          tblTC10MinDataLog WHERE      (TIMESTAMP >= CONVERT(char(11),GETDATE() , 120)) AND (SENSOR_ID = '" + sensorID + "' ) AND (ISVALID = 'Y') ORDER BY TIMESTAMP";
                    DateTime ConverterTime = Convert.ToDateTime(DateTime.Today);
                    selectday = ConverterTime.ToString("yyyy") + "/" + ConverterTime.ToString("MM") + "/" + ConverterTime.ToString("dd") + " " + ConverterTime.ToString("HH:mm:ss");

                }
            }
            else
            {
                if (selectDate != " ")//other days
                {

                    DateTime ConverterStarTime = Convert.ToDateTime(startime);
                    startime = ConverterStarTime.ToString("yyyy") + "/" + ConverterStarTime.ToString("MM") + "/" + ConverterStarTime.ToString("dd") + " " + ConverterStarTime.ToString("HH:mm:ss");

                    DateTime ConverterEndTime = Convert.ToDateTime(endtime);
                    ConverterEndTime = ConverterEndTime.AddDays(1);
                    endtime = ConverterEndTime.ToString("yyyy") + "/" + ConverterEndTime.ToString("MM") + "/" + ConverterEndTime.ToString("dd") + " " + ConverterEndTime.ToString("HH:mm:ss");

                    s_com.CommandText = "SELECT    TIMESTAMP As '時間', VALUE0 As " + title0_down + ", VALUE1 As " + title1_down + ", VALUE2 As " + title2_down + "  FROM          tblTC10MinDataLog WHERE      (TIMESTAMP >= '" + startime + "') AND   (TIMESTAMP < '" + endtime + "') AND (SENSOR_ID = '" + sensorID + "') ORDER BY TIMESTAMP";
                }
                else//today
                {
                    s_com.CommandText = "SELECT    TIMESTAMP As '時間', VALUE0 As " + title0_down + ", VALUE1 As " + title1_down + ", VALUE2 As " + title2_down + "  FROM          tblTC10MinDataLog WHERE      (TIMESTAMP >= CONVERT(char(11),GETDATE() , 120)) AND (SENSOR_ID = '" + sensorID + "' )  ORDER BY TIMESTAMP";
                    DateTime ConverterTime = Convert.ToDateTime(DateTime.Today);
                    selectday = ConverterTime.ToString("yyyy") + "/" + ConverterTime.ToString("MM") + "/" + ConverterTime.ToString("dd") + " " + ConverterTime.ToString("HH:mm:ss");

                }
            }



            s_com.Connection = conn;    //select convert(char(19),GETDATE()+2,120)


            




            
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(s_com);
            sda.Fill(dt);
            

            GridView1.DataSource = dt;
            GridView1.DataBind();

            DateTime parsed;

            if (DateTime.TryParseExact(startime, "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parsed))
            {
                //filename = parsed.ToString("yyyyMMdd") + "-SensorID_" + Request["id"].ToString();
                DateTime stardatetime = Convert.ToDateTime(startime),enddatetime = Convert.ToDateTime(endtime);
                enddatetime =enddatetime.AddDays(-1);
                filename = stardatetime.ToString("yyyyMMdd") +"-"+ enddatetime.ToString("yyyyMMdd") + "-SensorID_" + Request["id"].ToString();

            }
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + filename + ".xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write("<meta http-equiv=Content-Type content=text/html;charset=utf-8>");
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            //關閉換頁跟排序
            GridView1.AllowSorting = false;
            GridView1.AllowPaging = false;

            //'移去不要的欄位
            //GridView1.Columns.RemoveAt(GridView1.Columns.Count - 1);
            //GridView1.DataBind();

            //    建立假HtmlForm避免以下錯誤()
            //    Control() 'GridView1' of type 'GridView' must be placed inside 
            //a form tag with runat=server. 
            //    另一種做法是override(VerifyRenderingInServerForm後不做任何事)
            //這樣就可以直接GridView1.RenderControl(htw);

            ////System.Web.UI.HtmlControls.HtmlForm hf = new System.Web.UI.HtmlControls.HtmlForm();
            ////Controls.Add(hf);
            ////hf.Controls.Add(GridView1);
            ////hf.RenderControl(htw);
            Response.Clear();
            //GridView1.RenderControl(htw);
            //Response.Write(sw.ToString());




            

            conn.Close();



        }

        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }






















        //private void WriteXls()
        //{
        //    Console.WriteLine("WriteXls");
        //    //啟動Excel應用程式
        //    xlApp = new Microsoft.Office.Interop.Excel.Application();

        //    if (xlApp == null)
        //    {
        //        Console.WriteLine("Error! xlApp");
        //        return;
        //    }
        //    //用Excel應用程式建立一個Excel物件，也就是Workbook。並取得Workbook中的第一個sheet。這就是我們要操作資料的地方。
        //    wb = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
        //    ws = (Worksheet)wb.Worksheets[1];
        //    if (ws == null)
        //    {
        //        Console.WriteLine("Error! ws");
        //    }
        //    //要在Excel儲存資料，有三種方式，以下分別介紹。利用Range物件，設定要儲存資料的儲存格範圍。
        //    // Select the Excel cells, in the range c1 to c7 in the worksheet.
        //    Range aRange = ws.get_Range("C1", "C11");
        //    if (aRange == null)
        //    {
        //        Console.WriteLine("Could not get a range. Check to be sure you have the correct versions of the office DLLs.");
        //    }
        //    // Fill the cells in the C1 to C7 range of the worksheet with the number 6.  
        //    Object[] args = new Object[1];
        //    args[0] = 7;
        //    aRange.Value2 = args;
        //    //衍生自上面方法，但是在儲存資料的時候，可以用InvokeMember呼叫aRange的資料成員(成員函式?)。
        //    //aRange.GetType().InvokeMember("Value", BindingFlags.SetProperty, null, aRange, args);
        //    //利用Cells屬性，取得單一儲存格，並進行操作。
        //    string[] number = { "A", "B", "C", "D", "E", "F", "G", "H" };
        //    foreach (string s in number)
        //    {
        //        Range aRange2 = (Range)ws.Cells["1", s];
        //        Object[] args2 = new Object[1];
        //        args2[0] = s;
        //        aRange2.Value2 = args2;
        //    }

        //    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        //    saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
        //    saveFileDialog1.Title = "Save an Image File";
        //    saveFileDialog1.ShowDialog();







        //    //最後，呼叫SaveAs function儲存這個Excel物件到硬碟。
        //    wb.SaveAs(@"C:\test2.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlXMLSpreadsheet, mObj_opt, mObj_opt, mObj_opt, mObj_opt, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, mObj_opt, mObj_opt, mObj_opt, mObj_opt, mObj_opt);

        //    Console.WriteLine("save");
        //    wb.Close(false, mObj_opt, mObj_opt);
        //    xlApp.Workbooks.Close();
        //    xlApp.Quit();
        //    //刪除 Windows工作管理員中的Excel.exe 進程，  
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(aRange);

        //    xlApp = null;
        //    wb = null;
        //    ws = null;
        //    aRange = null;

        //    //呼叫垃圾回收  
        //    GC.Collect();
        //}
     


    }
}