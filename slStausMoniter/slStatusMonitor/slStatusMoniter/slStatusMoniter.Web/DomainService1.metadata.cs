
namespace slStatusMoniter.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // MetadataTypeAttribute 會將 vwSensorStatusMetadata 識別為
    // 帶有 vwSensorStatus 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwSensorStatus.vwSensorStatusMetadata))]
    public partial class vwSensorStatus
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwSensorStatus 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwSensorStatusMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwSensorStatusMetadata()
            {
            }

            public string CX { get; set; }

            public string CY { get; set; }

            public int LEVEL { get; set; }

            public string PICTYPE { get; set; }

            public string PNO { get; set; }
        }
    }
}
