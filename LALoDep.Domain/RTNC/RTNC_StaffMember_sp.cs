namespace LALoDep.Domain.RTNC
{

    public class RTNC_StaffMember_spParams {
  public int? WorkID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class RTNC_StaffMember_spResult
	{
		public RTNC_StaffMember_spResult()
		{
		}
		public string PersonDisplay	{ get; set; }
		public int? PersonID	{ get; set; }
		public int? Selected	{ get; set; }
        public int? IsAttorneyFlag { get; set; }

        public int? IsSupervisorFlag { get; set; }

    }
}
