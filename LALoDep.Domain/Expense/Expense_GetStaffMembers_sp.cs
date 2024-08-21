namespace LALoDep.Domain.Expense
{
public class Expense_GetStaffMembers_spParams {
  public int? ExpenseID { get; set; }
  public int? AgencyID { get; set; }
  public int? CaseID { get; set; }
  public int? UserID { get; set; }

}

	
	public class Expense_GetStaffMembers_spResult
	{
		public Expense_GetStaffMembers_spResult()
		{
		}
		public string PersonDisplay	{ get; set; }
		public int? PersonID	{ get; set; }
		public int? Selected	{ get; set; }
	}
}
