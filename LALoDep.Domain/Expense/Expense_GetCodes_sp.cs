namespace LALoDep.Domain.Expense
{
public class Expense_GetCodes_spParams {
  public int? CodeTypeCodeID { get; set; }
  public int? ExpenseID { get; set; }
  public int? AgencyID { get; set; }
  public int? CaseID { get; set; }
  public int? UserID { get; set; }

}

	
	public class Expense_GetCodes_spResult
	{
		public Expense_GetCodes_spResult()
		{
		}
		public string CodeDisplay	{ get; set; }
		public int? CodeID	{ get; set; }
		public int? Selected	{ get; set; }
	}
}
