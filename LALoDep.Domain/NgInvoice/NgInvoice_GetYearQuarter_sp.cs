using System;

namespace LALoDep.Domain.NgInvoice
{
public class NgInvoice_GetYearQuarter_spParams {
  public int? PersonID { get; set; }
  public DateTime? CurrentDate { get; set; }
  public string LoadOption { get; set; }
  public int? UserID { get; set; }

}

	
	public class NgInvoice_GetYearQuarter_spResult
	{
		public NgInvoice_GetYearQuarter_spResult()
		{
		}
		public int? YearQuarterID	{ get; set; }
		public string YearQuaterDisplay	{ get; set; }
		public int? Selected	{ get; set; }
		public System.Nullable<System.DateTime> StartDateTime	{ get; set; }
		public System.Nullable<System.DateTime> EndDateTime	{ get; set; }
	}
}
