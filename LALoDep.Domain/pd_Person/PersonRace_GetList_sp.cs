namespace LALoDep.Domain.pd_Person
{
public class PersonRace_GetList_spParams {
  public int? CaseAgencyID { get; set; }
  public int? CaseID { get; set; }
  public int? PersonID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class PersonRace_GetList_spResult
	{
		public PersonRace_GetList_spResult()
		{
		}
		public int? PersonRaceID	{ get; set; }
		public int? PersonID	{ get; set; }
		public int? PersonRaceCodeID	{ get; set; }
		public string PersonRaceDisplay	{ get; set; }
		public string CodeEnumName { get; set; }
		public int? IsOther { get; set; }
        public string PersonRaceVerbalValue { get; set; }
    }
}
