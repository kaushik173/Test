namespace LALoDep.Domain.TitleIVe
{

    public class TitleIVePersonnelGet_spParams
    {
        public int? ErrorID { get; set; }
        public int? InvoiceID { get; set; }
        public int? UserID { get; set; }

    }


    public class TitleIVePersonnelGet_spResult
    {
        public TitleIVePersonnelGet_spResult()
        {
        }
		public string UploadButton { get; set; }
		public int? ReadOnly { get; set; }
		public int? TitleIVePersonnelID { get; set; }
        public int? FromTitleIVeActivityLogID { get; set; }
        public int? TitleIVeInvoiceID { get; set; }
		public int? EmployeePersonID { get; set; }
		public decimal? MonthlySalaryAndBenefits { get; set; }
		public decimal? PercentCACFunds { get; set; }
		public decimal? PercentFFDRPFunds { get; set; }
		public decimal? CACFundAmount { get; set; }
		public decimal? EligibleCost { get; set; }
		public string Note { get; set; }
		public short? RecordStateID { get; set; }
		public string EmployeeName { get; set; }
		public string Title { get; set; }
		public int? OHCode { get; set; }
		public decimal? CountyProgramPercent { get; set; }
		public decimal? EligibleCaseSpecific { get; set; }
		public decimal? EligibleAdmin { get; set; }
		public decimal? EligibleAttorneyTraining { get; set; }
		public decimal? EligibleNonAttorneyTraining { get; set; }
		public decimal? EligibleAmountCaseSpecific { get; set; }
		public decimal? EligibleAmountAdmin { get; set; }
		public decimal? EligibleAmountAttorneyTraining { get; set; }
		public decimal? EligibleAmountNonAttorneyTraining { get; set; }
		public int? DeleteFlag { get; set; }

        public decimal? OverHeadRate { get; set; }
        public int? InsertedByUserID { get; set; }
        public System.Nullable<System.DateTime> InsertedOnDateTime { get; set; }

		public int? AlreadyImported { get; set; }
	}
}
