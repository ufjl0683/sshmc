
namespace MapApplication.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // MetadataTypeAttribute 會將 tblCCTVMetadata 識別為
    // 帶有 tblCCTV 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblCCTV.tblCCTVMetadata))]
    public partial class tblCCTV
    {
       
        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblCCTV 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblCCTVMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblCCTVMetadata()
            {
            }

            public string CCTV_ID { get; set; }

        

            public string SITE_ID { get; set; }

            public tblSite tblSite { get; set; }

            public Nullable<double> X { get; set; }

            public Nullable<double> Y { get; set; }
            public string DESCRIPTION { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblHOST_ConfigMetadata 識別為
    // 帶有 tblHOST_Config 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblHOST_Config.tblHOST_ConfigMetadata))]
    public partial class tblHOST_Config
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblHOST_Config 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblHOST_ConfigMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblHOST_ConfigMetadata()
            {
            }

            public string HOST_ID { get; set; }

            public string HOST_IP { get; set; }

            public string HOST_TYPE { get; set; }

            public string MEMO { get; set; }

            public EntityCollection<tblMFCC_Config> tblMFCC_Config { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblMFCC_ConfigMetadata 識別為
    // 帶有 tblMFCC_Config 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblMFCC_Config.tblMFCC_ConfigMetadata))]
    public partial class tblMFCC_Config
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblMFCC_Config 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblMFCC_ConfigMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblMFCC_ConfigMetadata()
            {
            }

            public string HOST_ID { get; set; }

            public string MEMO { get; set; }

            public string MFCC_ID { get; set; }

            public string MFCC_NAME { get; set; }

            public string MFCC_TYPE { get; set; }

            public int REMOTE_PORT { get; set; }

            public tblHOST_Config tblHOST_Config { get; set; }

            public EntityCollection<tblTC> tblTC { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblRulesMetadata 識別為
    // 帶有 tblRules 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblRules.tblRulesMetadata))]
    public partial class tblRules
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblRules 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblRulesMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblRulesMetadata()
            {
            }

            public byte DEGREE { get; set; }

            public Nullable<double> HOUR_MA { get; set; }

            public Nullable<double> LEFT_HOUR_MA1 { get; set; }

            public Nullable<double> LEFT_HOUR_MA2 { get; set; }

            public Nullable<double> LOWER_LIMIT { get; set; }

            public Nullable<double> RGHT_HOUR_MA2 { get; set; }

            public Nullable<double> RIGHT_HOUR_MA1 { get; set; }

            public int SENSOR_ID { get; set; }

            public tblSensor_Values tblSensor_Values { get; set; }

            public Nullable<double> UPPER_LIMIT { get; set; }

            public int VALUE_ID { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblSensorMetadata 識別為
    // 帶有 tblSensor 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblSensor.tblSensorMetadata))]
    public partial class tblSensor
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblSensor 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblSensorMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblSensorMetadata()
            {
            }

            public string COM_TYPE { get; set; }

            public Nullable<int> CONTROLLER_ID { get; set; }

            public Nullable<byte> CURRENT_DEGREE { get; set; }

            public Nullable<bool> EXCUTION_MODE { get; set; }

            public Nullable<int> ID { get; set; }

            public string IP__COMPORT { get; set; }

            public string ISCONNECTED { get; set; }

            public Nullable<int> PORT_BAUD { get; set; }

            public Nullable<int> REFGPS_ID { get; set; }

            public int SENSOR_ID { get; set; }

            public string SENSOR_NAME { get; set; }

            public string SENSOR_TYPE { get; set; }

            public EntityCollection<tblSensor_Values> tblSensor_Values { get; set; }

            public tblTC tblTC { get; set; }

            public EntityCollection<tblTC10MinDataLog> tblTC10MinDataLog { get; set; }

            public Nullable<double> X { get; set; }

            public Nullable<double> Y { get; set; }

            public string ISVALID { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblSensor_ValuesMetadata 識別為
    // 帶有 tblSensor_Values 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblSensor_Values.tblSensor_ValuesMetadata))]
    public partial class tblSensor_Values
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblSensor_Values 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblSensor_ValuesMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblSensor_ValuesMetadata()
            {
            }

            public double COEFFICIENT { get; set; }

            public string DESC { get; set; }

            public Nullable<double> OFFSET { get; set; }

            public int SENSOR_ID { get; set; }

            public EntityCollection<tblRules> tblRules { get; set; }

            public tblSensor tblSensor { get; set; }

            public int VALUE_ID { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblSiteMetadata 識別為
    // 帶有 tblSite 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblSite.tblSiteMetadata))]
    public partial class tblSite
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblSite 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblSiteMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblSiteMetadata()
            {
            }

            public string ENVIRONMENT { get; set; }

            public Nullable<double> MAX_X { get; set; }

            public Nullable<double> MAX_Y { get; set; }

            public Nullable<double> MIN_X { get; set; }

            public Nullable<double> MIN_Y { get; set; }

            public string SITE_ADDRESS { get; set; }

            public string SITE_ID { get; set; }

            public string SITE_NAME { get; set; }

            
            public EntityCollection<tblCCTV> tblCCTV { get; set; }
            
            public EntityCollection<tblTC> tblTC { get; set; }

            public Nullable<double> X { get; set; }

            public Nullable<double> Y { get; set; }

            public bool ISBIM { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblTCMetadata 識別為
    // 帶有 tblTC 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblTC.tblTCMetadata))]
    public partial class tblTC
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblTC 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblTCMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblTCMetadata()
            {
            }

            public Nullable<DateTime> BUILD_DATE { get; set; }

            public int CONTROLLER_ID { get; set; }

            public string DEVICE_TYPE { get; set; }

            public string ENABLE { get; set; }

            public byte HW_STATUS_1 { get; set; }

            public byte HW_STATUS_2 { get; set; }

            public byte HW_STATUS_3 { get; set; }

            public byte HW_STATUS_4 { get; set; }

            public string IP { get; set; }

            public string ISCONNECTED { get; set; }

            public string MFCC_ID { get; set; }

            public int PORT { get; set; }

            public string SITE_ID { get; set; }

            public tblMFCC_Config tblMFCC_Config { get; set; }
           
            public EntityCollection<tblSensor> tblSensor { get; set; }

            public tblSite tblSite { get; set; }

            public EntityCollection<tblTC_Restore> tblTC_Restore { get; set; }

            public string VERSION_NO { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblTC_BoxMetadata 識別為
    // 帶有 tblTC_Box 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblTC_Box.tblTC_BoxMetadata))]
    public partial class tblTC_Box
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblTC_Box 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblTC_BoxMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblTC_BoxMetadata()
            {
            }

            public string TC_BOX_ID { get; set; }

            public Nullable<double> X { get; set; }

            public Nullable<double> Y { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblTC_RestoreMetadata 識別為
    // 帶有 tblTC_Restore 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblTC_Restore.tblTC_RestoreMetadata))]
    public partial class tblTC_Restore
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblTC_Restore 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblTC_RestoreMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblTC_RestoreMetadata()
            {
            }

            public int CONTROLLER_ID { get; set; }

            public string ISFINSH { get; set; }

            public tblTC tblTC { get; set; }

            public DateTime TIMESTAMP { get; set; }

            public Nullable<int> TRYCNT { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblTC10MinDataLogMetadata 識別為
    // 帶有 tblTC10MinDataLog 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblTC10MinDataLog.tblTC10MinDataLogMetadata))]
    public partial class tblTC10MinDataLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblTC10MinDataLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblTC10MinDataLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblTC10MinDataLogMetadata()
            {
            }

            public Nullable<int> CONTROLLER_ID { get; set; }

            public byte DEGREE { get; set; }

            public Nullable<bool> EXECUTION_MODE { get; set; }

            public string ISVALID { get; set; }

            public int SENSOR_ID { get; set; }

            public tblSensor tblSensor { get; set; }

            public DateTime TIMESTAMP { get; set; }

            public Nullable<byte> VALUE_CNT { get; set; }

            public Nullable<double> VALUE0 { get; set; }

            public Nullable<double> VALUE1 { get; set; }

            public Nullable<double> VALUE2 { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 VWHOSTMFCCMetadata 識別為
    // 帶有 VWHOSTMFCC 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(VWHOSTMFCC.VWHOSTMFCCMetadata))]
    public partial class VWHOSTMFCC
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 VWHOSTMFCC 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class VWHOSTMFCCMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private VWHOSTMFCCMetadata()
            {
            }

            public string HOSTID { get; set; }

            public string HOSTIP { get; set; }

            public string HOSTTYPE { get; set; }

            public string MEMO { get; set; }

            public string MFCCID { get; set; }

            public string MFCCTYPE { get; set; }

            public int REMOTEPORT { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwSiteDegreeMetadata 識別為
    // 帶有 vwSiteDegree 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwSiteDegree.vwSiteDegreeMetadata))]
    public partial class vwSiteDegree
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwSiteDegree 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwSiteDegreeMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwSiteDegreeMetadata()
            {
            }

            public Nullable<byte> CURRENT_DEGREE { get; set; }

            public string ENVIRONMENT { get; set; }

            public Nullable<double> MAX_X { get; set; }

            public Nullable<double> MAX_Y { get; set; }

            public Nullable<double> MIN_X { get; set; }

            public Nullable<double> MIN_Y { get; set; }

            public string SITE_ADDRESS { get; set; }

            public string SITE_ID { get; set; }

            public string SITE_NAME { get; set; }

            public Nullable<double> X { get; set; }

            public Nullable<double> Y { get; set; }

            public bool ISBIM { get; set; }
        }

        [MetadataTypeAttribute(typeof(vwSensorDegree.vwSensorDegreeMetadata))]
        public partial class vwSensorDegree
        {

            // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwSensorDegree 類別
            // 的 properties。
            //
            // 例如，下列程式碼將 Xyz 屬性標記為
            // 必要的屬性，並指定有效值的格式:
            //    [Required]
            //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
            //    [StringLength(32)]
            //    public string Xyz { get; set; }
            internal sealed class vwSensorDegreeMetadata
            {

                // 中繼資料類別本就不應該具現化。
                private vwSensorDegreeMetadata()
                {
                }

                public Nullable<byte> CURRENT_DEGREE { get; set; }

                public Nullable<bool> EXCUTION_MODE { get; set; }

                public Nullable<int> ID { get; set; }

                public string ISCONNECTED { get; set; }

                public int SENSOR_ID { get; set; }

                public string SENSOR_NAME { get; set; }

                public string SITE_ID { get; set; }

                public Nullable<double> X { get; set; }

                public Nullable<double> Y { get; set; }
                public string SENSOR_TYPE { get; set; }
                public Nullable<double> VALUE0 { get; set; }
                public Nullable<double> VALUE1 { get; set; }
                public Nullable<double> VALUE2 { get; set; }
                public string ISVALID { get; set; }
            }
        }
        [MetadataTypeAttribute(typeof(vwSensorValuesAndTC10MinDataLog.vwSensorValuesAndTC10MinDataLogMetadata))]
        public partial class vwSensorValuesAndTC10MinDataLog
        {

            // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwSensorValuesAndTC10MinDataLog 類別
            // 的 properties。
            //
            // 例如，下列程式碼將 Xyz 屬性標記為
            // 必要的屬性，並指定有效值的格式:
            //    [Required]
            //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
            //    [StringLength(32)]
            //    public string Xyz { get; set; }
            internal sealed class vwSensorValuesAndTC10MinDataLogMetadata
            {

                // 中繼資料類別本就不應該具現化。
                private vwSensorValuesAndTC10MinDataLogMetadata()
                {
                }

                public Nullable<int> CONTROLLER_ID { get; set; }

                public byte DEGREE { get; set; }

                public Nullable<bool> EXECUTION_MODE { get; set; }

                public double initmean0 { get; set; }

                public double initmean1 { get; set; }

                public double initmean2 { get; set; }

                public string IS_REPAIR { get; set; }

                public string ISVALID { get; set; }

                public int SENSOR_ID { get; set; }

                public double sigma1 { get; set; }

                public double sigma2 { get; set; }

                public double signama0 { get; set; }

                public DateTime TIMESTAMP { get; set; }

                public Nullable<byte> VALUE_CNT { get; set; }

                public Nullable<double> VALUE0 { get; set; }

                public Nullable<double> VALUE1 { get; set; }

                public Nullable<double> VALUE2 { get; set; }
            }
        }
    }
}
