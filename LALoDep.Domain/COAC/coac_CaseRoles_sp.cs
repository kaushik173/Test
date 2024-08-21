namespace Jcats.SD.Domain.COAC
{
    public class coac_CaseRoles_spParams
    {
        public int? CaseID { get; set; }
        public int? AgencyID { get; set; }
        public int? UserID { get; set; }

    }


    public class coac_CaseRoles_spResult
    {
        public coac_CaseRoles_spResult()
        {
        }
        public byte? ClientFlag { get; set; }
        public int? ChildFlag { get; set; }
        public string PersonDisplay { get; set; }
        public int? PersonID { get; set; }
        public string CaseInfo { get; set; }
        public string DOBAge { get; set; }
        public string RepresentingAtty { get; set; }
    }
}
