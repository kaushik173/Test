namespace LALoDep.Domain.pd_Code
{
    public class pd_CodeGetByTypeIDForAdministration_spParams
    {
        public int? CodeTypeID { get; set; }
        public string LoadOption { get; set; }
        public int? UserID { get; set; }

    }


    public class pd_CodeGetByTypeIDForAdministration_spResult
    {

        public int? CodeID { get; set; }
        public int? CodeTypeID { get; set; }
        public string CodeValue { get; set; }
        public string CodeShortValue { get; set; }
        public string CodeEnumName { get; set; }
        public string CodeMobileValue { get; set; }
        public string SortBy { get; set; }
    }
}
