using System;

namespace LALoDep.Core.NG_sp.com_Report
{
    public class NG_com_ReportParameterDefinitionGetByReportID_spResult
    {
        public int? ReportParameterDefinitionDisplayOrder { get; set; }
        public int Sequence { get; set; }
        public string Type { get; set; }
        public int? Mask { get; set; }
        public string ControlName { get; set; }
        public string Caption { get; set; }
        public string DefaultValue { get; set; }
        public int? CodeType { get; set; }
        public short? Required { get; set; }
        public string RequiredIfEntered { get; set; }
        public string RequiredIfEnteredType { get; set; }
        public string RequiredIfNotEntered { get; set; }
        public string RequiredIfNotEnteredType { get; set; }
        public string Comparison { get; set; }
        public string ComparisonControlName { get; set; }
        public string ComparisonControlCaption { get; set; }
        public string StartRange { get; set; }
        public string EndRange { get; set; }
        public string ReportName { get; set; }
        public string DocumentType { get; set; }
        public string OtherValue { get; set; }
        public int? SystemValueTypeID { get; set; }
        public string SPName { get; set; }
        public string SPParms { get; set; }
        public string SPIDFieldName { get; set; }
        public string SPValueFieldName { get; set; }
    }
}
