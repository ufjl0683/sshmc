
namespace MapApplication.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using System.Web;


    // 使用 SSHMC01Entities 內容實作應用程式邏輯。
    // TODO: 將應用程式邏輯加入至這些方法或其他方法。
    // TODO: 連接驗證 (Windows/ASP.NET Forms) 並取消下面的註解，以停用匿名存取
    // 視需要也考慮加入要限制存取的角色。
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public class DbService : LinqToEntitiesDomainService<SSHMC01Entities>
    {

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblCCTV' 查詢。
        public IQueryable<tblCCTV> GetTblCCTV()
        {
            return this.ObjectContext.tblCCTV;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblHOST_Config' 查詢。
        public IQueryable<tblHOST_Config> GetTblHOST_Config()
        {
            return this.ObjectContext.tblHOST_Config;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblMFCC_Config' 查詢。
        public IQueryable<tblMFCC_Config> GetTblMFCC_Config()
        {
            return this.ObjectContext.tblMFCC_Config;
        }

        public void InsertTblMFCC_Config(tblMFCC_Config tblMFCC_Config)
        {
            if ((tblMFCC_Config.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblMFCC_Config, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblMFCC_Config.AddObject(tblMFCC_Config);
            }
        }

        public void UpdateTblMFCC_Config(tblMFCC_Config currenttblMFCC_Config)
        {
            this.ObjectContext.tblMFCC_Config.AttachAsModified(currenttblMFCC_Config, this.ChangeSet.GetOriginal(currenttblMFCC_Config));
        }

        public void DeleteTblMFCC_Config(tblMFCC_Config tblMFCC_Config)
        {
            if ((tblMFCC_Config.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblMFCC_Config, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblMFCC_Config.Attach(tblMFCC_Config);
                this.ObjectContext.tblMFCC_Config.DeleteObject(tblMFCC_Config);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblRules' 查詢。
        public IQueryable<tblRules> GetTblRules()
        {
            return this.ObjectContext.tblRules;
        }

        public void InsertTblRules(tblRules tblRules)
        {
            if ((tblRules.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblRules, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblRules.AddObject(tblRules);
            }
        }

        public void UpdateTblRules(tblRules currenttblRules)
        {
            this.ObjectContext.tblRules.AttachAsModified(currenttblRules, this.ChangeSet.GetOriginal(currenttblRules));
        }

        public void DeleteTblRules(tblRules tblRules)
        {
            if ((tblRules.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblRules, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblRules.Attach(tblRules);
                this.ObjectContext.tblRules.DeleteObject(tblRules);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblSensor' 查詢。
        public IQueryable<tblSensor> GetTblSensor()
        {
            return this.ObjectContext.tblSensor;
        }

        public void InsertTblSensor(tblSensor tblSensor)
        {
            if ((tblSensor.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSensor, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblSensor.AddObject(tblSensor);
            }
        }

        public void UpdateTblSensor(tblSensor currenttblSensor)
        {
            this.ObjectContext.tblSensor.AttachAsModified(currenttblSensor, this.ChangeSet.GetOriginal(currenttblSensor));
        }

        public void DeleteTblSensor(tblSensor tblSensor)
        {
            if ((tblSensor.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSensor, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblSensor.Attach(tblSensor);
                this.ObjectContext.tblSensor.DeleteObject(tblSensor);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblSensor_Values' 查詢。
        public IQueryable<tblSensor_Values> GetTblSensor_Values()
        {
            return this.ObjectContext.tblSensor_Values;
        }

        public void InsertTblSensor_Values(tblSensor_Values tblSensor_Values)
        {
            if ((tblSensor_Values.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSensor_Values, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblSensor_Values.AddObject(tblSensor_Values);
            }
        }

        public void UpdateTblSensor_Values(tblSensor_Values currenttblSensor_Values)
        {
            this.ObjectContext.tblSensor_Values.AttachAsModified(currenttblSensor_Values, this.ChangeSet.GetOriginal(currenttblSensor_Values));
        }

        public void DeleteTblSensor_Values(tblSensor_Values tblSensor_Values)
        {
            if ((tblSensor_Values.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSensor_Values, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblSensor_Values.Attach(tblSensor_Values);
                this.ObjectContext.tblSensor_Values.DeleteObject(tblSensor_Values);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblSite' 查詢。
        public IQueryable<tblSite> GetTblSite()
        {
            return this.ObjectContext.tblSite;
        }

        public void InsertTblSite(tblSite tblSite)
        {
            if ((tblSite.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSite, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblSite.AddObject(tblSite);
            }
        }

        public void UpdateTblSite(tblSite currenttblSite)
        {
            this.ObjectContext.tblSite.AttachAsModified(currenttblSite, this.ChangeSet.GetOriginal(currenttblSite));
        }

        public void DeleteTblSite(tblSite tblSite)
        {
            if ((tblSite.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSite, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblSite.Attach(tblSite);
                this.ObjectContext.tblSite.DeleteObject(tblSite);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblTC' 查詢。
        public IQueryable<tblTC> GetTblTC()
        {
            return this.ObjectContext.tblTC;
        }

        public void InsertTblTC(tblTC tblTC)
        {
            if ((tblTC.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblTC, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblTC.AddObject(tblTC);
            }
        }

        public void UpdateTblTC(tblTC currenttblTC)
        {
            this.ObjectContext.tblTC.AttachAsModified(currenttblTC, this.ChangeSet.GetOriginal(currenttblTC));
        }

        public void DeleteTblTC(tblTC tblTC)
        {
            if ((tblTC.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblTC, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblTC.Attach(tblTC);
                this.ObjectContext.tblTC.DeleteObject(tblTC);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblTC_Box' 查詢。
        public IQueryable<tblTC_Box> GetTblTC_Box()
        {
            return this.ObjectContext.tblTC_Box;
        }

        public void InsertTblTC_Box(tblTC_Box tblTC_Box)
        {
            if ((tblTC_Box.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblTC_Box, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblTC_Box.AddObject(tblTC_Box);
            }
        }

        public void UpdateTblTC_Box(tblTC_Box currenttblTC_Box)
        {
            this.ObjectContext.tblTC_Box.AttachAsModified(currenttblTC_Box, this.ChangeSet.GetOriginal(currenttblTC_Box));
        }

        public void DeleteTblTC_Box(tblTC_Box tblTC_Box)
        {
            if ((tblTC_Box.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblTC_Box, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblTC_Box.Attach(tblTC_Box);
                this.ObjectContext.tblTC_Box.DeleteObject(tblTC_Box);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblTC_Restore' 查詢。
        public IQueryable<tblTC_Restore> GetTblTC_Restore()
        {
            return this.ObjectContext.tblTC_Restore;
        }

        public void InsertTblTC_Restore(tblTC_Restore tblTC_Restore)
        {
            if ((tblTC_Restore.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblTC_Restore, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblTC_Restore.AddObject(tblTC_Restore);
            }
        }

        public void UpdateTblTC_Restore(tblTC_Restore currenttblTC_Restore)
        {
            this.ObjectContext.tblTC_Restore.AttachAsModified(currenttblTC_Restore, this.ChangeSet.GetOriginal(currenttblTC_Restore));
        }

        public void DeleteTblTC_Restore(tblTC_Restore tblTC_Restore)
        {
            if ((tblTC_Restore.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblTC_Restore, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblTC_Restore.Attach(tblTC_Restore);
                this.ObjectContext.tblTC_Restore.DeleteObject(tblTC_Restore);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblTC10MinDataLog' 查詢。
        public IQueryable<tblTC10MinDataLog> GetTblTC10MinDataLog()
        {
            return this.ObjectContext.tblTC10MinDataLog;
        }

        public void InsertTblTC10MinDataLog(tblTC10MinDataLog tblTC10MinDataLog)
        {
            if ((tblTC10MinDataLog.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblTC10MinDataLog, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblTC10MinDataLog.AddObject(tblTC10MinDataLog);
            }
        }

        public void UpdateTblTC10MinDataLog(tblTC10MinDataLog currenttblTC10MinDataLog)
        {
            this.ObjectContext.tblTC10MinDataLog.AttachAsModified(currenttblTC10MinDataLog, this.ChangeSet.GetOriginal(currenttblTC10MinDataLog));
        }

        public void DeleteTblTC10MinDataLog(tblTC10MinDataLog tblTC10MinDataLog)
        {
            if ((tblTC10MinDataLog.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblTC10MinDataLog, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblTC10MinDataLog.Attach(tblTC10MinDataLog);
                this.ObjectContext.tblTC10MinDataLog.DeleteObject(tblTC10MinDataLog);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'VWHOSTMFCC' 查詢。
      

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'vwSiteDegree' 查詢。
        public IQueryable<vwSiteDegree> GetVwSiteDegree()
        {
            return this.ObjectContext.vwSiteDegree;
        }

        public void InsertVwSiteDegree(vwSiteDegree vwSiteDegree)
        {
            if ((vwSiteDegree.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(vwSiteDegree, EntityState.Added);
            }
            else
            {
                this.ObjectContext.vwSiteDegree.AddObject(vwSiteDegree);
            }
        }

        public void UpdateVwSiteDegree(vwSiteDegree currentvwSiteDegree)
        {
            this.ObjectContext.vwSiteDegree.AttachAsModified(currentvwSiteDegree, this.ChangeSet.GetOriginal(currentvwSiteDegree));
        }

        public void DeleteVwSiteDegree(vwSiteDegree vwSiteDegree)
        {
            if ((vwSiteDegree.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(vwSiteDegree, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.vwSiteDegree.Attach(vwSiteDegree);
                this.ObjectContext.vwSiteDegree.DeleteObject(vwSiteDegree);
            }
        }
        public IQueryable<vwSensorDegree> GetVwSensorDegree()
        {
            return this.ObjectContext.vwSensorDegree;
        }
        public void InsertVwSensorDegreee(vwSensorDegree vwSensorDegree)
        {
            if ((vwSensorDegree.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(vwSensorDegree, EntityState.Added);
            }
            else
            {
                this.ObjectContext.vwSensorDegree.AddObject(vwSensorDegree);
            }
        }

        public void UpdateVwSensorDegree(vwSensorDegree currentvwSensorDegree)
        {
            this.ObjectContext.vwSensorDegree.AttachAsModified(currentvwSensorDegree, this.ChangeSet.GetOriginal(currentvwSensorDegree));
        }

        public void DeleteVwSensorDegree(vwSensorDegree vwSensorDegree)
        {
            if ((vwSensorDegree.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(vwSensorDegree, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.vwSensorDegree.Attach(vwSensorDegree);
                this.ObjectContext.vwSensorDegree.DeleteObject(vwSensorDegree);
            }
        }

        public IQueryable<vwSensorValuesAndTC10MinDataLog> GetVwSensorValuesAndTC10MinDataLog()
        {
            return this.ObjectContext.vwSensorValuesAndTC10MinDataLog;
        }

      
        

        [Invoke]
        public bool IsUserLogin(string CustomerID)
        {
            return HttpContext.Current.Session["CustomerID"] != null  && HttpContext.Current.Session["CustomerID"].ToString()==CustomerID  ;
        }

        public IQueryable<tblSensorTypeGroup> GetTblSensorTypeGroup()
        {
            return this.ObjectContext.tblSensorTypeGroup;
        }
    }
}


