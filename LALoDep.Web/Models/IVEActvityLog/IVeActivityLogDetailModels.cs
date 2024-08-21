using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models.IVEActvityLog
{
    public class IVeActivityLogDetailModel
    {
        public int? ActivityLogDetailID { get; set; }
        public int? ActivityLogID { get; set; }
        public string RowCodeID { get; set; }
        public int? SortOrder { get; set; }
        public decimal? Col1 { get; set; }
        public decimal? Col2 { get; set; }
        public decimal? Col3 { get; set; }
        public decimal? Col4 { get; set; }
        public decimal? Col5 { get; set; }
        public decimal? Col6 { get; set; }
        public decimal? Col7 { get; set; }
        public decimal? Col8 { get; set; }
        public decimal? Col9 { get; set; }
        public decimal? Col10 { get; set; }
        public decimal? Col11 { get; set; }
        public decimal? Col12 { get; set; }
        public decimal? Col13 { get; set; }
        public decimal? Col14 { get; set; }
        public decimal? Col15 { get; set; }
        public decimal? Col16 { get; set; }
        public decimal? Col17 { get; set; }
        public decimal? Col18 { get; set; }
        public decimal? Col19 { get; set; }
        public decimal? Col20 { get; set; }
        public decimal? Col21 { get; set; }
        public decimal? Col22 { get; set; }
        public decimal? Col23 { get; set; }
        public decimal? Col24 { get; set; }
        public decimal? Col25 { get; set; }
        public decimal? Col26 { get; set; }
        public decimal? Col27 { get; set; }
        public decimal? Col28 { get; set; }
        public decimal? Col29 { get; set; }
        public decimal? Col30 { get; set; }
        public decimal? Col31 { get; set; }
        public int? InsertedByUserID { get; set; }
        public System.DateTime? InsertedOnDateTime { get; set; }
        public int? RecordStateID { get; set; }

    }
    public class IVeActivityLogDetail: IVeActivityLogDetailModel
    {        
        
        public System.Collections.Generic.List<CodeModel> Code { get; set; }
    }
    public class CodeModel
    {
        public string CodeValue { get; set; }
    }
}