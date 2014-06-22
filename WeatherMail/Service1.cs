using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WeatherMail
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的類別名稱 "Service1"。
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerCall)]
    public class Service1 : IService1
    {
        public void DoWork()
        {
        }

        public string SendMailToAll(string subject, string bodytext)
        {
            try
            {
                Program.SendMailToAllUser(subject, bodytext);
                return "ok";
                    
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            //throw new NotImplementedException();
        }

        public string SendMailToUser(string mailaddress, string subject, string bodytext)
        {
            try
            {
                Program.SendMailToUserFromSSHMC(mailaddress,subject, bodytext);
                return "ok";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            //throw new NotImplementedException();
        }

     
    }
}
