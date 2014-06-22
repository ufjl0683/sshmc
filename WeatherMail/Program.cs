using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using OpenPop.Mime;
using System.ServiceModel;
using System.Text.RegularExpressions;

namespace WeatherMail
{
    class Program
    {
        static void Main(string[] args)
        {

            System.Threading.Thread th;
            th = new System.Threading.Thread(WeatrherMailTask);
            th.Start();
            ServiceHost host = new ServiceHost(typeof(Service1));
            host.Open();

        }

        static void WeatrherMailTask()
        {

            while (true)
            {

                try
                {
                    WeatherMailJob();
                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                System.Threading.Thread.Sleep(1000 * 60 * 5);
            }


        }
        static void WeatherMailJob()
        {

            OpenPop.Pop3.Pop3Client mclient = new OpenPop.Pop3.Pop3Client();
            mclient.Connect("mail.cute.edu.tw", 110, false);
            mclient.Authenticate("weather", "0988163835");
            int cnt = mclient.GetMessageCount();
            Console.WriteLine("Message cnt:" + cnt);
            string bodytext;

            for (int i = 1; i <= cnt; i++)
            {
                try
                {
                    bodytext = "";
                    Message msg = mclient.GetMessage(i);
                    string from = msg.Headers.From.MailAddress.Address;

                    Console.WriteLine("from:" + from);

                    Console.WriteLine("subject:" + msg.Headers.Subject);
                    if (from != "eservice@cwb.gov.tw" && from != "ufjl0683@cute.edu.tw" && from != "sshmc@cute.edu.tw")
                        continue;
                    //  string b = msg.MessagePart.GetBodyAsText();
                    // string htmlString = @"<p>I'm HTML!</p>";
                    //   b= Regex.Replace(b, @"(<(.|\n)*?>|&nbsp;)", "");
                    //  Console.WriteLine(Program.StripHTML( b));
                    if (!msg.MessagePart.IsText)
                    {
                        MessagePart msgpart = msg.FindFirstPlainTextVersion();
                        if (msgpart != null && msgpart.IsText)
                        {
                            //  Console.WriteLine("body:" + msgpart.GetBodyAsText());

                            bodytext = msgpart.GetBodyAsText();
                        }

                    }
                    else
                    {

                        bodytext = msg.MessagePart.GetBodyAsText();
                    }



                    string txtbodytext = StripHTML(bodytext);
                    txtbodytext = txtbodytext.Replace("\r", "<br>");
                    Console.WriteLine("body:" + txtbodytext);
                    //   txtbodytext = txtbodytext.Replace("\r", "<br>");

                    if (LogToDB(msg, txtbodytext) || from == "ufjl0683@cute.edu.tw" || from == "sshmc@cute.edu.tw")
                    {


                        if (from == "ufjl0683@cute.edu.tw" || from == "sshmc@cute.edu.tw" || msg.Headers.Subject.Contains("颱風"))
                        {

#if DEBUG
                            txtbodytext = "<img src=\"http://192.192.161.4/sshmc/logo.png\"><p>致 貴客戶，依中央氣象局發布資料，提醒您以下防災預警資訊:<br>" + txtbodytext;
                            SendMailToAllUser(msg.Headers.Subject.Replace("\n", ""), txtbodytext);
#else
                         SendMailToAllUser(msg.Headers.Subject.Replace("\n", ""), bodytext);
#endif
                        }
                        else
                        {
                            txtbodytext = "<img src=\"http://192.192.161.4/sshmc/logo.png\"><p>致 貴客戶，依中央氣象局發布資料，提醒您以下防災預警資訊:<br>" + txtbodytext;
                            SendMailToAllUser(msg.Headers.Subject.Replace("\n", ""), txtbodytext);
                        }
                    }



                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
#if !DEBUG
            for (int i = 1; i <= cnt; i++)
                mclient.DeleteMessage(i);
#endif

            mclient.Dispose();
        //    Console.ReadKey();
        }

        static bool LogToDB(Message msg,string bodytext)
        {
            string _class;

            if (bodytext.Length > 1000)
                bodytext = bodytext.Substring(0, 1000);
            //msg.Headers.Subject.Contains("大雨") || msg.Headers.Subject.Contains("豪雨") || msg.Headers.Subject.Contains("豪大雨") || msg.Headers.Subject.Contains("強風") || msg.Headers.Subject.Contains("地震")
            if (msg.Headers.Subject.Contains("大雨"))

                _class = "大雨";
            else if (msg.Headers.Subject.Contains("豪雨"))
                _class = "豪雨";
            else if (msg.Headers.Subject.Contains("豪大雨"))
                _class = "豪大雨";
            else if (msg.Headers.Subject.Contains("強風"))
                _class = "強風";
            else if (msg.Headers.Subject.Contains("地震"))
                _class = "地震";
            else if (msg.Headers.Subject.Contains("颱風"))
                _class = "颱風";
            else
                return false;

            WeatherMail.SSHMC01Entities db = new SSHMC01Entities();
            tblPre_disasterNotified rec = new tblPre_disasterNotified()
            {
                ISSEND = false,
                TITLE = msg.Headers.Subject,
                CONTENT = bodytext,
                PRE_ADMONISH_CLASS = _class,
                PUBLISH_ORANG = "中央氣象局",
                TIMESTAMP = DateTime.Now


            };

            db.tblPre_disasterNotified.AddObject(rec);
            db.SaveChanges();
            db.AcceptAllChanges();
            return true;
        }

        public static void SendMailToUser(string address, string subject, string bodytext)
        {
            System.Net.Mail.SmtpClient c = new SmtpClient("mail.cute.edu.tw", 25);
            c.DeliveryMethod = SmtpDeliveryMethod.Network;
            c.Credentials = new System.Net.NetworkCredential("weather", "0988163835");

            MailMessage m_mesg = new MailMessage(new MailAddress("weather@cute.edu.tw"), new MailAddress(address));
            m_mesg.Body = bodytext;
            m_mesg.Subject = subject;
            m_mesg.IsBodyHtml = true;
            c.Send(m_mesg);
           // c.Dispose();
        }

        public static void SendMailToUserFromSSHMC(string address, string subject, string bodytext)
        {
            System.Net.Mail.SmtpClient c = new SmtpClient("mail.cute.edu.tw", 25);
            c.DeliveryMethod = SmtpDeliveryMethod.Network;
            c.Credentials = new System.Net.NetworkCredential("sshmc", "S$hmc@464");

            MailMessage m_mesg = new MailMessage(new MailAddress("sshmc@cute.edu.tw"),new MailAddress(address));
            m_mesg.Body = bodytext;
            m_mesg.Subject = subject;
            m_mesg.IsBodyHtml = true;
            c.Send(m_mesg);
            // c.Dispose();
        }
       public static void SendMailToAllUser(  string subject, string bodytext )
        {
            System.Net.Mail.SmtpClient c = new SmtpClient("mail.cute.edu.tw", 25);
            c.DeliveryMethod = SmtpDeliveryMethod.Network;
            c.Credentials = new System.Net.NetworkCredential("weather", "0988163835");

            SSHMC01Entities db = new SSHMC01Entities();

            foreach (tblUser cust in db.tblUser.Where(n => n.USER_MAIL != null && n.USER_MAIL.Trim() != ""))
            {
#if DEBUG
                if(!cust.USER_MAIL.ToLower().Contains("ufjl0683"))
                   continue;
#endif

                MailMessage m_mesg = new MailMessage(new MailAddress("weather@cute.edu.tw"), new MailAddress(cust.USER_MAIL));
                m_mesg.Body = bodytext;
                m_mesg.Subject = subject;
                m_mesg.IsBodyHtml = true;
              //  m_mesg.IsBodyHtml = false;
                if (bodytext != "")
                    c.Send(m_mesg);
            }
        }

       private static string StripHTML(string source)
       {
           try
           {
               string result;

               // Remove HTML Development formatting
               // Replace line breaks with space
               // because browsers inserts space
               result = source.Replace("\r", " ");
               // Replace line breaks with space
               // because browsers inserts space
               result = result.Replace("\n", " ");
               // Remove step-formatting
               result = result.Replace("\t", string.Empty);
               // Remove repeating spaces because browsers ignore them
               result = System.Text.RegularExpressions.Regex.Replace(result,
                                                                     @"( )+", " ");

               // Remove the header (prepare first by clearing attributes)
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"<( )*head([^>])*>", "<head>",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"(<( )*(/)( )*head( )*>)", "</head>",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        "(<head>).*(</head>)", string.Empty,
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);

               // remove all scripts (prepare first by clearing attributes)
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"<( )*script([^>])*>", "<script>",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"(<( )*(/)( )*script( )*>)", "</script>",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               //result = System.Text.RegularExpressions.Regex.Replace(result,
               //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
               //         string.Empty,
               //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"(<script>).*(</script>)", string.Empty,
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);

               // remove all styles (prepare first by clearing attributes)
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"<( )*style([^>])*>", "<style>",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"(<( )*(/)( )*style( )*>)", "</style>",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        "(<style>).*(</style>)", string.Empty,
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);

               // insert tabs in spaces of <td> tags
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"<( )*td([^>])*>", "\t",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);

               // insert line breaks in places of <BR> and <LI> tags
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"<( )*br( )*>", "\r",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"<( )*li( )*>", "\r",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);

