
namespace LALoDep.Domain.TitleIVe
{
    public class TitleIVeCodeDropDown_spParams
    {
        public int? UserID { get; set; }
        public int? CodeTypeID { get; set; }

    }


    public class TitleIVeCodeDropDown_spResult
    {
        public TitleIVeCodeDropDown_spResult()
        {
        }
        public int? CodeID { get; set; }
        public string CodeValue { get; set; }
        public string CodeShortValue { get; set; }
        public int? CodeTypeID { get; set; }
        public string CodeEnumName { get; set; }
    }
}
