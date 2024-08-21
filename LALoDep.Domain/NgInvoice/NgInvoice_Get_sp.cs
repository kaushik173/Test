namespace LALoDep.Domain.NgInvoice
{
public class NgInvoice_Get_spParams {
  public int? CaseID { get; set; }
  public int? CaseAgencyID { get; set; }
  public int? YearQuarterID { get; set; }
  public int? ContractorPersonID { get; set; }
  public int? NgInvoiceID { get; set; }
  public int? UserID { get; set; }

}

	
	public class NgInvoice_Get_spResult
    {
		public NgInvoice_Get_spResult()
		{
		}
        public int? NgInvoiceID { get; set; }
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        public int? YearQuarterID { get; set; }
        public int? ContractorPersonID { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public string InvoiceDate { get; set; }
        public string SignatureDate { get; set; }
        public int? SignaturePersonID { get; set; }
        public string SignatureName { get; set; }
        public string InvoiceNote { get; set; }
        public string InvoiceAdminNote { get; set; }
        public int? InvoiceAdminNote_DisplayFlag { get; set; }
        public int? InvoiceAdminNote_CanEditFlag { get; set; }
        public string YearQuarter { get; set; }
        public string Contractor { get; set; }
        public int? InvoiceStatusID { get; set; }
        public int? InvoiceStatusCodeID { get; set; }
        public string InvoiceStatusDate { get; set; }
        public int? AllowSaveFlag { get; set; }
        public string SaveAction { get; set; }
        public string CertificationMessage { get; set; }
        public string SignatureButtonLabel { get; set; }
        public string AdminApprovedNotAllowedAlert { get; set; }
        public System.Nullable<System.DateTime> NgInvoiceSignatureDate { get; set; }
    }
}
