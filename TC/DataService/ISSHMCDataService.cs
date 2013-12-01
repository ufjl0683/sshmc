using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DataService
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的介面名稱 "ISSJMCDataService"。
    [ServiceContract]
    public interface ISSHMCDataService
    {
        [OperationContract]
        bool CheckUserIDPassword(string id, string Password);
        [OperationContract]
        DataService.vwSiteDegree[] GetSiteInfo(string id);
        [OperationContract]
        DataService.vwSensorDegree[] GetSensorInfo(string SiteID);
        [OperationContract]
        DataService.tblCCTV[] GetCCTVInfo(string SiteID);
        [OperationContract]
        DataService.tblPre_disasterNotified[] GetDisasterInfo();
        [OperationContract]
        DataService.tblSurvey_Disaster[] GetSurveyDisaster();
        [OperationContract]
        string AddSurverDiasterInfo(DataService.tblSurvey_Disaster info);
        [OperationContract]
        DataService.vwReportNotified[] GettblReportNotified(string userid);
        [OperationContract]
        DataService.vwSensorValuesAndTC10MinDataLog[] GetvwSensorValuesAndTC10MinDataLog(int snrid,DateTime StartDate,DateTime EndDate);

    }
}
