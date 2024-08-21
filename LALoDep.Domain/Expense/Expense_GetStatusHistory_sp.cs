namespace LALoDep.Domain.Expense
{
    public class Expense_GetStatusHistory_spParams
    {
        public int? ExpenseID { get; set; }
        public int? UserID { get; set; }

    }


    public class Expense_GetStatusHistory_spResult
    {
        public Expense_GetStatusHistory_spResult()
        {
        }
        public string StatusDate { get; set; }
        public string StatusDisplay { get; set; }
    }
}
