using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;

namespace WeatherMail
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的介面名稱 "IService1"。
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [WebGet(UriTemplate="SendMailToAll/{subject}/{bodytext}")]
        string SendMailToAll(string subject,string bodytext);
         [OperationContract]
        [WebGet]
        string SendMailToUser(string address,string subject,string bodytext);
    }
}