               // insert line paragraphs (double line breaks) in place
               // if <P>, <DIV> and <TR> tags
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"<( )*div([^>])*>", "\r\r",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"<( )*tr([^>])*>", "\r\r",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"<( )*p([^>])*>", "\r\r",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);

               // Remove remaining tags like <a>, links, images,
               // comments etc - anything that's enclosed inside < >
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"<[^>]*>", string.Empty,
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);

               // replace special characters:
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @" ", " ",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);

               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"&bull;", " * ",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"&lsaquo;", "<",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"&rsaquo;", ">",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"&trade;", "(tm)",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"&frasl;", "/",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"&lt;", "<",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"&gt;", ">",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"&copy;", "(c)",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"&reg;", "(r)",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               // Remove all others. More can be added, see
               // http://hotwired.lycos.com/webmonkey/reference/special_characters/
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        @"&(.{2,6});", string.Empty,
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);

               // for testing
               //System.Text.RegularExpressions.Regex.Replace(result,
               //       this.txtRegex.Text,string.Empty,
               //       System.Text.RegularExpressions.RegexOptions.IgnoreCase);

               // make line breaking consistent
               result = result.Replace("\n", "\r");

               // Remove extra line breaks and tabs:
               // replace over 2 breaks with 2 and over 4 tabs with 4.
               // Prepare first to remove any whitespaces in between
               // the escaped characters and remove redundant tabs in between line breaks
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        "(\r)( )+(\r)", "\r\r",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        "(\t)( )+(\t)", "\t\t",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        "(\t)( )+(\r)", "\t\r",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        "(\r)( )+(\t)", "\r\t",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               // Remove redundant tabs
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        "(\r)(\t)+(\r)", "\r\r",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               // Remove multiple tabs following a line break with just one tab
               result = System.Text.RegularExpressions.Regex.Replace(result,
                        "(\r)(\t)+", "\r\t",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
               // Initial replacement target string for line breaks
               string breaks = "\r\r\r";
               // Initial replacement target string for tabs
               string tabs = "\t\t\t\t\t";
               for (int index = 0; index < result.Length; index++)
               {
                   result = result.Replace(breaks, "\r\r");
                   result = result.Replace(tabs, "\t\t\t\t");
                   breaks = breaks + "\r";
                   tabs = tabs + "\t";
               }

               // That's it.
       //        result = System.Text.RegularExpressions.Regex.Replace(result, "[\u3000]+", " "); //result.Replace("\uA140", " ");

               return result;
           }
           catch
           {
              Console.WriteLine("Error");
               return source;
           }
       }



    }


}
