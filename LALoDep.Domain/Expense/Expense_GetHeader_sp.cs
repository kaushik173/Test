namespace LALoDep.Domain.Expense
{
public class Expense_GetHeader_spParams {
  public int? ExpenseID { get; set; }
  public int? UserID { get; set; }

}

	
	public class Expense_GetHeader_spResult
	{
		public Expense_GetHeader_spResult()
		{
		}
		public string ExpenseHeader	{ get; set; }
		public string AttachedFiles	{ get; set; }

        public int ExpenseID { get; set; }
    }
}
