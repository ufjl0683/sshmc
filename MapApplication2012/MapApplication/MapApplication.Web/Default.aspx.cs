using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapApplication.Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Btn_Login_Click(object sender, EventArgs e)
        {
            MapApplication.Web.SSHMC01Entities db=new SSHMC01Entities();
            string userid,Password ;
            userid=Txt_MemberNo.Text;
            Password=Txt_Password.Text;
           ;
            tblUser q ;
            if (customerid.Value == "")
                q = (from n in db.tblUser where userid == n.USER_ID && n.USER_PW == Password select n).FirstOrDefault<tblUser>();
            else
            {
               int cid = int.Parse(customerid.Value);
                q = (from n in db.tblUser where userid == n.USER_ID && n.USER_PW == Password && n.CUSTOMER_ID == cid select n).FirstOrDefault<tblUser>();
            }
            if (q == null)
                Response.Write("<script>alert('帳號密碼錯誤!')</script>");
            else
            {
                Session["CustomerID"] = q.CUSTOMER_ID;
                

                try
                {
                    db.tblUserLogin.AddObject(new tblUserLogin(){ TIMESTAMP=DateTime.Now, USER_ID=userid});
                    db.SaveChanges();
                }
                catch {   
                        
                    }
                Response.Redirect("MapApplicationTestPage.aspx?CustomerID=" + q.CUSTOMER_ID+"&UserID="+q.USER_ID);
            }



        }
    }
}