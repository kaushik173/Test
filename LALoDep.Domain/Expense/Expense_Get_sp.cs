namespace LALoDep.Domain.Expense
{
    public class Expense_Get_spParams
    {
        public int? ExpenseID { get; set; }
        public int? UserID { get; set; }

    }


    public class Expense_Get_spResult
    {
        public Expense_Get_spResult()
        {
        }
        public int? CanEditFlag { get; set; }
        public int? ExpenseID { get; set; }
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        public string ExpenseDate { get; set; }
        public int? ExpenseTypeCodeID { get; set; }
        public string ExpenseType { get; set; }
        public string VendorName { get; set; }
        public int? StaffPersonID { get; set; }
        public string StaffMember { get; set; }
        public int? EligibleCodeID { get; set; }
        public decimal? Amount { get; set; }
        public decimal? PreviousAmount { get; set; }
        public string Note { get; set; }
        public string AdminNote { get; set; }
        public int? CurrentStatusCodeID { get; set; }
        public string CurrentStatusDate { get; set; }
        public string AttachmentDisplay { get; set; }
        public int? AttachmentCount { get; set; }
        public string StatusHistory { get; set; }
    }
}
