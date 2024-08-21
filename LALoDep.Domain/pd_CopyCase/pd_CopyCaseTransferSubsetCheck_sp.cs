
namespace LALoDep.Domain.pd_CopyCase
{
    public class pd_CopyCaseTransferSubsetCheck_spParams {
  public int? CaseID { get; set; }
  public int? AgencyID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class pd_CopyCaseTransferSubsetCheck_spResult
	{
		public pd_CopyCaseTransferSubsetCheck_spResult()
		{
		}
		public int? AllowTranfer	{ get; set; }
	}
}
