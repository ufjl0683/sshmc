using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Net.Mail;

namespace MapApplication.Web
{
    public partial class ChangePasswordPage : System.Web.UI.Page
    {
        string password;
        protected void Page_Load(object sender, EventArgs e)
        {
            //TextBox1.Text = "corn";
            //TextBox2.Text = "林育民";
            //TextBox3.Text = "cornming@gmail.com";

            if (Request["Verification"] != null)
            {
                password = Request["Verification"];
                //Response.Write("<script>alert('" + password + "');</script>");
                Label3.Text = "新密碼：";
                Label4.Text = "確認新密碼：";

                //TextBox1.Text = "sshmc04";
                //TextBox2.Text = "sshmc04";
                //TextBox3.Text = "cornming@gmail.com";

                TextBox1.TextMode = TextBoxMode.Password;
                TextBox2.TextMode = TextBoxMode.Password;
                CompareValidator1.Visible = true;
                
            }
        }

        string id, name, pwd, email,newPwd;


        protected void Button1_Click1(object sender, EventArgs e)
        {
            if (password == null)
            {
                string connectionString =
                    "server=192.192.161.2;database=SSHMC01;uid=david;pwd=ufjl0683@";
                SqlCommand dataCommand = new SqlCommand();
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = connectionString;
                conn.Open();
                dataCommand.Connection = conn;
                dataCommand.CommandText = "SELECT USER_ID,USER_NAME, USER_PW, USER_MAIL  FROM tblUser WHERE (USER_ID = '" + TextBox1.Text + "')";

                SqlDataReader dataReader = dataCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    id = dataReader.GetString(0);
                    name = dataReader.GetString(1);
                    pwd = dataReader.GetString(2);
                    email = dataReader.GetString(3);


                }


                if (id == TextBox1.Text && name == TextBox2.Text && email == TextBox3.Text)
                {

                    dataReader.Dispose();
                    newPwd = CreatePassword(pwd.Count());
                    dataCommand.CommandText = "UPDATE tblUser SET USER_PW='" + newPwd + "' WHERE (USER_ID = '" + TextBox1.Text + "')";

                    dataCommand.ExecuteNonQuery();
                    MailMain();
                }
                else
                {
                    if (id == TextBox1.Text && email != TextBox2.Text)
                    {
                        Label5.Visible = true;
                        Label5.Text = "您輸入的資料不符，請重新輸入。";
                    }
                    else if (id != TextBox1.Text && email != TextBox2.Text)
                    {
                        Label5.Visible = true;
                        Label5.Text = "輸入錯誤，很抱歉，查無此名稱。";
                    }
                    else
                    {
                        Label5.Visible = true;
                        Label5.Text = "輸入錯誤，請新輸入。";
                    }
                }


                conn.Close();
            }
            else { 
                    //驗證新舊密碼
                if (TextBox1.Text == TextBox2.Text && TextBox1.Text!="")
                {
                    string connectionString =
                    "server=192.192.161.2;database=SSHMC01;uid=david;pwd=ufjl0683@";
                    SqlCommand dataCommand = new SqlCommand();
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = connectionString;
                    conn.Open();
                    dataCommand.Connection = conn;
                    dataCommand.CommandText = "SELECT USER_ID,USER_NAME, USER_PW, USER_MAIL  FROM tblUser WHERE (USER_PW = '" + password + "')";

                    SqlDataReader dataReader = dataCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        id = dataReader.GetString(0);
                        name = dataReader.GetString(1);
                        pwd = dataReader.GetString(2);
                        email = dataReader.GetString(3);


                    }


                    

                        dataReader.Dispose();
                        dataCommand.CommandText = "UPDATE tblUser SET USER_PW='" + TextBox1.Text + "' WHERE (USER_PW = '" + password + "')";

                        dataCommand.ExecuteNonQuery();
                        MailMain();
                    


                    conn.Close();
                }
                else
                {
                    Response.Write("<script>alert('輸入錯誤!!');</script>");
                    TextBox1.Focus();
                }

            }
        }

        public void MailMain()
        {
            try
            {

                MailMessage message = new MailMessage();

                //message.From = new MailAddress("bridge.work@gm.cute.edu.tw", "結構物安全與防災系統", System.Text.Encoding.UTF8);
                message.From = new MailAddress("sshmc@cute.edu.tw", "結構物安全與防災系統", System.Text.Encoding.UTF8);
                //設定寄件信箱、寄件者、編碼方式
                message.To.Add(TextBox3.Text);
                //設定收件信箱
                if (password != null)
                {
                    message.Subject = "結構物安全與防災中心密碼已變更通知!";
                }
                else
                {
                    message.Subject = "結構物安全與防災中心忘記密碼通知!";
                }
                //設定信箱主旨
                if (password != null)
                {
                    message.Body = "您好，您的密碼已變更為"+ TextBox1.Text + "，謝謝!";
                }
                else
                {
                    message.Body = "您好，請點擊連結更改您的密碼http://192.192.161.4/sshmc/ChangePasswordPage.aspx?Verification=" + newPwd + "，謝謝!";
                }
                //設定信箱內容
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                //設定信箱主旨編碼方式
                message.BodyEncoding = System.Text.Encoding.UTF8;
                //設定信箱內容編碼方式
                message.IsBodyHtml = true;
                //啟用 HTML 格式
                message.Priority = MailPriority.High;
                //設定優先權

                //if (mailFiles != null)
                //{
                //    if (mailFiles.Count > 0)
                //    {
                //        foreach (string mailFile in mailFiles)
                //        {
                //            message.Attachments.Add(new Attachment(mailFile));
                //            //加入附加檔案
                //          }
                //    }
                //}

                SmtpClient smtp = new SmtpClient("mail.cute.edu.tw");

                //設定 Mail Server

                //smtp.EnableSsl = true;
                //啟用 SSL
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //指定外送電子信箱的處理方式 

                smtp.Credentials = new System.Net.NetworkCredential("sshmc@cute.edu.tw", "S$hmc@464");
                
                //利用帳號／密碼取得 Smtp 伺服器的憑證 

                smtp.Send(message);
                //發送
                Label5.Visible = true;

                if (password == null)
                    Label5.Text = "寄送成功!!請到您的EMAIL收信。";
                else
                    Label5.Text = "密碼已變更，請回上一頁登入，謝謝!";

            }
            catch (Exception ex)
            {
                Label5.Visible = true;
                //Label5.Text = "Exception is:" + ex.ToString();
                Label5.Text = "寄送失敗!!請通知管理人員。";
            }
        }

        private string CreatePassword(int Length)
        {
            string newPassword = "";
            string pattern = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < Length; i++)
            {
                newPassword += pattern[r.Next(0, pattern.Length)];
            }
            return newPassword;
        }
    }
}