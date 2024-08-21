namespace LALoDep.Domain.Expense
{
public class Expense_GetByCaseID_spParams {
  public int? CaseID { get; set; }
  public int? UserID { get; set; }

}

	
	public class Expense_GetByCaseID_spResult
	{
		public Expense_GetByCaseID_spResult()
		{
		}
		public int? ExpenseID	{ get; set; }
		public string ExpenseDate	{ get; set; }
		public string ExpenseType	{ get; set; }
		public string StaffMember	{ get; set; }
		public decimal? Amount	{ get; set; }
		public decimal? PreviousAmount	{ get; set; }
		public string CurrentStatus	{ get; set; }
		public string AttachmentDisplay	{ get; set; }
		public int? AttachmentCount	{ get; set; }
		public int? CanDeleteFlag	{ get; set; }
		public string SortDate	{ get; set; }
	}
}
