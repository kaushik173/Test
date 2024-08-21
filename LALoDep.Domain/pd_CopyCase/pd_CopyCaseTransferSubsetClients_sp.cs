
namespace LALoDep.Domain.pd_CopyCase
{
    public class pd_CopyCaseTransferSubsetClients_spParams
    {
        public int? CaseID { get; set; }
        public int? AgencyID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class pd_CopyCaseTransferSubsetClients_spResult
    {
       
        public int? PersonID { get; set; }
        public string PersonDisplay { get; set; }
    }
}
